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
                flags += Shell32.SHGFI_LARGEICON;  // include the large icon flag
            }

            Shell32.SHGetFileInfo(name,
                Shell32.FILE_ATTRIBUTE_NORMAL,
                ref shfi,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
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
            set { _isSelected = value; RaisePropertyChanged(() => IsSelected); }
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
            album.PropertyChanged += album_PropertyChanged;
            AlbumCollection.Add(album);
        }

        void album_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
            set { _isSelected = value; RaisePropertyChanged(() => IsSelected); }
        }

        public string AlbumName { get; set; }

        public ObservableCollection<Song> SongsCollection { get; set; }
        public Uri AlbumCover { get; set; }

        public Album(string albumName)
        {
            AlbumName = albumName;
            SongsCollection = new ObservableCollection<Song>();
        }

        public Album(string albumName, string albumCover)
        {
            AlbumCover = new Uri(albumCover);
            AlbumName = albumName;
            SongsCollection = new ObservableCollection<Song>();
        }

        public Album(string albumName, Uri albumCover)
        {
            AlbumCover = albumCover;
            AlbumName = albumName;
            SongsCollection = new ObservableCollection<Song>();
        }

        public void AddSong(Song song)
        {
            song.PropertyChanged += song_PropertyChanged;
            SongsCollection.Add(song);
        }

        public void AddSong(TagLib.File file)
        {
            var song = new Song(file);
            song.PropertyChanged += song_PropertyChanged;
            SongsCollection.Add(song);
        }

        void song_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsSelected = SongsCollection.All(song => song.IsSelected);
        }

        public void RemoveSong(TagLib.File file)
        {

        }
    }

    public class Song : ViewModelBase
    {
        public int AudioBitrate
        {
            get { return _file.Properties.AudioBitrate; }
        }

        public uint Number
        {
            get
            {
                return _file.Tag.Track;
            }
            set
            {
                _file.Tag.Track = value;
                RaisePropertyChanged(() => Number);
            }
        }
        public string Name { get; set; }
        public string Title
        {
            get
            {
                return _file.Tag.Title;
            }
            set
            {
                _file.Tag.Title = value;
                RaisePropertyChanged(() => Title);
            }
        }
        public string Artist
        {
            get
            {
                return _file.Tag.Performers[0];
            }
            set
            {
                _file.Tag.Performers[0] = value;
                RaisePropertyChanged(() => Artist);
            }
        }
        public string Album
        {
            get
            {
                return _file.Tag.Album;
            }
            set
            {
                _file.Tag.Album = value;
                RaisePropertyChanged(() => Album);
            }
        }
        public string Genre
        {
            get
            {
                return _file.Tag.Genres.Length > 0 ? _file.Tag.Genres[0] : "";
            }
            set
            {
                _file.Tag.Genres[0] = value;
                RaisePropertyChanged(() => Genre);
            }
        }
        public uint Year
        {
            get
            {
                return _file.Tag.Year;
            }
            set
            {
                _file.Tag.Year = value;
                RaisePropertyChanged(() => Year);
            }
        }

        public TimeSpan Duration
        {
            get { return _file.Properties.Duration; }
        }

        public string Lyric
        {
            get
            {
                return _file.Tag.Lyrics;
            }
            set
            {
                _file.Tag.Lyrics = value;
                RaisePropertyChanged(() => Lyric);
            }
        }
        public string Path 
        {
            get { return _file.Name; } 
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged(() => IsSelected); }
        }

        public BitmapSource Icon
        {
            get
            {
                var icon = MusicLibrary.GetFileIcon(Path, IconReader.IconSize.Small, false); 
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle,System.Windows.Int32Rect.Empty,BitmapSizeOptions.FromEmptyOptions());
            }
        }
 
        private readonly TagLib.File _file;
        public Song(TagLib.File file)
        {
            _file = file;
            Name = System.IO.Path.GetFileName(Path);
        }

        public void Save()
        {
            _file.Tag.Title = Title;
            _file.Tag.Genres[0] = Genre;
            _file.Tag.Album = Album;
            _file.Tag.Performers[0] = Artist;
            _file.Tag.Track = Number;
            _file.Tag.Year = Year;
            _file.Save();
        }

        private static string[] musicExt = new[] { ".MP3", ".OGG", ".ACC", ".M4A", ".WMA", ".WMV" };
        public static bool IsFileSong(string path)
        {
            var extension = System.IO.Path.GetExtension(path);
            return extension != null && -1 != Array.IndexOf(musicExt, extension.ToUpperInvariant());
        }

        public bool Equals(Song obj)
        {
            return (Title == obj.Title) && (AudioBitrate == obj.AudioBitrate) && (Artist == obj.Artist) && (Album == obj.Album) && (Number == obj.Number) && (Year == obj.Year);
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
    }
}
