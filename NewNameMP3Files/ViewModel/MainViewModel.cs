using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using TagLibFile = TagLib.File;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MusicLibrary;
using NewNameMP3Files.Model;
using NewNameMP3Files.Skins;
using Application = System.Windows.Application;
using DragEventArgs = System.Windows.DragEventArgs;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using CheckBox = System.Windows.Controls.CheckBox;
using System.Windows.Input;

namespace NewNameMP3Files.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // edit tags
        public MainViewModel()
        {
            _authorCollection = new ObservableCollection<Author>();
            RenameCheckedCommand = new RelayCommand(RenameAction);
            OpenFilesCommand = new RelayCommand(OpenFilesMethod);
            OpenDirectoryCommand = new RelayCommand(OpenDirectoryMethod);
            ExitCommand = new RelayCommand<Window>(ExitMethod);
            AboutCommand = new RelayCommand(AboutMethod);
            DragCommand = new RelayCommand<DragEventArgs>(DragEnterAuthorsListViewMethod);
            OpenTemplateOptionWindow = new RelayCommand(OpenTemplateWindowMethod);
            ChangeLanguageCommand = new RelayCommand<MenuItem>(ChangeLanguageMethod);
            SelectAllCommand = new RelayCommand<bool>(b => SelectAllMethod(true));
            DeSelectAllCommand = new RelayCommand<bool>(b => SelectAllMethod(false));
            EditTagsCommand = new RelayCommand(EditTagsMethod);
            _optionsWindow = new Options();
            _aboutWindow = new AboutWindow();
            _editTagsWindow = new EditTagsWindow();
            ListViewKeyDownCommand = new RelayCommand<KeyEventArgs>(ListViewKeyDownMethod);
            ClickAuthorCommand = new RelayCommand<CheckBox>(AuthorCheckBoxClickMethod);
            ClickAlbumCommand = new RelayCommand<CheckBox>(AlbumCheckBoxClickMethod);
            OpenMusicLibraryWindow = new RelayCommand(OpenMusicLibraryWindowMethod);
            FindImageMenuCommand = new RelayCommand<string>(FindCoverMethod);
        }

        private void FindCoverMethod(string albumName)
        {
            System.Diagnostics.Process.Start(String.Format("https://www.google.by/search?q={0}+cover&source=lnms&tbm=isch&tbs=isz:l",albumName));
        }

        private void ListViewKeyDownMethod(KeyEventArgs args)
        {
            if (args.Key == Key.Delete)
            {
                AuthorCollection.Clear();
            }
        }

        private void OpenMusicLibraryWindowMethod()
        {
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
                viewModel.SongsCollection.Add(new SongViewModel(new Song(TagLibFile.Create(file))));
            }
            _editTagsWindow.ShowDialog();
        }

        private void RefreshMethod()
        {
            Console.WriteLine("RefreshMethod");
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

        private void OpenTemplateWindowMethod()
        {
            var res = _optionsWindow.ShowDialog();
            var viewModel = (OptionsViewModel)_optionsWindow.DataContext;
            if (viewModel != null)
            {
                _renameExpression = viewModel.TemplateForFiles;
            }
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
                    var list = (List<string>)send;
                    Console.WriteLine(String.Format("Done {0}/{1}", args, list.Count));
                    CountRenamedFiles = String.Format("{0}/{1}", args, list.Count);
                    int percent = args * 100 / list.Count;
                    ProgressRenamedFiles = percent;
                    if (percent == 100)
                    {
                        RefreshMethod();
                        MessageBox.Show(App.ResourceDictionary["DoneString"].ToString(), App.ResourceDictionary["InformationString"].ToString());
                    }
                };
            }

            Console.WriteLine("Start Rename Task " + files.Count);
            Task.Factory.StartNew(() => RenameActionTask(_renameExpression, files));
        }

        private void RenameActionTask(string expression, List<string> filesPathList)
        {
            int count = 0;
            foreach (var file in filesPathList)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                var mp3File = TagLib.File.Create(file);
                StaticMethods.TagsToUpper(mp3File);
                var tempName = StaticMethods.GetNewNameByTemplate(expression, mp3File.Tag);
                tempName = StaticMethods.DeleteBannedSymbols(tempName);

                var folder = Path.GetDirectoryName(file);
                var finalPath = String.Format("{0}\\{1}{2}", folder, tempName, Path.GetExtension(file));

                var i = 1;
                while (File.Exists(finalPath))
                {
                    if (finalPath.Equals(file, StringComparison.Ordinal))
                    {
                        break;
                    }
                    finalPath = String.Format("{0}\\{1}({2}){3}", folder, tempName, i, Path.GetExtension(file));
                    i++;
                }


                var index = _renamingFilesList.IndexOf(file);
                if (index!=-1)
                {
                    _renamingFilesList[index] = finalPath;
                }

                File.Move(file, finalPath);
                count++;
                NewFileRenamed?.Invoke(filesPathList, count);
            }
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
            var files = Directory.EnumerateFiles(direcorypath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".mp3") || s.EndsWith(".m4a") || s.EndsWith(".ogg"));

            foreach (var file in files)
            {
                AddSongToList(file);
            }
        }

        private void AddSongToList(string filepath)
        {
            if (!Song.IsFileSong(filepath))
            {
                return;
            }
            var mp3File = TagLib.File.Create(filepath);
            var album = mp3File.Tag.Year + " - " + mp3File.Tag.Album;

            int res = -1;
            foreach (Author author in AuthorCollection)
            {
                var arthist = author;
                if (arthist.AuthorName.Equals(mp3File.Tag.FirstPerformer))
                {
                    res = 1;
                    var resAlbums = -1;
                    for (int albumIndex = 0; albumIndex < arthist.AlbumCollection.Count; albumIndex++)
                    {
                        if (author.AlbumCollection[albumIndex].AlbumName.Equals(album))
                        {
                            resAlbums = 1;
                            author.AlbumCollection[albumIndex].AddSong(mp3File);
                        }
                    }

                    if (resAlbums == -1)
                    {
                        string albumCoverPath = Path.Combine(Path.GetDirectoryName(filepath), "cover.jpg");
                        if (File.Exists(albumCoverPath))
                        {
                            author.AddAlbum(new Album(mp3File.Tag.Year + " - " + mp3File.Tag.Album, albumCoverPath));
                        }
                        else
                        {
                            author.AddAlbum(new Album(mp3File.Tag.Year + " - " + mp3File.Tag.Album, new Uri("/Skins/nocoverart.jpg", UriKind.Relative)));
                        }
                        
                        author.AlbumCollection.Last().AddSong(mp3File);
                        
                    }
                }
            }

            if (res == -1)
            {
                AuthorCollection.Add(new Author(mp3File.Tag.FirstPerformer));

                string albumCoverPath = Path.Combine(Path.GetDirectoryName(filepath), "cover.jpg");
                if (File.Exists(albumCoverPath))
                {
                    AuthorCollection.Last().AddAlbum(new Album(mp3File.Tag.Year + " - " + mp3File.Tag.Album, albumCoverPath));
                }
                else
                {
                    AuthorCollection.Last().AddAlbum(new Album(mp3File.Tag.Year + " - " + mp3File.Tag.Album,new Uri("/Skins/nocoverart.jpg", UriKind.Relative)));
                }

                AuthorCollection.Last().AlbumCollection.Last().AddSong(mp3File);
            }
        }

        #endregion

        #region Public Properties

        private ObservableCollection<Author> _authorCollection;
        public ObservableCollection<Author> AuthorCollection
        {
            get { return _authorCollection; } 
        }
        public MenuItem LanguageMenuItem { get; set; }

        public string _countRenamedFiles;
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
        private readonly EditTagsWindow _editTagsWindow;
        private readonly Options _optionsWindow;
        private readonly AboutWindow _aboutWindow;
        private string _renameExpression = "(n) - (t)";
        private event EventHandler<int> NewFileRenamed;
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
        public RelayCommand OpenMusicLibraryWindow { get; private set; }
        public RelayCommand EditTagsCommand { get; private set; } 
        public RelayCommand<KeyEventArgs> ListViewKeyDownCommand { get; private set; }
        public RelayCommand<string> FindImageMenuCommand { get; set; }
        #endregion
    }
}