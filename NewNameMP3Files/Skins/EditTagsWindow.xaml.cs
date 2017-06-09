using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MusicLibrary;
using NewNameMP3Files.Properties;
using NewNameMP3Files.ViewModel;

namespace NewNameMP3Files.Skins
{
    /// <summary>
    /// Interaction logic for EditTagsWindow.xaml
    /// </summary>
    public partial class EditTagsWindow : Window
    {
        public EditTagsWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void SongsGridView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = (EditTagsViewModel)DataContext;
            viewmodel.SelectedItems = SongsGridView.SelectedItems
                .Cast<SongViewModel>().ToList();
        }
    }
}
