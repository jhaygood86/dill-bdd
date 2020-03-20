using Dill.Infrastructure;
using System;

namespace Dill
{
    public class Dill
    {
        private readonly IServiceProvider _serviceProvider;

        public Dill()
        {
            _serviceProvider = new DillSimpleServiceProvider(this);
        }

        public Dill(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetFeatureContext<T>() where T : FeatureContext
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }

        public object GetFeatureContext(Type contextType)
        {
            if (contextType == null)
            {
                throw new ArgumentNullException(nameof(contextType));
            }

            if (!typeof(FeatureContext).IsAssignableFrom(contextType))
            {
                throw new ArgumentException($"Type {contextType.FullName} is not assignable from {nameof(FeatureContext)} type", nameof(contextType));
            }

            return _serviceProvider.GetService(contextType);
        }

        /// <summary>
        /// Determines whether .feature files are loaded from
        /// </summary>
        public FeatureLoaderType FeatureLoaderType { get; set; } = FeatureLoaderType.File;

        /// <summary>
        /// For files, the base path, and for embedded resources, the base resource
        /// </summary>
        public string FeatureBasePath { get; set; }
    }
}
