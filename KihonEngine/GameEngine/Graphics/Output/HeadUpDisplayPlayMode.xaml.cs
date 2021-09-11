using KihonEngine.GameEngine.Configuration;
using KihonEngine.Services;
using System.Linq;
using System.Windows.Controls;

namespace KihonEngine.GameEngine.Graphics.Output
{
    /// <summary>
    /// Interaction logic for HeadUpDisplayPlayMode.xaml
    /// </summary>
    public partial class HeadUpDisplayPlayMode : UserControl
    {
        public HeadUpDisplayPlayMode()
        {
            InitializeComponent();

            var keyboardConfig = Container.Get<IConfigurationService>().GetKeyboardSettings();
            lblMinimalHelp.Text =
                $"Press {keyboardConfig.CancelOperation} to quit. "
                + $"Move with {string.Join(",", keyboardConfig.MoveKeys.Select(x => x.ToString()))}";
        }
    }
}
