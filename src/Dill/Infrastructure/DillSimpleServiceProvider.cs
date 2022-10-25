using System;

namespace Dill.Infrastructure
{
    class DillSimpleServiceProvider : IServiceProvider
    {
        private readonly Dill _dillInstance;

        public DillSimpleServiceProvider(Dill dillInstance)
        {
            _dillInstance = dillInstance;
        }

        public object GetService(Type serviceType)
        {
            return Activator.CreateInstance(serviceType, _dillInstance);
        }
    }
}
