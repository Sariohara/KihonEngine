using System;
using System.Collections.Generic;

namespace KihonEngine.Services
{
    public class LogService : ILogService
    {
        private List<ILogListener>  listeners = new List<ILogListener>();

        public void Log(string message)
        {
            var fullMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}:{message}";

            listeners.ForEach(x => x.Log(fullMessage));
        }

        public void AddListener(ILogListener listener)
        {
            listeners.Add(listener);
        }
    }
}
