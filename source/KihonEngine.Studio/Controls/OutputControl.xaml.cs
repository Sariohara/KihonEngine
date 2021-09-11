using KihonEngine.Services;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for OutputControl.xaml
    /// </summary>
    public partial class OutputControl : UserControl, ILogListener
    {
        private List<string> logCache = new List<string>();
        private ConcurrentQueue<string> logBuffer = new ConcurrentQueue<string>();
        private int maxLogsInCache = 1000;

        private Timer timer;
        private int timerIntervalInMilliseconds = 200;
        private bool processing;

        public OutputControl()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = timerIntervalInMilliseconds;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public void Log(string message)
        {
            logBuffer.Enqueue(message);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Closing += window_Closing;
        }

        private void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!processing)
            {
                try
                {
                    processing = true;

                    var counter = logBuffer.Count;

                    if (counter > 0)
                    {
                        // Add logs from buffer
                        for (int i = 0; i < counter; i++)
                        {
                            if (!logBuffer.TryDequeue(out var message))
                            {
                                logBuffer.Clear();
                                logCache.Add("ERROR : error when calling logBuffer.TryDequeue. Clear log buffer and continue.");
                                break;
                            }

                            logCache.Add(message);
                        }

                        // Remove old logs from cache
                        if (logCache.Count >= maxLogsInCache)
                        {
                            var countToRemove = logCache.Count - maxLogsInCache + 1;
                            for (int i = 0; i < countToRemove; i++)
                            {
                                logCache.RemoveAt(0);
                            }
                        }

                        var textBoxContent = string.Join("\n", logCache) + "\n";

                        // Display
                        if (timer.Enabled)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                textBox.Text = textBoxContent;
                                textBox.ScrollToEnd();
                            });
                        }
                    }
                }
                finally
                {
                    processing = false;
                }
            }
        }
    }
}
