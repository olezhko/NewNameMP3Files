using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using MusicLibrary;
using NewNameMP3Files.Properties;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace NewNameMP3Files.ViewModel
{
    public class EditTagsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public EditTagsViewModel()
        {
            _songCollection = new ObservableCollection<SongViewModel>();
            DragCommand = new RelayCommand<DragEventArgs>(DragEnterAuthorsListViewMethod);
            SaveCommand = new RelayCommand(SaveMethod);
            SelectedItemsImageSource = new BitmapImage(Song.NoCoverImage);
            KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDownMethod);
        }

        private void KeyDownMethod(KeyEventArgs obj)
        {
            if (obj.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SaveFiles();
            }
        }

        private void SaveFiles()
        {
            foreach (var song in SongsCollection)
            {
                song.Save();
            }
            MessageBox.Show("Saved");
        }

        private void SaveMethod()
        {
            if (SelectedItems != null && SelectedItems.Count != 0)
            {
                foreach (var selectedItem in SelectedItems)
                {
                    if (SelectedItemsAlbum != DefaultValue)
                    {
                        selectedItem.Album = SelectedItemsAlbum;
                    }

                    if (SelectedItemsArtist != DefaultValue)
                    {
                        selectedItem.Artist = SelectedItemsArtist;
                    }

                    if (SelectedItemsGenre != DefaultValue)
                    {
                        selectedItem.Genre = SelectedItemsGenre;
                    }

                    if (SelectedItemsYear != DefaultValue)
                    {
                        selectedItem.Year = Convert.ToUInt32(SelectedItemsYear);
                    }

                    if (SelectedItemsNumber != DefaultValue)
                    {
                        selectedItem.Number = Convert.ToUInt32(SelectedItemsNumber);
                    }

                    if (SelectedItemsLyrics != DefaultValue)
                    {
                        selectedItem.Lyric = SelectedItemsLyrics;
                    }

                    if (SelectedItemsTitle != DefaultValue)
                    {
                        selectedItem.Title = SelectedItemsTitle;
                    }
                    selectedItem.ClearBadTags();
                    selectedItem.Save();
                }
            }
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
            var files = Directory.EnumerateFiles(direcorypath, "*.*", SearchOption.AllDirectories).Where(Song.IsFileSong);

            foreach (var file in files)
            {
                AddSongToList(file);
            }
        }

        private void AddSongToList(string filepath)
        {
            if (!Song.IsFileSong(filepath))
            {
                return;
            }
            File.SetAttributes(filepath, FileAttributes.Normal);
            var mp3File = TagLib.File.Create(filepath);
            SongsCollection.Add(new SongViewModel(new Song(mp3File)));
        }

        #region Public Properties
        private readonly ObservableCollection<SongViewModel> _songCollection;
        public ObservableCollection<SongViewModel> SongsCollection
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

        public double NumberGridWidth
        {
            get { return Settings.Default.NumberGridWidth; }
            set
            {
                Settings.Default.NumberGridWidth = value;
            }
        }

        public double AlbumGridWidth
        {
            get { return Settings.Default.AlbumGridWidth; }
            set
            {
                Settings.Default.AlbumGridWidth = value;
            }
        }

        public double ArtistGridWidth
        {
            get { return Settings.Default.ArtistGridWidth; }
            set
            {
                Settings.Default.ArtistGridWidth = value;
            }
        }

        public double YearGridWidth
        {
            get { return Settings.Default.YearGridWidth; }
            set
            {
                Settings.Default.YearGridWidth = value;
            }
        }

        public double GenreGridWidth
        {
            get { return Settings.Default.GenreGridWidth; }
            set
            {
                Settings.Default.GenreGridWidth = value;
            }
        }

        public double TitleGridWidth
        {
            get { return Settings.Default.TitleGridWidth; }
            set
            {
                Settings.Default.TitleGridWidth = value;
            }
        }

        public double AudioBitrateGridWidth
        {
            get { return Settings.Default.AudioBitrateGridWidth; }
            set
            {
                Settings.Default.AudioBitrateGridWidth = value;
            }
        }

        public double PathGridWidth
        {
            get { return Settings.Default.PathGridWidth; }
            set
            {
                Settings.Default.PathGridWidth = value;
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand<KeyEventArgs> KeyDownCommand { get; private set; }
        public RelayCommand<DragEventArgs> DragCommand
        {
            get;
            private set;
        }



        private const string DefaultValue = "<Not Change>";
        private List<SongViewModel> _selectedItems;
        public List<SongViewModel> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                if (_selectedItems.Count == 0)
                {
                    return;
                }
                SelectedItemsTitle = DefaultValue;
                if (_selectedItems.All(o => o.Title == _selectedItems.First().Title))
                {
                    SelectedItemsTitle = _selectedItems[0].Title;
                }

                SelectedItemsArtist = DefaultValue;
                if (_selectedItems.All(o => o.Artist == _selectedItems.First().Artist))
                {
                    SelectedItemsArtist = _selectedItems[0].Artist;
                }

                SelectedItemsAlbum = DefaultValue;
                if (_selectedItems.All(o => o.Album == _selectedItems.First().Album))
                {
                    SelectedItemsAlbum = _selectedItems[0].Album;
                }

                SelectedItemsGenre = DefaultValue;
                if (_selectedItems.All(o => o.Genre == _selectedItems.First().Genre))
                {
                    SelectedItemsGenre = _selectedItems[0].Genre;
                }

                SelectedItemsNumber = DefaultValue;
                if (_selectedItems.All(o => o.Number == _selectedItems.First().Number))
                {
                    SelectedItemsNumber = _selectedItems[0].Number.ToString();
                }

                SelectedItemsYear = DefaultValue;
                if (_selectedItems.All(o => o.Year == _selectedItems.First().Year))
                {
                    SelectedItemsYear = _selectedItems[0].Year.ToString();
                }

                SelectedItemsLyrics = DefaultValue;
                if (_selectedItems.All(o => o.Lyric == _selectedItems.First().Lyric))
                {
                    SelectedItemsLyrics = _selectedItems[0].Lyric;
                }

                SelectedItemsPath = DefaultValue;
                if (_selectedItems.All(o => Path.GetDirectoryName(o.Path) == Path.GetDirectoryName(_selectedItems.First().Path)))
                {
                    SelectedItemsPath = Path.GetDirectoryName(_selectedItems[0].Path);
                }

                SelectedItemsImageSource = null;
                SelectedItemsImageSource = _selectedItems.All(o => Song.GetCoverPath(o.Path) == Song.GetCoverPath(_selectedItems.First().Path)) ? new BitmapImage(Song.GetCoverPath(_selectedItems[0].Path)) : new BitmapImage(Song.NoCoverImage);
            }
        }

        private string _selectedItemsTitle;
        public string SelectedItemsTitle
        {
            get { return _selectedItemsTitle; }
            set { _selectedItemsTitle = value; RaisePropertyChanged(() => SelectedItemsTitle); }
        }

        private string _selectedItemsNumber;
        public string SelectedItemsNumber
        {
            get { return _selectedItemsNumber; }
            set
            {
                try
                {
                    _selectedItemsNumber = value;
                    RaisePropertyChanged(() => SelectedItemsNumber);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private string _selectedItemsArtist;
        public string SelectedItemsArtist
        {
            get { return _selectedItemsArtist; }
            set { _selectedItemsArtist = value; RaisePropertyChanged(() => SelectedItemsArtist); }
        }

        private string _selectedItemsAlbum;
        public string SelectedItemsAlbum
        {
            get { return _selectedItemsAlbum; }
            set { _selectedItemsAlbum = value; RaisePropertyChanged(() => SelectedItemsAlbum); }
        }

        private string _selectedItemsGenre;
        public string SelectedItemsGenre
        {
            get { return _selectedItemsGenre; }
            set { _selectedItemsGenre = value; RaisePropertyChanged(() => SelectedItemsGenre); }
        }

        private string _selectedItemsYear;
        public string SelectedItemsYear
        {
            get { return _selectedItemsYear; }
            set
            {
                try
                {
                    _selectedItemsYear = value;
                    RaisePropertyChanged(() => SelectedItemsYear);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private string _selectedItemsPath;
        public string SelectedItemsPath
        {
            get { return _selectedItemsPath; }
            set
            {
                try
                {
                    _selectedItemsPath = value;
                    RaisePropertyChanged(() => SelectedItemsPath);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private string _selectedItemsLyrics;
        public string SelectedItemsLyrics
        {
            get { return _selectedItemsLyrics; }
            set
            {
                try
                {
                    _selectedItemsLyrics = value;
                    RaisePropertyChanged(() => SelectedItemsLyrics);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private BitmapImage _selectedItemsImageSource;
        public BitmapImage SelectedItemsImageSource
        {
            get { return _selectedItemsImageSource; }
            set
            {
                _selectedItemsImageSource = value;
                RaisePropertyChanged(() => SelectedItemsImageSource);
            }
        }
        #endregion
    }
}
