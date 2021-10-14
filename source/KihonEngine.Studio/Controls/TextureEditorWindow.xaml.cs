using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System;
using KihonEngine.GameEngine.Graphics.Content;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using KihonEngine.Services;
using System.Collections.Generic;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for TextureEditorWindow.xaml
    /// </summary>
    public partial class TextureEditorWindow : Window
    {
        private IContentService ContentService
            => Container.Get<IContentService>();

        public event EventHandler<TextureMetadata> OnTextureChanged;

        private List<string> _textures;
        private List<string> _directories;

        public TextureEditorWindow()
        {
            InitializeComponent();

            cbTileMode.ItemsSource = Enum.GetValues<TileMode>();
            cbStretch.ItemsSource = Enum.GetValues<Stretch>();

            LoadTextures();
        }

        //private void ShowProgressBar()
        //{
        //    progressBar.Visibility = Visibility.Visible;
        //    lbox.Visibility = Visibility.Hidden;
        //}

        //private void HideProgressBar()
        //{
        //    progressBar.Visibility = Visibility.Hidden;
        //    lbox.Visibility = Visibility.Visible;
        //}

        private void LoadTextures()
        {
            // Load textures
            _textures = new List<string>();
            _textures.Add(string.Empty);
            _textures.AddRange(ContentService.GetResources(GraphicContentType.Texture));

            // Get directories
            _directories = new List<string>();
            foreach (var texture in _textures)
            {
                var folderName = GetFolderName(texture);

                if (!_directories.Contains(folderName))
                {
                    _directories.Add(folderName);
                }
            }

            _directories.Sort();

            treeView.Items.Clear();
            foreach (var directory in _directories)
            {
                var directoryNode = new TreeViewItem { Header = directory };
                treeView.Items.Add(directoryNode);
            }
        }

        private string GetFolderName(string texture)
        {
            if (string.IsNullOrEmpty(texture))
            {
                return string.Empty;
            }

            var folderEndIndex = texture.LastIndexOf('/');
            if (folderEndIndex > 0)
            {
                return texture.Substring(0, folderEndIndex);
            }

            return string.Empty;
        }

        public TextureMetadata Texture { get; set; }

        bool _synchronizing;
        public void Synchronize()
        {
            _synchronizing = true;
            if (Texture == null)
            {
                Texture = new TextureMetadata {
                    Name = string.Empty,
                    TileMode = TileMode.Tile,
                    Stretch = Stretch.UniformToFill,
                    RatioX = 1,
                    RatioY = 1,
                };
            }

            if (Texture.Name == null)
            {
                Texture.Name = string.Empty;
            }

            cbTileMode.SelectedValue = Texture.TileMode;
            cbStretch.SelectedValue = Texture.Stretch;
            TrySelectTexture();
            tbRatioX.Text = Texture.RatioX.ToString();
            tbRatioY.Text = Texture.RatioY.ToString();
            _synchronizing = false;
        }

        private void LoadDirectory(string directory)
        {
            var textures = new List<string>();
            if (directory == string.Empty)
            {
                
                textures.AddRange(_textures.Where(x => x.IndexOf('/') == -1));
            }
            else
            {
                textures.AddRange(_textures.Where(x => x.StartsWith(directory)));
            }

            _synchronizing = true;
            lbox.ItemsSource = textures
                .Select(x => CreateTextureViewModel(x, directory))
                .ToArray();
            _synchronizing = false;

            var matched = false;
            foreach (var item in lbox.Items)
            {
                var viewModel = (TextureViewModel)item;
                if (viewModel.Name == Texture.Name)
                {
                    lbox.SelectedItem = viewModel;
                    lbox.ScrollIntoView(viewModel);
                    matched = true;
                    break;
                }
            }

            if (!matched && lbox.Items.Count > 0)
            {
                lbox.ScrollIntoView(lbox.Items.GetItemAt(0));
                lbox.ScrollIntoView(lbox.Items.GetItemAt(0));
            }

            lblStatus.Text = $"{_textures.Count} texture(s) in total.{Environment.NewLine}{lbox.Items.Count} texture(s) loaded in selected folder";
        }
        
        private void TrySelectTexture()
        {
            // Select tree view node
            var folderName = GetFolderName(Texture.Name);
            var match = false;
            foreach(var item in treeView.Items)
            {
                if (((TreeViewItem)item).Header.ToString() == folderName)
                {
                    ((TreeViewItem)item).IsSelected = true;
                    ((TreeViewItem)item).BringIntoView();
                    match = true;
                    break;
                }
            }

            if (!match)
            {
                var item = (TreeViewItem)treeView.Items.GetItemAt(0);
                item.IsSelected = true;
                folderName = item.Header.ToString();
                item.BringIntoView();
            }

            LoadDirectory(folderName);
        }

        private TextureViewModel CreateTextureViewModel(string filename, string directory)
        {
            var result = new TextureViewModel { Name = filename, Directory = directory, ShortName = filename };

            if (string.IsNullOrEmpty(filename))
            {
                result.PreviewBrush = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                result.PreviewBrush = new ImageBrush(ImageHelper.Get(GraphicContentType.Texture, filename));

                if (!string.IsNullOrEmpty(directory))
                {
                    result.ShortName = result.Name.Substring(result.Directory.Length + 1);
                }
            }

            return result;
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

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!_synchronizing)
            {
                var node = (TreeViewItem)e.NewValue;
                LoadDirectory(node.Header.ToString());
            }
        }

        private class TextureViewModel
        {
            public string Name { get; set; }
            public string Directory { get; set; }
            public string ShortName { get; set; }
            public Brush PreviewBrush { get; set; }
        }
    }
}
