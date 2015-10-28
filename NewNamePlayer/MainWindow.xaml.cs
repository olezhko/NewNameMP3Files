using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using NewNamePlayer.Code;

namespace NewNamePlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // запуск песни по DOuble click
        // сохранять последовательность колонок
        // сделать изменение тэгов треков по правой клавише мыши
        // сделать функцию очереди треков по сочетанию клавиш ctrl + Q
        private readonly Settings _settings;
        private readonly Playlist _mainPlaylist;
        Audio _currentPlayingFile;
        public MainWindow()
        {
            InitializeComponent();
            
            _settings = new Settings();
            _mainPlaylist = new Playlist();
            PlayListView.ItemsSource = _mainPlaylist.Songs;
            _mainPlaylist.SongsListEventChanged += _mainPlaylist_SongsListEventChanged;
            _mainPlaylist.CurrentSongEventChanged += _mainPlaylist_CurrentSongEventChanged;
        }

        void _mainPlaylist_CurrentSongEventChanged(object sender, EventArgs e)
        {
            _mainPlaylist.CurrentSong.IsCurrentSong = true;
        }

        void _mainPlaylist_SongsListEventChanged(object sender, EventArgs e)
        {
            UpdateStatusStrip();
        }

        private void ApplySettings()
        {
            NumberColumn.Width = _settings.NumberColumnSize;
            NameColumn.Width = _settings.NameColumnSize;
            ArtistColumn.Width = _settings.ArtistColumnSize;
            AlbumColumn.Width = _settings.AlbumColumnSize;
            PathColumn.Width = _settings.PathColumnSize;
            GenreColumn.Width = _settings.GenreColumnSize;
            YearColumn.Width = _settings.YearColumnSize;
            DurationColumn.Width = _settings.DurationColumnSize;
            BitrateColumn.Width = _settings.BitrateColumnSize;
            if (_settings.IsMaximaze)
            {
                WindowState = WindowState.Maximized;
            }
            //_mainPlaylist.CurrentSong = _mainPlaylist.Songs[_settings.LastSongIndex];
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _settings.NumberColumnSize = NumberColumn.ActualWidth;
            _settings.NameColumnSize = NameColumn.ActualWidth;
            _settings.ArtistColumnSize = ArtistColumn.ActualWidth;
            _settings.AlbumColumnSize = AlbumColumn.ActualWidth;
            _settings.PathColumnSize = PathColumn.ActualWidth;
            _settings.GenreColumnSize = GenreColumn.ActualWidth;
            _settings.BitrateColumnSize = BitrateColumn.ActualWidth;
            _settings.YearColumnSize = YearColumn.ActualWidth;
            _settings.DurationColumnSize = DurationColumn.ActualWidth;
            _settings.IsMaximaze = WindowState == WindowState.Maximized;
            _settings.SaveSettings();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ApplySettings();
        }

        private static IEnumerable<Song> AddFiles(string path)
        {
            List<Song> songs = new List<Song>();
            try
            {
                if (Directory.Exists(path)) // is Directory
                {
                    List<string> newpathList = new List<string>();
                    newpathList.AddRange(Directory.GetDirectories(path));
                    newpathList.AddRange(Directory.GetFiles(path));
                    foreach (var newpath in newpathList)
                    {
                        songs.AddRange(AddFiles(newpath));
                    }
                }
                else
                {
                    if (File.Exists(path) && IsMediaFile(path))
                    {
                        var fileWithTags = TagLib.File.Create(path);
                        var newSong = new Song
                        {
                            Album = fileWithTags.Tag.Album,
                            Artist = fileWithTags.Tag.FirstPerformer,
                            Genre = fileWithTags.Tag.FirstGenre,
                            Name = fileWithTags.Tag.Title,
                            Number = fileWithTags.Tag.Track,
                            Year = fileWithTags.Tag.Year,
                            Path = path,
                            Duration = fileWithTags.Properties.Duration,
                            AudioBitrate = fileWithTags.Properties.AudioBitrate
                        };

                        songs.Add(newSong);
                    }
                } 
                return songs;
            }
            catch
            {
               return new List<Song>();
            }
        }

        private static bool IsMediaFile(string path)
        {
            string[] mediaExtensions = {".WAV", ".WMA", ".MP3", ".OGG", ".WMV"};
            var extension = Path.GetExtension(path);
            return extension != null && -1 != Array.IndexOf(mediaExtensions, extension.ToUpperInvariant());
        }

        private void PlayListView_Drop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var elements = (string[])e.Data.GetData(DataFormats.FileDrop);
                List<Song> allSongsList = new List<Song>();
                foreach (string t in elements)
                {
                    allSongsList.AddRange(AddFiles(t));
                }

                ObservableCollection<Song> list = new ObservableCollection<Song>(allSongsList);
                _mainPlaylist.AddSongs(list);
                _mainPlaylist.CurrentSong = _mainPlaylist.Songs[0];
            }
        }
       
        private void PrevSongButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mainPlaylist.Songs.Count == 0)
            {
                return;
            }


            int currentIndex = _mainPlaylist.Songs.IndexOf(_mainPlaylist.CurrentSong);
            if (currentIndex != 0)
            {
                _mainPlaylist.CurrentSong = _mainPlaylist.Songs[currentIndex + 1];
            }
            if (_mainPlaylist.Songs.Count - 1 == currentIndex)
            {
                _mainPlaylist.CurrentSong = _mainPlaylist.Songs[0];
            }

            _mainPlaylist.Stop();
            _mainPlaylist.Play();
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mainPlaylist.CurrentSong == null)
            {
                _mainPlaylist.CurrentSong = _mainPlaylist.Songs.First();
            }

            if (_currentPlayingFile == null)
            {
                _currentPlayingFile = new Audio(_mainPlaylist.CurrentSong.Path);
                _currentPlayingFile.PlaybackStop += _currentPlayingFile_PlaybackStop;
            }

            var mp3File = TagLib.File.Create(_mainPlaylist.CurrentSong.Path);
            switch (_currentPlayingFile.State)
            {
                case Audio.PlayBackState.Paused:
                    PlayPauseButton.Content = Properties.Resources.pause;
                    _currentPlayingFile.Play();
                    Title = "Player     " + mp3File.Tag.FirstPerformer + " - " + mp3File.Tag.Title;
                    SongPositionSlider.Maximum = mp3File.Properties.Duration.TotalSeconds;
                    break;
                case Audio.PlayBackState.Playing:
                    PlayPauseButton.Content = Properties.Resources.play;
                    _currentPlayingFile.Pause();
                    break;
                case Audio.PlayBackState.Stopped:
                    PlayPauseButton.Content = Properties.Resources.pause;
                    Title = "Player     " + mp3File.Tag.FirstPerformer + " - " + mp3File.Tag.Title;
                    _currentPlayingFile.Play();
                    SongPositionSlider.Maximum = mp3File.Properties.Duration.TotalSeconds;
                    break;
            }
        }

        void _currentPlayingFile_PlaybackStop(EventArgs e)
        {
            
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if(_currentPlayingFile!=null)
                _currentPlayingFile.Stop();
        }

        private void NextSongButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void LastFmLoveButton_Click(object sender, RoutedEventArgs e)
        {
            LastFMModule.LoveSong(_mainPlaylist.CurrentSong);
        }

        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((int)VolumeSlider.Value)
            {
                case 100:
                {
                    VolumeSlider.Value = 66;
                    VolumeButton.Content = Properties.Resources.volume_up66;
                }
                    break;
                case 66:
                {
                    VolumeSlider.Value = 33;
                    VolumeButton.Content = Properties.Resources.volume_up33;
                }
                    break;
                case 33:
                {
                    VolumeSlider.Value = 0;
                    VolumeButton.Content = Properties.Resources.volume_down;
                }
                    break;
                case 0:
                {
                    VolumeSlider.Value = 100;
                    VolumeButton.Content = Properties.Resources.volume_up100;
                }
                    break;
                default:
                {
                    VolumeSlider.Value = 100;
                    VolumeButton.Content = Properties.Resources.volume_up100;
                }
                    break;
            }
            VolumeValueLabel.Content = VolumeSlider.Value + "%";
        }

        private void UpdateStatusStrip()
        {
            TimeSpan totalduration = new TimeSpan();
            totalduration = _mainPlaylist.Songs.Aggregate(totalduration, (current, t) => current + t.Duration);

            SongsInPlayListLabel.Content = _mainPlaylist.Songs.Count + " songs";
            TotalDurationSongsInPlayListLabel.Content = "[" + totalduration + "]";
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
            {
                return;
            }
            var a = (float)(VolumeSlider.Value / 100.0);
            VolumeValueLabel.Content = VolumeSlider.Value + "%";
            if (_currentPlayingFile != null)
            {
                _currentPlayingFile.Volume = a;
            }

            if (VolumeSlider.Value >= 66)
            {
                VolumeButton.Content = Properties.Resources.volume_up100;
            }
            if (VolumeSlider.Value >= 33 && VolumeSlider.Value < 66)
            {
                VolumeButton.Content = Properties.Resources.volume_up66;
            }
            if (VolumeSlider.Value > 0 && VolumeSlider.Value < 33)
            {
                VolumeButton.Content = Properties.Resources.volume_up33;
            }
            if (VolumeSlider.Value.Equals(0.0))
            {
                VolumeButton.Content = Properties.Resources.volume_down;
            }
        }

        private void songPositionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_currentPlayingFile != null)
            {
                _currentPlayingFile.TimePosition = TimeSpan.FromSeconds(SongPositionSlider.Value);
            }
        }

        private void PlayListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var index = PlayListView.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            _mainPlaylist.Stop();
            _mainPlaylist.CurrentSong = _mainPlaylist.Songs[index];
            _mainPlaylist.Play();
            
        }

        private void PlayListView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var index = PlayListView.SelectedIndex;
                if (index == -1)
                {
                    return;
                }

                _mainPlaylist.Songs.RemoveAt(index);
            }
        }
    }
}
