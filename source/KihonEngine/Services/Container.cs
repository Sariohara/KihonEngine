
namespace KihonEngine.Services
{
    public static class Container
    {
        private static IServiceLocator _serviceLocator;
        public static void Initialize(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public static TInterface Get<TInterface>()
        {
            return _serviceLocator.Get<TInterface>();
        }
    }
}
