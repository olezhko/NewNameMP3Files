using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewNameMP3Files.Code;
using NewNameMP3Files.Forms;
using NewNameMP3Files.Properties;
using UndoMethods;
using TagLibFile = TagLib.File;

namespace NewNameMP3Files
{
    public partial class MainForm : Form
    {
        private ChangesHistory _history;
        private readonly TemplateForm _template;
        private readonly List<Author> _arthistList;
        private readonly List<string> _allPaths;

        public MainForm()
        {
            InitializeComponent();
            _history = new ChangesHistory();
            _template = new TemplateForm();
            _arthistList = new List<Author>();
            _allPaths = new List<string>();

            UndoRedoManager.Instance().RedoStackStatusChanged += MainForm_RedoStackStatusChanged;
            UndoRedoManager.Instance().UndoStackStatusChanged += MainForm_UndoStackStatusChanged;
            UndoRedoManager.Instance().Clear(); 
        }

        #region Load files to list

        private void FilesTableLayoutPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var dragItems = (string[])e.Data.GetData(DataFormats.FileDrop);
                _allPaths.AddRange(dragItems);

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

            SetFilesOnForm(_arthistList);
        }

        private void ClearAllItems()
        {
            FilesTableLayoutPanel.Controls.Clear();
            _arthistList.Clear();
        }

        private void AddDirectoryToList(string direcorypath)
        {
            var files = Directory.EnumerateFiles(direcorypath, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".mp3") || s.EndsWith(".m4a") || s.EndsWith(".ogg"));

            AddSongsToList(files.ToArray());
        }

        private void AddSongsToList(string[] songsPaths)
        {
            foreach (string song in songsPaths)
            {
                AddSongToList(song);
            }
        }

        private void AddSongToList(string filepath)
        {
            try
            {
                var mp3File = TagLibFile.Create(filepath);
                var album = mp3File.Tag.Year + " - " + mp3File.Tag.Album;


                int res = -1;
                foreach (Author author in _arthistList)
                {
                    var arthist = author;
                    if (arthist.Name.Equals(mp3File.Tag.FirstPerformer))
                    {
                        res = 1;
                        var resAlbums = -1;
                        for (int albumIndex = 0; albumIndex < arthist.Albums.Count; albumIndex++)
                        {
                            if (author.Albums[albumIndex].Name.Equals(album))
                            {
                                resAlbums = 1;
                                author.Albums[albumIndex].Songs.Add(filepath);
                            }
                        }

                        if (resAlbums == -1)
                        {
                            author.Albums.Add(new Album(mp3File.Tag.Year + " - " + mp3File.Tag.Album));
                            author.Albums.Last().Songs.Add(filepath);
                        }
                    }
                }

                if (res == -1)
                {
                    _arthistList.Add(new Author(mp3File.Tag.FirstPerformer));
                    _arthistList.Last().Albums.Add(new Album(mp3File.Tag.Year + " - " + mp3File.Tag.Album));
                    _arthistList.Last().Albums.Last().Songs.Add(filepath);
                }
            }
            catch
            {
                // ignored
            }
        }

        private void SetFilesOnForm(IEnumerable<Author> artistsList)
        {
            FilesTableLayoutPanel.RowStyles.Clear();
            var list = new List<CheckBoxArtist>();
            var i = 0;
            var authors = artistsList as Author[] ?? artistsList.ToArray();
            FilesTableLayoutPanel.RowCount = authors.Count();
            foreach (var author in authors)
            {
                list.Add(new CheckBoxArtist());
                list[i].tableLayoutPanel2.RowStyles.Clear();
                list[i].ArtistTableLayoutPanel.RowStyles.Clear();
                FilesTableLayoutPanel.Controls.Add(list[i], 0, i);
                list[i].Location = new Point(0, 0);
                list[i].Dock = DockStyle.Fill;
                list[i].Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
                list[i].Margin = new Padding(4);
                list[i].Name = author.Name;
                list[i].artistCheckBox.Text = author.Name;
                list[i].AddNewAlbum(author.Albums);
                FilesTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, list[i].ControlHeight));
                i++;
            }
        }

        private void openFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _allPaths.AddRange(openFileDialog1.FileNames);
                AddSongsToList(openFileDialog1.FileNames);
            }
        }

        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectWorkingDirectory();
        }

        private void SelectWorkingDirectory()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _allPaths.Add(folderBrowserDialog1.SelectedPath);
                AddDirectoryToList(folderBrowserDialog1.SelectedPath);
            }
        }

        private List<string> GetListCheckedFiles()
        {
            var list = FilesTableLayoutPanel.Controls;
            if (list.Count == 0)
            {
                return null;
            }
            var files = list.Cast<CheckBoxArtist>().SelectMany(artist => artist.GetCheckedTracksFromArtist()).ToList();
            return files;
        }
        #endregion

        #region Context Menu Tools - Pre-actions

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var list = FilesTableLayoutPanel.Controls;
            if (list.Count != 0)
            {
                foreach (CheckBoxArtist artist in list)
                {
                    artist.artistCheckBox.Checked = true;
                }
            }
        }

        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var list = FilesTableLayoutPanel.Controls;
            if (list.Count != 0)
            {
                foreach (CheckBoxArtist artist in list)
                {
                    artist.artistCheckBox.Checked = false;
                }
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_arthistList.Count != 0)
            {
                ClearAllItems();
                foreach (string item in _allPaths)
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
                SetFilesOnForm(_arthistList);
            }
        }

        private void deleteCheckedFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var list = FilesTableLayoutPanel.Controls;
            foreach (CheckBoxArtist artist in list)
            {
                foreach (var album in artist._listAlbums)
                {
                    var i = 0;
                    do
                    {
                        if (album.SongsCheckedListBox.GetItemChecked(i))
                        {
                            album.SongsCheckedListBox.Items.RemoveAt(i);
                        }
                        else i++;
                    } while (album.SongsCheckedListBox.CheckedItems.Count != 0);
                }
            }
        }

        private void deleteAllFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAllItems();
            _allPaths.Clear();
        }

        #endregion

        #region Menu

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     Вывод окна "О программе"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ap = new AboutProgram();
            ap.Show();
        }

        /// <summary>
        ///     Вывод окна, в котором вводится шаблон для переименовывания файлов и директорий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void templateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _template.Show();
        }

        /// <summary>
        ///     Вывод истории в программе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewInProgramToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _history = new ChangesHistory();
            _history.Show();
        }

        /// <summary>
        ///     Открытие директории с файлами истории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewInFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory() + "\\History\\");
        }

        #endregion

        #region Undo-Redo

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoRedoManager.Instance().Undo();
            refreshToolStripMenuItem_Click(sender, EventArgs.Empty);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoRedoManager.Instance().Redo();
            refreshToolStripMenuItem_Click(sender, EventArgs.Empty);
        }

        private void MainForm_UndoStackStatusChanged(bool hasItems)
        {
            undoToolStripMenuItem.Enabled = hasItems;
        }

        private void MainForm_RedoStackStatusChanged(bool hasItems)
        {
            redoToolStripMenuItem.Enabled = hasItems;
        }

        #endregion

        #region Menu Actions

        private void makeCODEForTorrentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    var finalText = new List<string>();

                    var directories = Directory.GetDirectories(fbd.SelectedPath, "*", SearchOption.AllDirectories);
                    foreach (var dir in directories)
                    {
                        var nameOfDirectory = Path.GetDirectoryName(dir);
                        var filesDir = Directory.GetFiles(dir, "*.mp3", SearchOption.TopDirectoryOnly);


                        if (filesDir.Any())
                        {
                            var bitrates =
                                filesDir.Select(TagLibFile.Create)
                                    .Select(mp3File => mp3File.Properties.AudioBitrate)
                                    .ToList();
                            var bitrate = StaticMethods.GetMaxAndMinBitrate(bitrates);
                            nameOfDirectory += "  " + bitrate + "kbps" + ":[/b]";
                            //get the name of directory and bitrate of files in this dir
                            finalText.Add("[b]" + nameOfDirectory);
                            finalText.Add("[spoiler]");
                            finalText.AddRange(filesDir.Select(Path.GetDirectoryName));
                            finalText.Add("[/spoiler]");
                        }
                    }


                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllLines(saveFileDialog1.FileName, finalText.ToArray(), Encoding.Unicode);
                        StaticMethods.WorkDoneMessage();
                    }
                    _history.WriteLine("Code For Torrents. File Saved to: " + Environment.CurrentDirectory +
                                       "\\discografy.txt");
                }
            }
        }

        private void makePlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var files = GetListCheckedFiles();
            var playlistFileText = new List<string>();

            if (files != null)
            {
                if (files.Any())
                {
                    MessageBox.Show(Resources.Mp3_Files_not_found_, Resources.Please_Attention);
                }
                else
                {
                    playlistFileText.Add("#EXTM3U");
                    foreach (var file in files)
                    {
                        var mp3File = TagLibFile.Create(file);
                        var t = mp3File.Properties.Duration.Minutes*60 + mp3File.Properties.Duration.Seconds;
                        playlistFileText.Add("#EXTINF:" + t + "," + mp3File.Tag.FirstPerformer + " - " +
                                             mp3File.Tag.Title);
                        playlistFileText.Add(file);
                    }
                    if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllLines(saveFileDialog2.FileName, playlistFileText.ToArray(),
                            Encoding.Unicode);
                    }
                    _history.WriteLine("New PlayList. File Saved to: " + saveFileDialog2.FileName);
                    StaticMethods.WorkDoneMessage();
                }
            }
            else
            {
                MessageBox.Show(Resources.Mp3_Files_not_found_, Resources.Please_Attention);
            }
        }

        private void organizeFilesToDirectoriesByTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string expression = _template.expressionDirectory;
            //if (expression[0] != '/')
            //    MessageBox.Show("Template is invalid. First symbol should be /.");

            //List<string> files = null;
            //try
            //{
            //    files = GetListCheckedFiles();
            //    if (!files.Any())
            //    {
            //        MessageBox.Show("Mp3 Files not checked!", "Please Attention");
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(), "Attention");
            //    return;
            //}

            //foreach (string file in files)
            //{
            //    var mp3File = TagLib.File.Create(file);
            //    TagsToUpper(mp3File);
            //    string tempName = userNameSettings(expression, mp3File.Tag);

            //    string[] directories = tempName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            //    string dir1 = workDirectory + "\\" + directories[0];
            //    string dir2 = workDirectory + "\\" + directories[0] + "\\" + directories[1];
            //    DirectoryInfo inf1 = Directory.CreateDirectory(dir1);
            //    DirectoryInfo inf2 = Directory.CreateDirectory(dir2);

            //    string folder = file.Substring(0, file.LastIndexOf("\\", StringComparison.Ordinal));
            //    folder = folder.Substring(0, folder.LastIndexOf("\\", StringComparison.Ordinal));
            //    string finalPath = folder + tempName + ".mp3";
            //    System.IO.File.Move(file, finalPath);
            //    _history.WriteLine("File Renamed. From:" + file + " To " + finalPath);
            //}
            //WorkDoneMessage();
        }

        private void renameCheckedFilesByTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var files = GetListCheckedFiles();
            if (files == null || !files.Any())
            {
                return;
            }

            NewFileRenamed += (send, args) =>
            {
                Invoke(new Action(() =>
                {
                    countRenamedFilesStatusStrip.Text = String.Format("{0}/{1}", args, files.Count);
                    int percent = args*100/files.Count;
                    progressRenamedFilesStatusStrip.Value = percent;
                    if (percent == 100)
                    {
                        refreshToolStripMenuItem_Click(sender, EventArgs.Empty);
                    }
                }));
            };

            var expression = _template.expressionFiles;
            Task.Factory.StartNew(() => RenameAction(expression,files));
        }


        private event EventHandler<int> NewFileRenamed;
        private void RenameAction(string expression, List<string> filesPathList)
        {
            int count = 0;
            foreach (var file in filesPathList)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                var mp3File = TagLibFile.Create(file);
                StaticMethods.TagsToUpper(mp3File);
                var tempName = StaticMethods.GetNewNameByTemplate(expression, mp3File.Tag);
                tempName = StaticMethods.DeleteBannedSymbols(tempName);

                var folder = Path.GetDirectoryName(file);
                var finalPath = String.Format("{0}\\{1}{2}", folder, tempName, Path.GetExtension(file));

                var i = 1;
                while (File.Exists(finalPath))
                {
                    if (finalPath.Equals(file,StringComparison.Ordinal))
                    {
                        break;
                    }
                    finalPath = String.Format("{0}\\{1}({2}){3}", folder, tempName, i, Path.GetExtension(file));
                    i++;
                }

                try
                {
                    File.Move(file, finalPath);
                    _history.WriteLine("File Renamed. From " + file + " To " + finalPath);
                }
                catch (IOException ex)
                {
                    _history.WriteLine(ex);
                }

                count++;
                if (NewFileRenamed != null)
                {
                    NewFileRenamed(this, count);
                }
            }
        }
        #endregion

    }
}