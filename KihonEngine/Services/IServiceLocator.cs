
namespace KihonEngine.Services
{
    public interface IServiceLocator
    {
        void Register<TInterface>(TInterface implementation);
        TInterface Get<TInterface>();
    }
}
