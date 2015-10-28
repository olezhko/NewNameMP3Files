using System;
using System.Collections.ObjectModel;

namespace NewNamePlayer
{
    public class Song
    {
        public int AudioBitrate { get; set; }
        public uint Number { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public uint Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string Lyric { get; set; }
        public string Path { get; set; }

        public bool IsCurrentSong { get; set; }
    }

    public class Playlist
    {
        public ObservableCollection<Song> Songs;

        public event EventHandler CurrentSongEventChanged;
        public event EventHandler SongsListEventChanged;
        private int _currentSongIndex;
        public Song CurrentSong
        {
            get
            {
                return Songs[_currentSongIndex];
            }
            set
            {
                if (CurrentSong!=null)
                {
                    CurrentSong.IsCurrentSong = false;
                }
                
                _currentSongIndex = Songs.IndexOf(value);
                if(CurrentSongEventChanged!=null)
                    CurrentSongEventChanged(this, EventArgs.Empty);
            }
        }

        public Playlist()
        {
            Songs = new ObservableCollection<Song>();
        }

        public void AddSongs(ObservableCollection<Song> allSongsList)
        {
            if (Songs == null)
            {
                Songs = new ObservableCollection<Song>();
            }
            foreach (var song in allSongsList)
            {
                Songs.Add(song);
            }
            if (SongsListEventChanged!=null)
            {
                SongsListEventChanged(this, EventArgs.Empty);
            }
        }

        internal void Play()
        {
            
        }

        internal void Stop()
        {
            
        }
    }
}
