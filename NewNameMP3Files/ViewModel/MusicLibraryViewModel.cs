using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using MusicLibrary;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Threading;
using NewNameMP3Files.Model;
using NewNameMP3Files.Properties;

namespace NewNameMP3Files.ViewModel
{
    public class MusicLibraryViewModel : ViewModelBase
    {
        public Dispatcher dispatcher;
        public MusicLibraryViewModel()
        {
            MusicLibraryList = new ObservableCollection<Author>();
            SynchroMusicLibraryCommand = new RelayCommand(SynchroMusicLibraryMethod);
            ClearMusicLibraryCommand = new RelayCommand(ClearMusicLibrary);
            ReInitMusicLibraryCommand = new RelayCommand(ReInitMusicLibraryMethod);
            MouseRightButtonUpCommand = new RelayCommand<MouseEventArgs>(e =>
            {
                var element = e.Source;
                var parent = VisualTreeHelper.GetParent((DependencyObject)element) as UIElement;
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
                parent = VisualTreeHelper.GetParent(parent) as UIElement;

                var treeview = parent as TreeViewItem;
                if (treeview != null)
                {
                    treeview.IsExpanded = !treeview.IsExpanded;
                }
            });
        }

        /// <summary>
        /// Переинициализация, очистка + загрузка заново из базы
        /// </summary>
        private void ReInitMusicLibraryMethod()
        {
            ClearMusicLibrary();
            LoadLibrary(Settings.Default.MusicLibraryPath);
        }
        /// <summary>
        /// Очистка базы и очистка ItemSource контрола
        /// </summary>
        private void ClearMusicLibrary()
        {
            using (var db = new MusicLibraryContext())
            {
                db.ClearAllSongs();
                MusicLibraryList.Clear();
            }
        }
        /// <summary>
        /// загрузка из базы данных + проверка на модификации
        /// </summary>
        /// <param name="librarypath">Путь к папке с музыкой</param>
        internal void LoadLibrary(string librarypath)
        {
            if (String.IsNullOrEmpty(librarypath) || !Directory.Exists(librarypath))
            {
                return;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (var db = new MusicLibraryContext())            // 1 - load from db
            {
                var songs = db.Songs.ToList();
                var tempMusicList = new ObservableCollection<Author>();
                foreach (var song in songs)
                {
                    AddSongToList(tempMusicList, song);
                }
                MusicLibraryList = tempMusicList;
                RaisePropertyChanged(()=>MusicLibraryList);
            }
            // 2 - check library for modifications
            if (Settings.Default.CheckMusicLibraryOnStartProgram)
            {
                SynchroLibrary(librarypath);
            }
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
        /// <summary>
        /// 
        /// </summary>
        private void SynchroMusicLibraryMethod()
        {
            SynchroLibrary(Settings.Default.MusicLibraryPath);
        }
        /// <summary>
        /// Добавление песни к коллекции на GUI
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="songItem"></param>
        private void AddSongToList(ObservableCollection<Author> collection, Song songItem)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                var albumName = songItem.Year + " - " + songItem.Album;
                var perfomer = songItem.Artist;

                int res = -1;
                foreach (Author author in collection)
                {
                    var arthist = author;
                    if (arthist.AuthorName.Equals(perfomer))
                    {
                        res = 1;
                        var resAlbums = -1;
                        for (int albumIndex = 0; albumIndex < arthist.AlbumCollection.Count; albumIndex++)
                        {
                            if (author.AlbumCollection[albumIndex].AlbumName.Equals(albumName))
                            {
                                resAlbums = 1;
                                author.AlbumCollection[albumIndex].AddSong(songItem);
                            }
                        }

                        if (resAlbums == -1)
                        {
                            string albumCoverPath = Path.Combine(Path.GetDirectoryName(songItem.Path), "cover.jpg");
                            author.AddAlbum(File.Exists(albumCoverPath)
                                ? new Album(albumName, albumCoverPath, author.AuthorName)
                                : new Album(albumName, new Uri("/Skins/nocoverart.jpg", UriKind.Relative),
                                    author.AuthorName));

                            author.AlbumCollection.Last().AddSong(songItem);
                        }
                    }
                }

                if (res == -1)
                {
                    collection.Add(new Author(perfomer));

                    string albumCoverPath = Path.Combine(Path.GetDirectoryName(songItem.Path), "cover.jpg");
                    collection.Last()
                        .AddAlbum(File.Exists(albumCoverPath)
                            ? new Album(albumName, albumCoverPath, collection.Last().AuthorName)
                            : new Album(albumName, new Uri("/Skins/nocoverart.jpg", UriKind.Relative),
                                collection.Last().AuthorName));

                    collection.Last().AlbumCollection.Last().AddSong(songItem);
                }
            }));        
        }


        private async void SynchroLibrary(string sDir)
        {
            var res = await StartTaskGetLibraryFolderSongs(sDir);
            using (var db = new MusicLibraryContext())
            {
                SynchroLibraryWithSongs(res,db.Songs.ToList());
            }

        }

        private void SynchroLibraryWithSongs(ObservableCollection<string> songs, List<Song> dbSongs)
        {
            Task.Run(() =>
            {
                foreach (var song in songs)
                {
                    CheckSong(song,dbSongs);
                }
            });
        }

        private void CheckSong(string songPath,List<Song> dbSongs)
        {
            if (!SongExtension.IsFileSong(songPath))
            {
                return;
            }
            Song song = new Song();
            song.LoadTags(songPath);

            if (dbSongs.Count == 0)
            {
                AddSongToList(MusicLibraryList, song);
                //db.AddSong(song);
                Console.WriteLine("Add Song to db {0}", song.Path);
            }
            else
            {
                var selSongs = dbSongs.First(a => a.Path == songPath);
                if (selSongs != null) // если такая песня есть в базе
                {
                    if (!song.Equals(selSongs)) // сравниваем теги
                    {
                        song.CopyTo(selSongs);// если разные, то обновляем базу данныъ
                        //db.SaveChanges();
                    }
                    dbSongs.Remove(selSongs);
                }
                else
                {
                    AddSongToList(MusicLibraryList, song);
                    //db.AddSong(song);
                    Console.WriteLine("Add Song to db {0}", song.Path);
                }
            }
        }

        private Task<ObservableCollection<string>> StartTaskGetLibraryFolderSongs(string sDir)
        {
            return Task.Run(() =>
            {
                return GetLibraryFolderSongs(sDir);
            });
        }

        private ObservableCollection<string> GetLibraryFolderSongs(string sDir)
        {
            var collection = new ObservableCollection<string>();

            var dirs = Directory.GetDirectories(sDir);
            var realFolderSongs = Directory.GetFiles(sDir).ToList();

            foreach (var realFolderSong in realFolderSongs)
            {
                collection.Add(realFolderSong);
            }

            foreach (string d in dirs)
            {
                var dirCollection = GetLibraryFolderSongs(d);
                foreach (var songpath in dirCollection)
                {
                    collection.Add(songpath);
                }
            }

            return collection;
        }

        public void Save()
        {
            
        }
        #region Properties
        
        public ObservableCollection<Author> MusicLibraryList { get; set; }
        public RelayCommand<MouseEventArgs> MouseRightButtonUpCommand
        {
            get;
            private set;
        }
        public RelayCommand ClearMusicLibraryCommand { get; private set; }
        public RelayCommand ReInitMusicLibraryCommand { get; private set; }
        public RelayCommand SynchroMusicLibraryCommand { get; private set; }
        private string _updateLibraryPercent;
        public string UpdateLibraryPercent
        {
            get { return _updateLibraryPercent; }
            set
            {
                _updateLibraryPercent = value;
                RaisePropertyChanged(() => UpdateLibraryPercent);
            }
        }


        #endregion
    }
}
