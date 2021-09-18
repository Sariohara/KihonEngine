using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihonEngine.Services
{
    public class FileLogListener : ILogListener
    {
        private string _filepath;

        public FileLogListener(string filepath)
        {
            _filepath = filepath;
        }

        public void Log(string message)
        {
            System.IO.File.AppendAllLines(_filepath, new[] { message });
        }
    }
}
