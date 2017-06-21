using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MusicLibrary
{
    public class MusicLibrary
    {
        public static System.Drawing.Icon GetFileIcon(string name, IconReader.IconSize size,
            bool linkOverlay)
        {
            Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
            uint flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

            if (true == linkOverlay) flags += Shell32.SHGFI_LINKOVERLAY;
            /* Check the size specified for return. */
            if (IconReader.IconSize.Small == size)
            {
                flags += Shell32.SHGFI_SMALLICON; // include the small icon flag
            }
            else
            {
                flags += Shell32.SHGFI_LARGEICON; // include the large icon flag
            }

            Shell32.SHGetFileInfo(name,
                Shell32.FILE_ATTRIBUTE_NORMAL,
                ref shfi,
                (uint) System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                flags);


            // Copy (clone) the returned icon to a new object, thus allowing us 
            // to call DestroyIcon immediately
            System.Drawing.Icon icon = (System.Drawing.Icon)
                System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
            User32.DestroyIcon(shfi.hIcon); // Cleanup
            return icon;
        }
    }

    public class Author : ViewModelBase
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public string AuthorName { get; set; }

        public ObservableCollection<Album> AlbumCollection { get; set; }

        public Author(string authorName)
        {
            AuthorName = authorName;
            AlbumCollection = new ObservableCollection<Album>();
        }

        public void AddAlbum(Album album)
        {
            album.PropertyChanged += Album_PropertyChanged;
            AlbumCollection.Add(album);
        }

        void Album_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsSelected = AlbumCollection.All(album => album.IsSelected);
        }
    }

    public class Album : ViewModelBase
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public string AlbumName { get; set; }
        public string AuthorName { get; set; }
        public ObservableCollection<SongViewModel> SongsCollection { get; set; }
        public Uri AlbumCover { get; set; }



        public Album(string albumName, string authorName)
        {
            AuthorName = authorName;
            AlbumName = albumName;
            SongsCollection = new ObservableCollection<SongViewModel>();
        }

        public Album(string albumName, string albumCover, string authorName)
        {
            AuthorName = authorName;
            AlbumCover = new Uri(albumCover);
            AlbumName = albumName;
            SongsCollection = new ObservableCollection<SongViewModel>();
        }

        public Album(string albumName, Uri albumCover, string authorName)
        {
            AuthorName = authorName;
            AlbumCover = albumCover;
            AlbumName = albumName;
            SongsCollection = new ObservableCollection<SongViewModel>();
        }

        public void AddSong(SongViewModel song)
        {
            song.PropertyChanged += Song_PropertyChanged;
            SongsCollection.Add(song);
        }

        public void AddSong(TagLib.File file)
        {
            var song = new SongViewModel(new Song(file));
            song.PropertyChanged += Song_PropertyChanged;
            SongsCollection.Add(song);
        }

        void Song_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsSelected = SongsCollection.All(song => song.IsSelected);
        }

        public void RemoveSong(TagLib.File file)
        {

        }
    }

    public class SongViewModel : ViewModelBase
    {
        public string Name
        {
            get { return _song.Name; }
        }

        public int AudioBitrate
        {
            get { return _song.AudioBitrate; }
        }

        public uint Number
        {
            get { return _song.Number; }
            set
            {
                _song.Number = value;
                RaisePropertyChanged(() => Number);
            }
        }

        public string Title
        {
            get { return _song.Title; }
            set
            {
                _song.Title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public string Artist
        {
            get { return _song.Artist; }
            set
            {
                _song.Artist = value;
                RaisePropertyChanged(() => Artist);
            }
        }

        public string Album
        {
            get { return _song.Album; }
            set
            {
                _song.Album = value;
                RaisePropertyChanged(() => Album);
            }
        }

        public string Genre
        {
            get { return _song.Genre; }
            set
            {
                _song.Genre = value;
                RaisePropertyChanged(() => Genre);
            }
        }

        public uint Year
        {
            get { return _song.Year; }
            set
            {
                _song.Year = value;
                RaisePropertyChanged(() => Year);
            }
        }

        public TimeSpan Duration
        {
            get { return _song.Duration; }
        }

        public string Lyric
        {
            get { return _song.Lyric; }
            set
            {
                _song.Lyric = value;
                RaisePropertyChanged(() => Lyric);
            }
        }

        public string Path
        {
            get { return _song.Path; }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public BitmapSource Icon
        {
            get
            {
                var icon = MusicLibrary.GetFileIcon(Path, IconReader.IconSize.Small, false);
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
        }

        private Song _song;

        public SongViewModel(Song song)
        {
            _song = song;
        }

        public bool Save()
        {
            return _song.Save();
        }

        public void ClearBadTags()
        {
            _song.ClearBadTags();
        }
    }

    public class Song
    {
        public int AudioBitrate => _file.Properties.AudioBitrate;

        public uint Number
        {
            get => _file.Tag.Track;
            set => _file.Tag.Track = value;
        }

        public string Name { get; set; }

        public string Title
        {
            get => _file.Tag.Title;
            set => _file.Tag.Title = value;
        }

        public string Artist
        {
            get { return _file.Tag.FirstPerformer ?? " "; }
            set
            {
                if (_file.Tag.FirstPerformer == null)
                {
                    _file.Tag.Performers = new[] {" "};
                }
                _file.Tag.Performers = new[] { value};
            }
        }

        public string Album
        {
            get { return _file.Tag.Album ?? " "; }
            set { _file.Tag.Album = value; }
        }

        public string Genre
        {
            get { return _file.Tag.Genres.Length > 0 ? _file.Tag.Genres[0] : ""; }
            set { _file.Tag.Genres = new[] {value}; }
        }

        public uint Year
        {
            get { return _file.Tag.Year; }
            set { _file.Tag.Year = value; }
        }

        public TimeSpan Duration
        {
            get { return _file.Properties.Duration; }
        }

        public string Lyric
        {
            get { return _file.Tag.Lyrics; }
            set { _file.Tag.Lyrics = value; }
        }

        public string Path
        {
            get { return _file.Name; }
        }

        public BitmapSource Icon
        {
            get
            {
                var icon = MusicLibrary.GetFileIcon(Path, IconReader.IconSize.Small, false);
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
        }

        private readonly TagLib.File _file;

        public Song(TagLib.File file)
        {
            _file = file;
            Name = System.IO.Path.GetFileName(Path);
        }

        public bool Save()
        {
            try
            {
                File.SetAttributes(_file.Name, FileAttributes.Normal);
                _file.Tag.Title = Title;
                _file.Tag.Genres = new[] { Genre };
                _file.Tag.Album = Album;
                _file.Tag.Performers[0] = Artist;
                _file.Tag.Track = Number;
                _file.Tag.Year = Year;
                _file.Save();
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private static string[] musicExt = new[] {".MP3", ".OGG", ".ACC", ".M4A", ".WMA", ".WMV"};

        public static bool IsFileSong(string path)
        {
            var extension = System.IO.Path.GetExtension(path);
            return extension != null && -1 != Array.IndexOf(musicExt, extension.ToUpperInvariant());
        }

        public bool Equals(SongViewModel obj)
        {
            return (Title == obj.Title) && (AudioBitrate == obj.AudioBitrate) && (Artist == obj.Artist) &&
                   (Album == obj.Album) && (Number == obj.Number) && (Year == obj.Year);
        }

        //public static string NoCoverImage = @"pack://siteoforigin:,,,/nocoverart.jpg";
        public static Uri NoCoverImage = new Uri("nocoverart.jpg", UriKind.Relative);

        public static Uri GetCoverPath(string path)
        {
            string dirName = System.IO.Path.GetDirectoryName(path);
            if (dirName != null)
            {
                string coverPath = System.IO.Path.Combine(dirName, "cover.jpg");
                return File.Exists(coverPath) ? new Uri(coverPath) : NoCoverImage;
            }
            return NoCoverImage;
        }

        internal void ClearBadTags()
        {
            _file.Tag.Comment = "";
            _file.Tag.AlbumArtists = null;
            _file.Tag.Pictures = null;
        }
    }
}