
using KihonEngine.GameEngine.InputControls;

namespace KihonEngine.GameEngine.Configuration
{
    public interface IConfigurationService
    {
        MouseSettings GetMouseSettings();
        KeyboardSettings GetKeyboardSettings();
    }
}
