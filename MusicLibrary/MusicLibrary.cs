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

        public void AddSong(Song _song)
        {
            var song = new SongViewModel(_song);
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

        public int Number
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

        public int Year
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
            get { return _song.GetIcon(); }
        }

        private Song _song;

        public SongViewModel(Song song)
        {
            _song = song;
        }

        public bool Save()
        {
            return _song.SaveSong(_song.Path);
        }
    }

    public class Song
    {
        public int Id { get; set; }

        public int AudioBitrate { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public string Genre { get; set; }

        public int Year { get; set; }

        public TimeSpan Duration { get; set; }

        public string Lyric { get; set; }

        public string Path { get; set; }

        public Song()
        {
            Name = System.IO.Path.GetFileName(Path);
        }

        public bool Equals(Song obj)
        {
            return (Title == obj.Title) && (AudioBitrate == obj.AudioBitrate) && (Artist == obj.Artist) &&
                   (Album == obj.Album) && (Number == obj.Number) && (Year == obj.Year);
        }

        public void CopyTo(Song dest)
        {
            dest.Album = Album;
            dest.Artist = Artist;
            dest.AudioBitrate = AudioBitrate;
            dest.Title = Title;
            dest.Year = Year;
            dest.Number = Number;
            dest.Genre = Genre;
            dest.Lyric = Lyric;
            dest.Duration = Duration;
            dest.Name = Name;
        }
    }


    public static class SongExtension
    {
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

        private static string[] musicExt = new[] { ".MP3", ".OGG", ".ACC", ".M4A", ".WMA", ".WMV" };

        public static bool IsFileSong(string path)
        {
            var extension = System.IO.Path.GetExtension(path);
            return extension != null && -1 != Array.IndexOf(musicExt, extension.ToUpperInvariant());
        }

        public static bool SaveSong(this Song songitem,string path)
        {
            try
            {
                var tagLibFile = TagLib.File.Create(path);
                File.SetAttributes(tagLibFile.Name, FileAttributes.Normal);
                tagLibFile.Tag.Title = songitem.Title;
                tagLibFile.Tag.Genres = new[] { songitem.Genre };
                tagLibFile.Tag.Album = songitem.Album;
                if (tagLibFile.Tag.FirstPerformer == null)
                {
                    tagLibFile.Tag.Performers = new[] { " " };
                }
                tagLibFile.Tag.Performers = new[] { songitem.Artist };
                tagLibFile.Tag.Track = (uint)songitem.Number;
                tagLibFile.Tag.Year = (uint)songitem.Year;
                tagLibFile.Save();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static BitmapSource GetIcon(this Song song)
        {
            var icon = MusicLibrary.GetFileIcon(song.Path, IconReader.IconSize.Small, false);
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        public static void LoadTags(this Song songitem, string path)
        {
            var tagLibFile = TagLib.File.Create(path);
            songitem.Album = tagLibFile.Tag.Album;
            songitem.Path = path;
            songitem.Artist = tagLibFile.Tag.Performers.Length>0? tagLibFile.Tag.Performers[0]:null;
            songitem.AudioBitrate = tagLibFile.Properties.AudioBitrate;
            songitem.Duration = tagLibFile.Properties.Duration;
            songitem.Genre = tagLibFile.Tag.Genres.Length > 0 ? tagLibFile.Tag.Genres[0] : null;
            songitem.Lyric = tagLibFile.Tag.Lyrics;
            songitem.Year = (int)tagLibFile.Tag.Year;
            songitem.Title = tagLibFile.Tag.Title;
            songitem.Number = (int)tagLibFile.Tag.Track;
            songitem.Name = Path.GetFileName(path);
        }
    }
}