using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using NewNameMP3Files.MVVM.Annotations;

namespace NewNameMP3Files.MVVM.Model
{
    class MusicLibrary
    {
    }

    public class Author
    {
        public bool IsSelected { get; set; }
        public string AuthorName { get; set; }

        public ObservableCollection<Album> AlbumCollection { get; set; }

        public Author(string authorName)
        {
            AuthorName = authorName;
            AlbumCollection = new ObservableCollection<Album>();
        }

        public void AddAlbum(Album album)
        {
            AlbumCollection.Add(album);
        }
    }

    public class Album
    {
        public bool IsSelected { get; set; }
        public string AlbumName { get; set; }

        public ObservableCollection<Song> SongsCollection { get; set; }
        public Album(string albumName)
        {
            AlbumName = albumName;
            SongsCollection = new ObservableCollection<Song>();
        }

        public void AddSong(Song song)
        {
            SongsCollection.Add(song);
        }

        internal void AddSong(TagLib.File file)
        {
            var song = new Song(file);
            SongsCollection.Add(song);
        }
    }

    public class Song:ViewModelBase
    {
        public int AudioBitrate { get; set; }
        public uint Number { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public uint Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string Lyric { get; set; }
        public string Path { get; set; }

        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; RaisePropertyChanged(()=>IsSelected); }
            
        }
        public bool IsCurrentSong { get; set; }

        public bool Equals(Song obj)
        {
            return (Name == obj.Name) && (AudioBitrate == obj.AudioBitrate) && (IsCurrentSong == obj.IsCurrentSong)
                && (Artist == obj.Artist) && (Album == obj.Album) && (Number == obj.Number);
        }

        public Song(TagLib.File file)
        {
            Title = file.Tag.Title;
            Name = file.Tag.Track.ToString("D2") + " - " + file.Tag.Title;
            Genre = file.Tag.FirstGenre;
            Album = file.Tag.Album;
            Artist = file.Tag.FirstPerformer;
            Number = file.Tag.Track;
            Year = file.Tag.Year;
            Duration = file.Properties.Duration;
            AudioBitrate = file.Properties.AudioBitrate;
            Path = file.Name;
        }
    }
}
