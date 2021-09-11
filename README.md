# KihonEngine
 
Kihon Engine is a basic 3D video game engine developed in dotnet core and WPF

## Getting started

Launch a video game based on kihon is very basic.

Just take a look inside the sample project `KihonEngine.SampleGame` . 

The whole `MainWindow.xaml.cs` should look something like this:

```csharp
namespace KihonEngine.SampleGame
{
    using System.Windows;
    using System.Windows.Input;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Step 1 : Configure game engine
            Engine.Configure();

            // Step 2 : Configure game : we will use an existing map from KihonEngine.SampleMaps
            StandardStartups.BuildStandaloneFullScreenGame<KihonEngine.SampleMaps.DarkCastleMapBuilder>();

            // Step 3 : When click Start button, launch game
            btnStart.Click += (sender, e) => Engine.Play();

            // And then, manage how application ends
            btnExit.Click += (sender, e) => Close();

            KeyDown += (sender, e) => { if (e.Key == Key.Escape) { Close(); } };
        }
    }
}
```

And that's all. When the application starts, you will have basic startup screen. And when click Start Game, you will launch one of the few sample maps available in `KihonEngine.SampleMaps`

![Screenshot - Splash Screen](https://raw.github.com/nico65535/kihonengine/master/doc/kihonEngine-sampleGame-splashScreen-01.png)

![Screenshot - Walkthrough](https://raw.github.com/nico65535/kihonengine/master/doc/kihonEngine-sampleGame-walkthrough-01.png)

