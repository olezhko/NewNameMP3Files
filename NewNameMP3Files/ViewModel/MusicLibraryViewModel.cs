using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using MusicLibrary;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using NewNameMP3Files.Model;
using NewNameMP3Files.Properties;

namespace NewNameMP3Files.ViewModel
{
    public class MusicLibraryViewModel : ViewModelBase
    {
        public MusicLibraryViewModel()
        {
            MusicLibraryList = new ObservableCollection<Author>();

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
            LoadLibrary(Settings.Default.MusicLibraryPath);
        }

        internal void LoadLibrary(string librarypath)
        {
            // 1 - load from db
            using (var db = new MusicLibraryContext())
            {
                Console.WriteLine("Count songs in databse = {0}", db.Songs.Count());
                var songs = db.Songs.ToList();
                var tempMusicList = new ObservableCollection<Author>();
                foreach (var song in songs)
                {
                    AddSongToList(tempMusicList, song);
                }
                MusicLibraryList = tempMusicList;
            }
            // 2 - check library for modifications
            UpdateDb(librarypath);
        }

        private void UpdateDb(string sDir) 
        {
            Task.Run(() =>
            {
                var resSongs = UpdateTask(sDir);
                using (var db = new MusicLibraryContext())
                {
                    CheckSongsWithBase(resSongs, db);
                    db.SaveChanges();
                    var songs = db.Songs.ToList();
                    var tempMusicList = new ObservableCollection<Author>();
                    foreach (var song in songs)
                    {
                        AddSongToList(tempMusicList, song);
                    }
                    MusicLibraryList = tempMusicList;
                }
            });
        }

        private void CheckSongsWithBase(ObservableCollection<Song> resSongs, MusicLibraryContext db)
        {
            var dbSongs = db.Songs.ToList();// get songs from base
            for (int i = 0; i < resSongs.Count; i++)
            {
                var song = resSongs[i];
                var path = song.Path;
                var selSongs = dbSongs.FirstOrDefault(a => a.Path == path);
                if (selSongs!=null)
                {
                    if (!song.Equals(selSongs))
                    {
                        song.CopyTo(selSongs);
                        db.SaveChanges();
                    }
                    dbSongs.Remove(selSongs); // delete song because checking with music lib folder
                }
                else
                {
                    db.AddSong(song);
                    Console.WriteLine("Add Song to db {0}", song.Path);
                }
            }

            if (dbSongs.Count>0)
            {
                db.RemoveSongs(dbSongs);
            }
        }

        internal void ClearMusicLibrary()
        {
            using (var db = new MusicLibraryContext())
            {
                db.ClearAll();
                MusicLibraryList.Clear();
            }
        }

        private ObservableCollection<Song> UpdateTask(string sDir)
        {
            int fCount = Directory.GetFiles(sDir, "*", SearchOption.AllDirectories).Length;
            Console.WriteLine("Count of files in {0}",fCount);
            var songs = new ObservableCollection<Song>();
            Console.WriteLine("Finding folder {0}", sDir);

            var dirs = Directory.GetDirectories(sDir);
            var files = Directory.GetFiles(sDir);

            foreach (string f in files)
            {
                if (SongExtension.IsFileSong(f))
                {
                    var song = new Song();
                    song.LoadTags(f);
                    songs.Add(song);
                }
            }

            foreach (string d in dirs)
            {
                var res = UpdateTask(d);
                foreach (var song in res)
                {
                    songs.Add(song);
                }
            }

            Console.WriteLine("Find songs in library folder count {0}", songs.Count);
            return songs;
        }

        private void AddSongToList(ObservableCollection<Author> collection,Song songItem)
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
        }


        public ObservableCollection<Author> MusicLibraryList { get; set; }

        public RelayCommand<MouseEventArgs> MouseRightButtonUpCommand
        {
            get;
            private set;
        }

        private string _updateLibraryPercent;
        public string UpdateLibraryPercent
        {
            get { return _updateLibraryPercent; }
            set
            {
                _updateLibraryPercent = value;
                RaisePropertyChanged(()=>UpdateLibraryPercent);
            }
        }

    }
}
