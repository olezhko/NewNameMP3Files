using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using NewNameMP3Files.MVVM.Model;
using TagLibFile = TagLib.File;
using System.Threading.Tasks;
using System.Windows.Controls;
using NewNameMP3Files.MVVM.Skins;

namespace NewNameMP3Files.MVVM.ViewModel
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
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            AuthorCollection = new ObservableCollection<Author>();
            RenameCheckedCommand = new RelayCommand(RenameAction);
            OpenFilesCommand = new RelayCommand(OpenFilesMethod);
            OpenDirectoryCommand = new RelayCommand(OpenDirectoryMethod);
            ExitCommand = new RelayCommand<Window>(ExitMethod);
            AboutCommand = new RelayCommand(AboutMethod);
            DragCommand = new RelayCommand<DragEventArgs>(DragEnterAuthorsListViewMethod);
            OpenTemplateOptionWindow = new RelayCommand(OpenTemplateWindowMethod);
            ChangeLanguageCommand = new RelayCommand<MenuItem>(ChangeLanguageMethod);
            SelectAllCommand = new RelayCommand(SelectAllMethod);
            _optionsWindow = new Options();
        }

        private void SelectAllMethod()
        {
            foreach (var song in from author in AuthorCollection from album in author.AlbumCollection from song in album.SongsCollection select song)
            {
                song.IsSelected = true;
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
            if (res.HasValue && res.Value)
            {
                var viewModel = (OptionsViewModel)_optionsWindow.DataContext;
                if (viewModel!=null)
                {
                    _renameExpression = viewModel.TemplateForFiles;
                }
            }
        }

        private void RenameAction()
        {
            List<string> files = GetListCheckedFiles();
            if (files == null || !files.Any())
            {
                return;
            }

            NewFileRenamed += (send, args) =>
            {
                CountRenamedFiles = String.Format("{0}/{1}", args, files.Count);
                int percent = args * 100 / files.Count;
                ProgressRenamedFiles = percent;
                if (percent == 100)
                {
                    RefreshMethod();
                    MessageBox.Show("Done");
                }
            };

            Task.Factory.StartNew(() => RenameAction(_renameExpression, files));
        }

        private void RefreshMethod()
        {
            
        }

        private List<string> GetListCheckedFiles()
        {
            return (from author in AuthorCollection from album in author.AlbumCollection from song in album.SongsCollection where song.IsSelected select song.Path).ToList();
        }

        private event EventHandler<int> NewFileRenamed;
        private void RenameAction(string expression, List<string> filesPathList)
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

                File.Move(file, finalPath);

                count++;
                if (NewFileRenamed != null)
                {
                    NewFileRenamed(this, count);
                }
            }
        }

        private void DragEnterAuthorsListViewMethod(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var dragItems = (string[])e.Data.GetData(DataFormats.FileDrop);
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
            var files = Directory.EnumerateFiles(direcorypath, "*.*", SearchOption.AllDirectories)
.Where(s => s.EndsWith(".mp3") || s.EndsWith(".m4a") || s.EndsWith(".ogg"));

            foreach (var file in files)
            {
                AddSongToList(file);
            }
        }

        private void AddSongToList(string filepath)
        {
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
                        author.AlbumCollection.Add(new Album(mp3File.Tag.Year + " - " + mp3File.Tag.Album));
                        author.AlbumCollection.Last().AddSong(mp3File);
                    }
                }
            }

            if (res == -1)
            {
                AuthorCollection.Add(new Author(mp3File.Tag.FirstPerformer));
                AuthorCollection.Last().AlbumCollection.Add(new Album(mp3File.Tag.Year + " - " + mp3File.Tag.Album));
                AuthorCollection.Last().AlbumCollection.Last().AddSong(mp3File);
            }
        }

        private void AboutMethod()
        {

        }

        private void ExitMethod(Window wnd)
        {
            if (wnd != null)
            {
                wnd.Close();
            }
        }

        private void OpenDirectoryMethod()
        {
            
        }

        private void OpenFilesMethod()
        {
            OpenFileDialog ofd = new OpenFileDialog {Multiselect = true};
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

        #region Public Properties
        public ObservableCollection<Author> AuthorCollection { get; set; }

        public string CountRenamedFiles { get; set; }
        
        public int ProgressRenamedFiles { get; set; }

        public MenuItem LanguageMenuItem { get; set; }

        #endregion

        #region Private Properties
        private MenuItem _checkedLanguageLastMenuItem;
        private readonly Options _optionsWindow;
        private string _renameExpression = "(n) - (t)";
        #endregion

        #region Commands

        public RelayCommand OpenFilesCommand { get; private set; }
        public RelayCommand OpenDirectoryCommand { get; private set; }
        public RelayCommand<Window> ExitCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }
        public RelayCommand<DragEventArgs> DragCommand
        {
            get;
            private set;
        }
        public RelayCommand RenameCheckedCommand
        {
            get; private set; }
        public RelayCommand OpenTemplateOptionWindow { get; private set; }
        public RelayCommand<MenuItem> ChangeLanguageCommand { get; private set; }
        public RelayCommand SelectAllCommand { get; private set; }

        #endregion
    }
}