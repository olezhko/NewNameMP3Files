using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using MusicLibrary;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using NewNameMP3Files.Properties;

namespace NewNameMP3Files.ViewModel
{
    public class MusicLibraryViewModel : ViewModelBase
    {
        public MusicLibraryViewModel()
        {
            MusicLibraryList = new ObservableCollection<Author>();

            MouseRightButtonUpCommand = new RelayCommand<MouseEventArgs>(e =>
            {
                var element = e.Source;
                var parent = VisualTreeHelper.GetParent((DependencyObject)element) as UIElement;
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
                parent = VisualTreeHelper.GetParent(parent) as UIElement;

                var treeview = parent as TreeViewItem;
                if (treeview != null)
                {
                    treeview.IsExpanded = !treeview.IsExpanded;
                }

            });
            LoadLibrary(Settings.Default.MusicLibraryPath);
        }

        private void LoadLibrary(string librarypath)
        {
            
        }

        public ObservableCollection<Author> MusicLibraryList { get; set; }

        public RelayCommand<MouseEventArgs> MouseRightButtonUpCommand
        {
            get;
            private set;
        }

    }
}
