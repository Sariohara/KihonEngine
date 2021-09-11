using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for ViewportHost.xaml
    /// </summary>
    public partial class ViewportHost : UserControl
    {
        public ViewportHost()
        {
            InitializeComponent();
        }

        public void AttachViewport(UserControl control)
        {
            grid.Children.Add(control);
        }
    }
}
