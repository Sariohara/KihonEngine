# KihonEngine
 
Kihon Engine is a basic 3D engine developed in dotnet core and WPF. It can be used for developpinng basic 3D video games.

## Getting started

Create a video game based on Kihon Engine is very easy.

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

And that's all. When the application starts, you will have basic startup screen. 

![Screenshot - Splash Screen](https://raw.github.com/nico65535/kihonengine/master/doc/kihonEngine-sampleGame-splashScreen-01.png)

And by click Start Game, you will launch one of the few sample maps available in `KihonEngine.SampleMaps`

![Screenshot - Walkthrough](https://raw.github.com/nico65535/kihonengine/master/doc/kihonEngine-sampleGame-walkthrough-01.png)

## Kihon Engine Studio

Kihon Engine Studio helps for building maps

The actual basic features are the following

Some generic features 
* Save and load maps from files from a json format
* ossibility to change map global properties like map name, respawn player camera (position and direction)
* Source viewer for the currently edited map, in order to visualize the json format
* Game state viewer to analyse game state at any time
* Possibility to switch between edit mode and game mode in order to vizualise how map is rendered at play time

![Screenshot - Edit 3D map](https://raw.github.com/nico65535/kihonengine/master/doc/kihonEngine-studio-editMap-01.png)

![Screenshot - Playt on 3D map](https://raw.github.com/nico65535/kihonengine/master/doc/kihonEngine-studio-playMap-01.png)

Adding 3D models to maps
* Add floor
* Add ceilings
* Add walls
* Add volumes, like cube, rectangles
* Add lights to make the viewport3D scene visible
* Add skyboxes. Actually, three predefined skyboxes are availables

![Screenshot - Add 3D models](https://raw.github.com/nico65535/kihonengine/master/doc/kihonEngine-studio-add3dModels-01.png)

Possibility to edit the 3D models
* By dimentions
* By colors
* As proof of concept, actually only four textures are available for floors

![Screenshot - Edit 3D models](https://raw.github.com/nico65535/kihonengine/master/doc/kihonEngine-studio-edit3dModels-01.png)

Possibility to mode 3D models on the map
* By moving on axis X, Y and Z
* By translation on axis X, Y and Z

![Screenshot - Move 3D models](https://raw.github.com/nico65535/kihonengine/master/doc/kihonEngine-studio-move3dModels-01.png)
