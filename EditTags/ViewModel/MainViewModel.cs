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
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var dragItems = (string[])e.Data.GetData(DataFormats.FileDrop);
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

        private readonly ObservableCollection<Song> _songCollection;
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

        private List<Song> _selectedItems;
        public List<Song> SelectedItems 
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                
                SelectedItemsTitle = "<Not Change>";
                if (_selectedItems.All(o => o.Title == _selectedItems.First().Title))
                {
                    SelectedItemsTitle = _selectedItems[0].Title;
                }

                SelectedItemsArtist = "<Not Change>";
                if (_selectedItems.All(o => o.Artist == _selectedItems.First().Artist))
                {
                    SelectedItemsArtist = _selectedItems[0].Artist;
                }

                SelectedItemsAlbum = "<Not Change>";
                if (_selectedItems.All(o => o.Album == _selectedItems.First().Album))
                {
                    SelectedItemsAlbum = _selectedItems[0].Album;
                }

                SelectedItemsGenre = "<Not Change>";
                if (_selectedItems.All(o => o.Genre == _selectedItems.First().Genre))
                {
                    SelectedItemsGenre = _selectedItems[0].Genre;
                }

                SelectedItemsNumber = "<Not Change>";
                if (_selectedItems.All(o => o.Number == _selectedItems.First().Number))
                {
                    SelectedItemsNumber = _selectedItems[0].Number.ToString();
                }

                SelectedItemsYear = "<Not Change>";
                if (_selectedItems.All(o => o.Year == _selectedItems.First().Year))
                {
                    SelectedItemsYear = _selectedItems[0].Year.ToString();
                }                           
            }
        }

        private Song _resultSelectedSong = new Song();

        public string SelectedItemsTitle 
        {
            get { return _resultSelectedSong.Title; }
            set { _resultSelectedSong.Title = value; RaisePropertyChanged(() => SelectedItemsTitle); } 
        }
        public string SelectedItemsNumber
        {
            get { return _resultSelectedSong.Number.ToString(); }
            set
            {
                try
                {
                    _resultSelectedSong.Number = Convert.ToUInt32(value);
                    RaisePropertyChanged(() => SelectedItemsNumber);
                }
                catch (Exception)
                {

                }           
            }
        }
        public string SelectedItemsArtist
        {
            get { return _resultSelectedSong.Artist; }
            set { _resultSelectedSong.Artist = value; RaisePropertyChanged(() => SelectedItemsArtist); }
        }
        public string SelectedItemsAlbum
        {
            get { return _resultSelectedSong.Album; }
            set { _resultSelectedSong.Album = value; RaisePropertyChanged(() => SelectedItemsAlbum); }
        }
        public string SelectedItemsGenre
        {
            get { return _resultSelectedSong.Genre; }
            set { _resultSelectedSong.Genre = value; RaisePropertyChanged(() => SelectedItemsGenre); }
        }
        public string SelectedItemsYear
        {
            get { return _resultSelectedSong.Year.ToString(); }
            set
            {
                try
                {
                    _resultSelectedSong.Year = Convert.ToUInt32(value);
                    RaisePropertyChanged(() => SelectedItemsYear);
                }
                catch (Exception)
                {

                }
            }
        }
        #endregion

    }
}