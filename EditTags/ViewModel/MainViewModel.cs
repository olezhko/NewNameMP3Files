using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using EditTags.Properties;
using GalaSoft.MvvmLight;
using MusicLibrary;
using GalaSoft.MvvmLight.Command;

namespace EditTags.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _songCollection = new ObservableCollection<Song>();
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                foreach (string t in args)
                {
                    var file = TagLib.File.Create(t);
                    SongsCollection.Add(new Song(file));
                }
            }
            DragCommand = new RelayCommand<DragEventArgs>(DragEnterAuthorsListViewMethod);
        }

        private void DragEnterAuthorsListViewMethod(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                var dragItems = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
                foreach (string item in dragItems)
                {
                    var file = new FileInfo(item);
                    if (file.Exists) // is it file
                    {
                        AddSongToList(item);
                    }
                    else
                    {
                        var dir = new DirectoryInfo(item);
                        if (dir.Exists)
                        {
                            AddDirectoryToList(item);
                        }
                    }
                }
            }
        }

        private void AddDirectoryToList(string direcorypath)
        {
            var files = Directory.EnumerateFiles(direcorypath, "*.*", SearchOption.AllDirectories)
.Where(s => s.EndsWith(".mp3") || s.EndsWith(".m4a") || s.EndsWith(".ogg"));

            foreach (var file in files)
            {
                AddSongToList(file);
            }
        }

        private void AddSongToList(string filepath)
        {
            var mp3File = TagLib.File.Create(filepath);
            SongsCollection.Add(new Song(mp3File));
        }

        #region Public Properties

        private ObservableCollection<Song> _songCollection;
        public ObservableCollection<Song> SongsCollection
        {
            get { return _songCollection; }
        }

        public double GridSplitterPosition
        {
            get { return Settings.Default.GridSpliiterWidth; }
            set
            {
                Settings.Default.GridSpliiterWidth = value;
            }
        }

        public RelayCommand<DragEventArgs> DragCommand
        {
            get;
            private set;
        }

        private List<Song> selectedItems;
        public List<Song> SelectedItems 
        {
            get { return selectedItems; }
            set
            {
                selectedItems = value;
                SelectedItemsTitle = "<Not Change>";
                if (selectedItems.All(o => o.Title == Enumerable.First(selectedItems).Title))
                {
                    SelectedItemsTitle = selectedItems[0].Title;
                }
                RaisePropertyChanged(() => SelectedItemsTitle);
            }
        }
        public string SelectedItemsTitle { get; set; }
        #endregion

    }
}