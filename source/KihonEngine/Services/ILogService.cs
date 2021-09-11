
namespace KihonEngine.Services
{
    public interface ILogService
    {
        void Log(string message);

        void AddListener(ILogListener listener);
    }
}
