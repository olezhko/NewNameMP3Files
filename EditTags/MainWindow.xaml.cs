using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EditTags.Properties;
using EditTags.ViewModel;
using MusicLibrary;

namespace EditTags
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Settings.Default.Save();
        }

        private void SongsGridView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = (MainViewModel)DataContext;
            viewmodel.SelectedItems = SongsGridView.SelectedItems
                .Cast<Song>().ToList();
        }
    }
}