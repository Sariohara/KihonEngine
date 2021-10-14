using KihonEngine.GameEngine.Graphics.Content;
using KihonEngine.Services;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for ContentSourceWindow.xaml
    /// </summary>
    public partial class ContentSourceWindow : Window
    {
        private IContentService ContentService
            => Container.Get<IContentService>();

        public ContentSourceWindow()
        {
            InitializeComponent();
        }

        private bool _synchronizing;
        private void Synchronize()
        {
            _synchronizing = true;
            var selected = lvSources.SelectedItem as ContentSourceDescription;
            btRemove.IsEnabled = selected != null && selected.Type != "Embedded";
            _synchronizing = false;
        }

        private void LoadSource()
        {
            _synchronizing = true;
            lvSources.ItemsSource = ContentService.GetSources();
            if (lvSources.Items.Count > 0)
            {
                lvSources.SelectedItem = lvSources.Items.GetItemAt(0);
            }
            _synchronizing = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSource();
            Synchronize();
        }

        private void lvModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_synchronizing)
            {
                Synchronize();
            }
        }

        private void btAddFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            dialog.Title = "Add content source";
            dialog.Filter = "Zip files (*.zip)|*.zip|PK3 files (*.pk3)|*.pk3|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                ContentService.RegisterSource(new ZipFileContentSource(dialog.FileName));

                LoadSource();
                Synchronize();
            }
        }

        private void btAddFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderPath = new BrowseForFolder().SelectFolder("Select a data source folder", System.Environment.CurrentDirectory, this);
            if (!string.IsNullOrEmpty(folderPath))
            {
                ContentService.RegisterSource(new FolderContentSource(folderPath));

                LoadSource();
                Synchronize();
            }
        }

        private void btRemove_Click(object sender, RoutedEventArgs e)
        {
            var selected = lvSources.SelectedItem as ContentSourceDescription;
            if (selected != null && MessageBox.Show(
                    "Do you really want to remove the selected content source?", 
                    "Remove content source", 
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                ContentService.RemoveSource(selected);

                LoadSource();
                Synchronize();
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
