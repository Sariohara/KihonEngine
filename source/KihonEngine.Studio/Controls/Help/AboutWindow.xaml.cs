using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private TAttribute GetAttribute<TAttribute>(Assembly assembly) where TAttribute : Attribute
        {
            return (TAttribute)Attribute.GetCustomAttribute(assembly, typeof(TAttribute), false);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Assembly infos
            lblStudioProduct.Content = GetAttribute<AssemblyProductAttribute>(Assembly.GetExecutingAssembly()).Product;
            lblStudioVersion.Content = "Version " + Assembly.GetExecutingAssembly().GetName().Version;
            lblStudioCopyright.Content = GetAttribute<AssemblyCopyrightAttribute>(Assembly.GetExecutingAssembly()).Copyright;

            lblEngineProduct.Content = GetAttribute<AssemblyProductAttribute>(typeof(Engine).Assembly).Product;
            lblEngineVersion.Content = "Version" + typeof(Engine).Assembly.GetName().Version;
            lblEngineCopyright.Content = GetAttribute<AssemblyCopyrightAttribute>(typeof(Engine).Assembly).Copyright;

            // License
            var targetAssembly = Assembly.GetExecutingAssembly();
            var assemblyName = targetAssembly.GetName().Name;
            using (var stream = targetAssembly.GetManifestResourceStream($"{assemblyName}.Content.LICENSE.TXT"))
            {
                using (var sr = new StreamReader(stream, Encoding.UTF8))
                {
                    tbLicense.Text = sr.ReadToEnd();
                }
            }
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
