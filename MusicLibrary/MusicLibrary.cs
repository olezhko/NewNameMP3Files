﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace MusicLibrary
{
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
        public Album(string albumName)
        {
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

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged(() => IsSelected); }
        }
        public bool IsCurrentSong { get; set; }

        private TagLib.File _file;
        public bool Equals(Song obj)
        {
            return (Title == obj.Title) && (AudioBitrate == obj.AudioBitrate) && (Artist == obj.Artist) && (Album == obj.Album) && (Number == obj.Number) && (Year == obj.Year);
        }

        public Song(TagLib.File file)
        {
            _file = file;
            Title = file.Tag.Title;
            Genre = file.Tag.Genres.Length>0?file.Tag.Genres[0]:"";
            Album = file.Tag.Album;
            Artist = file.Tag.Performers[0];
            Number = file.Tag.Track;
            Year = file.Tag.Year;
            Duration = file.Properties.Duration;
            AudioBitrate = file.Properties.AudioBitrate;
            Path = file.Name;
            Name = System.IO.Path.GetFileName(Path);
        }

        public Song()
        {
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

        private static string[] musicExt = new[] { ".mp3", "ogg","acc" };

        public static bool IsFileSong(string path)
        {
            string ext = System.IO.Path.GetExtension(path);
            return ext == musicExt[0] || ext == musicExt[1] || ext == musicExt[2];
        }
    }
}
