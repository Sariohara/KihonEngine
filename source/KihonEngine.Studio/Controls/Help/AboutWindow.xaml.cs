using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        private const string MinimalContent =
@"
MIT License\r\n
\r\n
Copyright (c) 2021 Nicolas VIEL\r\n";

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
            tbLicense.Text = MinimalContent;

            var filepath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"Content\LICENSE.TXT");

            if (System.IO.File.Exists(filepath))
            {
                tbLicense.Text = System.IO.File.ReadAllText(filepath);
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
