using Dill.Attributes;
using Gherkin;
using Gherkin.Ast;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dill
{
    public abstract class FeatureContext
    {
        private readonly Dill _dillInstance;
        private readonly GherkinDocument _gherkinDocument;

        protected FeatureContext(Dill dillInstance, string featureFileName)
        {
            _dillInstance = dillInstance;

            string featureFile = string.Empty;

            if (dillInstance.FeatureLoaderType == FeatureLoaderType.EmbeddedResource)
            {
                featureFile = LoadResource(featureFileName);
            }

            if (dillInstance.FeatureLoaderType == FeatureLoaderType.File)
            {
                featureFile = LoadFile(featureFileName);
            }

            using var sr = new StringReader(featureFile);
            var parser = new Parser();
            _gherkinDocument = parser.Parse(sr);
        }

        private string LoadResource(string featureFileName)
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            using var resource = assembly.GetManifestResourceStream(_dillInstance.FeatureBasePath + "." + featureFileName);
            using var sr = new StreamReader(resource);
            return sr.ReadToEnd();
        }

        private static string LoadFile(string featureFileName)
        {
            using var file = new FileStream(featureFileName, FileMode.Open);
            using var sr = new StreamReader(file);
            return sr.ReadToEnd();
        }

        public async Task ExecuteScenarioAsync(string scenarioName)
        {
            var scenario = _gherkinDocument.Feature.Children.OfType<Scenario>().SingleOrDefault(x => x.Name == scenarioName);

            if (scenario == null)
            {
                throw new ArgumentOutOfRangeException(nameof(scenarioName),"Provided scenario name could not be found");
            }

            if (scenario.Examples.Any())
            {
                var exampleSet = scenario.Examples;

                foreach (var examples in exampleSet)
                {
                    foreach (var exampleRow in examples.TableBody)
                    {
                        await ExecuteSteps(scenario.Steps, exampleRow, examples.TableHeader);
                    }
                }
            }
            else
            {
                await ExecuteSteps(scenario.Steps);
            }
        }

        private async Task ExecuteSteps(IEnumerable<Step> steps, TableRow exampleRow = null, TableRow exampleHeader = null)
        {
            foreach (var step in steps)
            {
                switch (step.Keyword.Trim())
                {
                    case "Given":
                        await ExecuteStep<GivenAttribute>(step,exampleRow,exampleHeader);
                        break;
                    case "And":
                        await ExecuteStep<AndAttribute>(step,exampleRow,exampleHeader);
                        break;
                    case "When":
                        await ExecuteStep<WhenAttribute>(step,exampleRow,exampleHeader);
                        break;
                    case "Then":
                        await ExecuteStep<ThenAttribute>(step,exampleRow,exampleHeader);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown Keyword");
                }
            }
        }

        private async Task ExecuteStep<TAttribute>(Step step, TableRow exampleRow, TableRow exampleHeader) where TAttribute : DillStepAttribute
        {
            var stepText = GetStepText(step, exampleRow, exampleHeader);

            var methods = GetType().GetMethods().Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(TAttribute))).ToList();

            var method = methods.Where(x => x.GetCustomAttribute<TAttribute>() != null).SingleOrDefault(x => Regex.IsMatch(stepText, x.GetCustomAttribute<TAttribute>().Value));

            if (method == null)
            {
                throw new StepLoadException(step,$"No method in {GetType().Name} implements Step: '{step.Keyword.Trim()} {step.Text}'");
            }

            var argumentMatches = Regex.Match(stepText, method.GetCustomAttribute<TAttribute>().Value);

            List<object> arguments = new();

            var parameters = method.GetParameters();

            if (argumentMatches.Groups.Count != parameters.Length + 1)
            {
                throw new StepLoadException(step, $"Parameter count of method {method.Name} does not equal number of arguments");
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                var groupIndex = i + 1;

                if (groupIndex >= argumentMatches.Groups.Count)
                {
                    throw new StepLoadException(step,"Argument could not be found for parameter: " + parameters[i].Name);
                }

                var argument = argumentMatches.Groups[groupIndex].Value;
                var value = Convert.ChangeType(argument, parameters[i].ParameterType, CultureInfo.InvariantCulture);
                arguments.Add(value);
            }

            if (arguments.Count != parameters.Length)
            {
                throw new StepLoadException(step,$"Parameter count of method {method.Name} does not equal number of arguments");
            }

            object[] args = arguments.ToArray();

            if (method.ReturnType == typeof(Task))
            {
                var task = (Task) method.Invoke(this, args);
                await task;
            }
            else
            {
                method.Invoke(this, args);
            }
        }

        private static string GetStepText(Step step, TableRow exampleRow, TableRow exampleHeader)
        {
            var stepText = step.Text;

            if (exampleRow != null && exampleHeader != null)
            {
                for (var i = 0; i < exampleHeader.Cells.Count(); i++)
                {
                    var argumentName = exampleHeader.Cells.ElementAt(i).Value;
                    var argument = exampleRow.Cells.ElementAt(i).Value;

                    var stepTextArgumentName = "<" + argumentName + ">";
                    stepText = stepText.Replace(stepTextArgumentName, argument);
                }
            }
            
            return stepText;
        }
    }
}
