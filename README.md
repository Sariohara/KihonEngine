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


## Kihon Engine Studio

Kihon Engine Studio helps for building maps

The actual basic features are the following

Adding 3D models to maps
* Add floor
* Add ceilings
* Add walls
* Add volumes, like cube, rectangles
* Add lights to make the viewport3D scene visible
* Add skyboxes. Actually, three predefined skyboxes are availables

Possibility to edit the 3D models
* By dimentions
* By colors
* As proof of concept, actually only four textures are available for floors

Possibility to mode 3D models on the map
* By moving on axis X, Y and Z
* By translation on axis X, Y and Z

Possibility to change map global properties
* map name
* Player camera respawn position
* Player camera respawn direction

And some other features 
* Save and load maps from files from a json format
* Source viewer for the currently edited map, in order to visualize the json format
* Game state viewer to analyse game state at any time
* Possibility to playswitch between edit mode and game mode in order to vizualise how map is rendered at play time

