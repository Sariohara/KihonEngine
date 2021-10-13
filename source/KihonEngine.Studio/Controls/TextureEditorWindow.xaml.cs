using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
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
using System.Windows.Shapes;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for TextureEditorWindow.xaml
    /// </summary>
    public partial class TextureEditorWindow : Window
    {
        private string[] AvailableTextures = {
                string.Empty,
                "default.png",
                "floor0.jpg",
                "ki.png",
                "hon.png",
                "steve-front.png",
                "steve-back.png",
                "steve-top.png",
                "steve-left.png",
                "steve-right.png",
            };

        public event EventHandler<TextureMetadata> OnTextureChanged;

        public TextureEditorWindow()
        {
            InitializeComponent();

            cbTileMode.ItemsSource = Enum.GetValues<TileMode>();
            cbStretch.ItemsSource = Enum.GetValues<Stretch>();
            lbox.ItemsSource = GetTextures();
        }

        public TextureMetadata Texture { get; set; }

        bool _synchronizing;
        public void Synchronize()
        {
            _synchronizing = true;
            if (Texture == null)
            {
                Texture = new TextureMetadata { 
                    TileMode = TileMode.Tile,
                    Stretch = Stretch.UniformToFill,
                    RatioX = 1,
                    RatioY = 1,
                };
            }

            cbTileMode.SelectedValue = Texture.TileMode;
            cbStretch.SelectedValue = Texture.Stretch;
            TrySelectTexture(Texture.Name);
            tbRatioX.Text = Texture.RatioX.ToString();
            tbRatioY.Text = Texture.RatioY.ToString();
            _synchronizing = false;
        }

        
        private void TrySelectTexture(string name)
        {
            var textures = GetTextures();
            for (int i = 0; i < textures.Length; i++)
            {
                if (textures[i].Name == name)
                {
                    lbox.SelectedIndex = i;
                    lbox.ScrollIntoView(lbox.SelectedItem);
                    return;
                }
            }

            lbox.SelectedIndex = 0;
        }

        private class TextureViewModel
        {
            public string Name { get; set; }
            public Brush PreviewBrush { get; set; }
        }

        private TextureViewModel CreateTextureViewModel(string filename)
        {
            var result = new TextureViewModel { Name = filename };

            if (string.IsNullOrEmpty(filename))
            {
                result.PreviewBrush = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                result.PreviewBrush = new ImageBrush(ImageHelper.Get($"Textures.{filename}"));
            }

            return result;
        }

        private TextureViewModel[] GetTextures()
        {
            return AvailableTextures
                .Select(x => CreateTextureViewModel(x))
                .ToArray();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Synchronize();
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

        private void lbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_synchronizing)
            {
                Texture.Name = ((TextureViewModel)lbox.SelectedItem).Name;

                if (OnTextureChanged != null)
                {
                    OnTextureChanged(this, Texture);
                }
            }
        }

        private void cbTileMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_synchronizing)
            {
                Texture.TileMode = (TileMode)cbTileMode.SelectedItem;

                if (OnTextureChanged != null)
                {
                    OnTextureChanged(this, Texture);
                }
            }
        }

        private void cbStretch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_synchronizing)
            {
                Texture.Stretch = (Stretch)cbStretch.SelectedItem;

                if (OnTextureChanged != null)
                {
                    OnTextureChanged(this, Texture);
                }
            }
        }

        private void tbRatioX_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            InputHelper.TryUpdateDouble(
                    tbRatioX.Text,
                    ratioX => Texture.RatioX = ratioX);

            if (OnTextureChanged != null)
            {
                OnTextureChanged(this, Texture);
            }
        }

        private void tbRatioY_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            InputHelper.TryUpdateDouble(
                    tbRatioY.Text,
                    ratioY => Texture.RatioY = ratioY);

            if (OnTextureChanged != null)
            {
                OnTextureChanged(this, Texture);
            }
        }
    }
}
