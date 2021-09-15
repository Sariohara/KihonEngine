
using System.Collections.Generic;

namespace KihonEngine.GameEngine.State
{
    public class GameState
    {
        private Dictionary<string, object> _featuredStates;

        public GameState()
        {
            _featuredStates = new Dictionary<string, object>();
        }

        public bool IsStandaloneFullScreenGame { get; set; }

        public T Get<T>() where T : class, new()
        {
            var key = typeof(T).FullName;
            if (!_featuredStates.ContainsKey(key))
            {
                _featuredStates.Add(key, new T());
            }

            return (T)_featuredStates[key];
        }

        public T Reset<T>() where T : class, new()
        {
            var key = typeof(T).FullName;
            if (_featuredStates.ContainsKey(key))
            {
                _featuredStates.Remove(key);
            }

            _featuredStates.Add(key, new T());
            return (T)_featuredStates[key];
        }

        public void Reset()
        {
            _featuredStates.Clear();
        }
    }
}
