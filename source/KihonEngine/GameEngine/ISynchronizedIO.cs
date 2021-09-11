using KihonEngine.GameEngine.State;

namespace KihonEngine.GameEngine
{
    public interface ISynchronizedIO
    {
        void Synchronize(IGameEngineState state);
    }
}
