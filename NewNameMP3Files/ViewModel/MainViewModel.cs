using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MusicLibrary;
using NewNameMP3Files.Model;
using NewNameMP3Files.Skins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NewNameMP3Files.Properties;
using Application = System.Windows.Application;
using CheckBox = System.Windows.Controls.CheckBox;
using DragEventArgs = System.Windows.DragEventArgs;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace NewNameMP3Files.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            _authorCollection = new ObservableCollection<Author>();
            RenameCheckedCommand = new RelayCommand(RenameAction);
            OpenFilesCommand = new RelayCommand(OpenFilesMethod);
            OpenDirectoryCommand = new RelayCommand(OpenDirectoryMethod);
            ExitCommand = new RelayCommand<Window>(ExitMethod);
            AboutCommand = new RelayCommand(AboutMethod);
            DragCommand = new RelayCommand<DragEventArgs>(DragEnterAuthorsListViewMethod);
            OpenTemplateOptionWindow = new RelayCommand(OpenOptionsWindow);
            ChangeLanguageCommand = new RelayCommand<MenuItem>(ChangeLanguageMethod);
            SelectAllCommand = new RelayCommand<bool>(b => SelectAllMethod(true));
            DeSelectAllCommand = new RelayCommand<bool>(b => SelectAllMethod(false));
            EditTagsCommand = new RelayCommand(EditTagsMethod);
            _optionsWindow = new Options();
            _aboutWindow = new AboutWindow();
            _editTagsWindow = new EditTagsWindow();
            _musicLibraryView = new MusicLibraryView();
            ListViewKeyDownCommand = new RelayCommand<KeyEventArgs>(ListViewKeyDownMethod);
            ClickAuthorCommand = new RelayCommand<CheckBox>(AuthorCheckBoxClickMethod);
            ClickAlbumCommand = new RelayCommand<CheckBox>(AlbumCheckBoxClickMethod);
            FindImageMenuCommand = new RelayCommand<Album>(FindCoverMethod);
            MainWindowClosingCommand = new RelayCommand<MusicLibraryViewModel>(MainWindowClosing);
            MainWindowLoadedCommand = new RelayCommand<MusicLibraryViewModel>(MainWindowLoaded);
        }

        private void MainWindowLoaded(MusicLibraryViewModel obj)
        {
            obj.LoadLibrary(Settings.Default.MusicLibraryPath);
        }

        private void MainWindowClosing(MusicLibraryViewModel vm)
        {
            Settings.Default.Save();
            vm.Save();
        }
        /// <summary>
        /// Find cover of album by AlbumName + ArtistName
        /// </summary>
        /// <param name="album"></param>
        private void FindCoverMethod(Album album)
        {
            System.Diagnostics.Process.Start($"https://www.google.com/search?q={album.AlbumName}+{album.AuthorName}&source=lnms&tbm=isch&tbs=isz:l");
        }

        private void ListViewKeyDownMethod(KeyEventArgs args)
        {
            if (args.Key == Key.Delete)
            {
                for (int i = 0; i < AuthorCollection.Count; )
                {
                    for (int j = 0; j < AuthorCollection[i].AlbumCollection.Count;)
                    {
                        for (int k = 0; k < AuthorCollection[i].AlbumCollection[j].SongsCollection.Count; )
                        {
                            if (AuthorCollection[i].AlbumCollection[j].SongsCollection[k].IsSelected)
                            {
                                _renamingFilesList.Remove(AuthorCollection[i].AlbumCollection[j].SongsCollection[k].Path);
                                AuthorCollection[i].AlbumCollection[j].SongsCollection.RemoveAt(k);
                            }
                            else
                            {
                                k++;
                            }
                        }
                        if (AuthorCollection[i].AlbumCollection[j].SongsCollection.Count == 0)
                        {
                            AuthorCollection[i].AlbumCollection.RemoveAt(j);
                        }
                        else
                        {
                            j++;
                        }
                    }
                    if (AuthorCollection[i].AlbumCollection.Count == 0)
                    {
                        AuthorCollection.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        private void AlbumCheckBoxClickMethod(CheckBox item)
        {
            var state = item.IsChecked.Value;
            var tag = item.Tag as ObservableCollection<SongViewModel>;
            if (tag!=null)
            {
                foreach (var song in tag)
                {
                    song.IsSelected = state;
                }
            }
        }

        private void AuthorCheckBoxClickMethod(CheckBox item)
        {
            var state = item.IsChecked.Value;
            var tag = item.Tag as ObservableCollection<Album>;
            if (tag != null)
            {
                foreach (var album in tag)
                {
                    foreach (var song in album.SongsCollection)
                    {
                        song.IsSelected = state;
                    }
                }  
            }
        }

        #region Methods
        private void AboutMethod()
        {
            _aboutWindow.ShowDialog();
        }

        private void ExitMethod(Window wnd)
        {
            if (wnd != null)
            {
                _optionsWindow.Close();
                _aboutWindow.Close();
                _editTagsWindow.Close();
                wnd.Close();
            }
        }

        private void OpenDirectoryMethod()
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Directory.Exists(fbd.SelectedPath))
                {
                    AddDirectoryToList(fbd.SelectedPath);
                }
            }
        }

        private void OpenFilesMethod()
        {
            OpenFileDialog ofd = new OpenFileDialog { Multiselect = true };
            var res = ofd.ShowDialog();
            if (res.HasValue && res.Value)
            {
                var items = ofd.FileNames;
                foreach (var item in items)
                {
                    AddSongToList(item);
                }
            }
        }

        private void EditTagsMethod()
        {
            List<string> files = GetListCheckedFiles();
            if (files == null || !files.Any())
            {
                return;
            }
            var viewModel = (EditTagsViewModel)_editTagsWindow.DataContext;
            viewModel.SongsCollection.Clear();
            foreach (var file in files)
            {
                try
                {
                    var song = new Song();
                    song.LoadTags(file);
                    viewModel.SongsCollection.Add(new SongViewModel(song));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            _editTagsWindow.ShowDialog();
        }

        private void RefreshCollection()
        {
            Console.WriteLine("RefreshCollection");
            Application.Current.Dispatcher.Invoke(delegate
            {
                AuthorCollection.Clear();
                foreach (var item in _renamingFilesList)
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
            }); 
        }

        private void SelectAllMethod(bool state)
        {
            foreach (var song in from author in AuthorCollection from album in author.AlbumCollection from song in album.SongsCollection select song)
            {
                song.IsSelected = state;
            }
        }

        private void ChangeLanguageMethod(MenuItem obj)
        {
            if (obj != null)
            {
                CultureInfo lang = new CultureInfo(obj.Tag.ToString());
                App.Language = lang;
                obj.IsChecked = true;
                if (_checkedLanguageLastMenuItem != null)
                {
                    _checkedLanguageLastMenuItem.IsChecked = false;
                }
                _checkedLanguageLastMenuItem = obj;
            }
        }

        private void OpenOptionsWindow()
        {
            var res = _optionsWindow.ShowDialog();
            var viewModel = (OptionsViewModel)_optionsWindow.DataContext;
            if (viewModel != null)
            {
                Settings.Default.RenameExpression = viewModel.TemplateForFiles;
            }
            Settings.Default.Save();
        }

        private List<string> GetListCheckedFiles()
        {
            return (from author in AuthorCollection from album in author.AlbumCollection from song in album.SongsCollection where song.IsSelected select song.Path).ToList();
        }

        private List<string> GetListAllFiles()
        {
            return (from author in AuthorCollection from album in author.AlbumCollection from song in album.SongsCollection select song.Path).ToList();
        }

        List<string> _renamingFilesList = new List<string>();
        private void RenameAction()
        {
            List<string> files = GetListCheckedFiles();
            if (files == null || !files.Any())
            {
                return;
            }

            _renamingFilesList = GetListAllFiles();
            if (NewFileRenamed == null)
            {
                NewFileRenamed += (send, args) =>
                {
                    var list = send as List<string>;
                    CountRenamedFiles = String.Format("{0}/{1}", args.CountFilesRenamed, list.Count);
                    int percent = args.CountFiles * 100 / list.Count;
                    ProgressRenamedFiles = percent;
                    if (percent == 100)
                    {
                        RefreshCollection();
                        MessageBox.Show(App.ResourceDictionary["DoneString"].ToString() + " " +String.Format("{0}/{1}", args.CountFilesRenamed, list.Count), App.ResourceDictionary["InformationString"].ToString());
                    }
                };
            }

            Console.WriteLine("Start Rename Task " + files.Count);
            Task.Factory.StartNew(() => RenameActionTask(files));
        }

        private void RenameActionTask(List<string> filesPathList)
        {
            int all = 0;
            int count = 0;
            foreach (var file in filesPathList)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                if (!StaticMethods.IsFileLocked(new FileInfo(file)))
                {
                    var res = FileMoveAsync(file, Settings.Default.RenameExpression);
                    if (!String.IsNullOrEmpty(res.Result))
                    {
                        var index = _renamingFilesList.IndexOf(file);
                        if (index != -1)
                        {
                            _renamingFilesList[index] = res.Result;
                        }
                        count++;
                        
                    }
                }
                all++;
                NewFileRenamed?.Invoke(filesPathList, new FileTryingRenamedEventArgs(count,all));
            }
        }

        private static Task<string> FileMoveAsync(string file,string expression)
        {
            var mp3File = TagLib.File.Create(file);
            StaticMethods.TagsToUpper(mp3File);
            var tempName = StaticMethods.GetNewNameByTemplate(expression, mp3File.Tag);
            tempName = StaticMethods.DeleteBannedSymbols(tempName);

            var folder = Path.GetDirectoryName(file);
            var finalPath = String.Format("{0}\\{1}{2}", folder, tempName, Path.GetExtension(file));

            return Task<string>.Run(() =>
            {
                var i = 1;
                while (true)// исключаем создание одинаковых имен
                {
                    try
                    {
                        File.Move(file, finalPath);
                        return finalPath;
                    }
                    catch
                    {
                        finalPath = String.Format("{0}\\{1}({2}){3}", folder, tempName, i, Path.GetExtension(file));
                        i++;
                    }
                }
            });
            
        }

        private void DragEnterAuthorsListViewMethod(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                var dragItems = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
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
            var files = Directory.EnumerateFiles(direcorypath, "*.*", SearchOption.AllDirectories).Where(SongExtension.IsFileSong);

            foreach (var file in files)
            {
                AddSongToList(file);
            }
        }

        private void AddSongToList(string filepath)
        {
            if (!SongExtension.IsFileSong(filepath))
            {
                return;
            }
            Song _song = new Song(filepath);

            var albumName = _song.Year + " - " + _song.Album;
            var perfomer = _song.Artist == null ? " " : _song.Artist;

            int res = -1;
            foreach (Author author in AuthorCollection)
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
                            author.AlbumCollection[albumIndex].AddSong(_song);
                        }
                    }

                    if (resAlbums == -1)
                    {
                        string albumCoverPath = Path.Combine(Path.GetDirectoryName(filepath), "cover.jpg");
                        if (File.Exists(albumCoverPath))
                        {
                            author.AddAlbum(new Album(albumName, albumCoverPath, author.AuthorName));
                        }
                        else
                        {
                            author.AddAlbum(new Album(albumName, new Uri("/Skins/nocoverart.jpg", UriKind.Relative),author.AuthorName));
                        }
                        
                        author.AlbumCollection.Last().AddSong(_song);
                    }
                }
            }

            if (res == -1)
            {
                AuthorCollection.Add(new Author(perfomer));

                string albumCoverPath = Path.Combine(Path.GetDirectoryName(filepath), "cover.jpg");
                if (File.Exists(albumCoverPath))
                {
                    AuthorCollection.Last().AddAlbum(new Album(albumName, albumCoverPath, AuthorCollection.Last().AuthorName));
                }
                else
                {
                    AuthorCollection.Last().AddAlbum(new Album(albumName, new Uri("/Skins/nocoverart.jpg", UriKind.Relative), AuthorCollection.Last().AuthorName));
                }

                AuthorCollection.Last().AlbumCollection.Last().AddSong(_song);
            }
        }

        #endregion

        #region Public Properties

        private ObservableCollection<Author> _authorCollection;
        public ObservableCollection<Author> AuthorCollection
        {
            get { return _authorCollection; } 
        }


        private string _countRenamedFiles;
        public string CountRenamedFiles
        {
            get { return _countRenamedFiles; }
            set
            {
                _countRenamedFiles = value;
                RaisePropertyChanged(() => CountRenamedFiles);
            }
        }

        private int _progressRenamedFiles;
        public int ProgressRenamedFiles
        {
            get { return _progressRenamedFiles; }
            set
            {
                _progressRenamedFiles = value;
                RaisePropertyChanged(() => ProgressRenamedFiles);
            }
            
        }
        #endregion

        #region Private Properties
        private MenuItem _checkedLanguageLastMenuItem;
        private readonly MusicLibraryView _musicLibraryView;
        private readonly EditTagsWindow _editTagsWindow;
        private readonly Options _optionsWindow;
        private readonly AboutWindow _aboutWindow;
        private event EventHandler<FileTryingRenamedEventArgs> NewFileRenamed;
        #endregion

        #region Commands
        public RelayCommand<CheckBox> ClickAlbumCommand { get; private set; } 
        public RelayCommand<CheckBox> ClickAuthorCommand { get; private set; }
        public RelayCommand OpenFilesCommand { get; private set; }
        public RelayCommand OpenDirectoryCommand { get; private set; }
        public RelayCommand<Window> ExitCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }
        public RelayCommand<DragEventArgs> DragCommand{get;private set;}
        public RelayCommand RenameCheckedCommand{get; private set; }
        public RelayCommand OpenTemplateOptionWindow { get; private set; }
        public RelayCommand<MenuItem> ChangeLanguageCommand { get; private set; }
        public RelayCommand<bool> SelectAllCommand { get; private set; }
        public RelayCommand<bool> DeSelectAllCommand { get; private set; }
        public RelayCommand EditTagsCommand { get; private set; } 
        public RelayCommand<KeyEventArgs> ListViewKeyDownCommand { get; private set; }
        public RelayCommand<Album> FindImageMenuCommand { get; set; }
        public RelayCommand<MusicLibraryViewModel> MainWindowLoadedCommand { get; set; }
        public RelayCommand<MusicLibraryViewModel> MainWindowClosingCommand { get; private set; }
        #endregion
    }


    public class FileTryingRenamedEventArgs:EventArgs
    {
        public int CountFilesRenamed;
        public int CountFiles;
        public FileTryingRenamedEventArgs(int countRenamed, int countAll)
        {
            CountFilesRenamed = countRenamed;
            CountFiles = countAll;
        }
    }
}