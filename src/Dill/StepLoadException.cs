using System;
using System.Collections.Generic;
using System.Text;
using Gherkin.Ast;

namespace Dill
{
    public class StepLoadException : Exception
    {
        public Step Step { get; set; }

        public StepLoadException(Step step, string message) : base(message)
        {
            Step = step;
        }
    }
}
