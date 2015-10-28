using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
        private readonly List<string> _filesList;
        private readonly TemplateForm _template;

        public MainForm()
        {
            InitializeComponent();
            _history = new ChangesHistory();
            _template = new TemplateForm();
            _filesList = new List<string>();
            UndoRedoManager.Instance().RedoStackStatusChanged += MainForm_RedoStackStatusChanged;
            UndoRedoManager.Instance().UndoStackStatusChanged += MainForm_UndoStackStatusChanged;
            UndoRedoManager.Instance().Clear();
        }

        #region Load files to list

        private void openFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AddUserControls(openFileDialog1.FileNames);
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
                AddUserControls(folderBrowserDialog1.SelectedPath);
            }
        }

        private void AddUserControls(string[] files)
        {
            FilesTableLayoutPanel.Controls.Clear();

            if (!files.Any())
            {
                MessageBox.Show(Resources.Mp3_Files_not_found_, Resources.Please_Attention);
            }
            else
            {
                FillArtistList(files);
            }
        }

        private void AddUserControls(string workdirectory)
        {
            FilesTableLayoutPanel.Controls.Clear();

            var selectedFiles = Directory.GetFiles(workdirectory, "*.mp3", SearchOption.AllDirectories);

            if (selectedFiles.Any())
            {
                MessageBox.Show(Resources.Mp3_Files_not_found_, Resources.Please_Attention);
            }
            else
            {
                FillArtistList(selectedFiles);
            }
        }

        private void FilesTableLayoutPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            deleteAllFromListToolStripMenuItem_Click(sender, EventArgs.Empty);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var directoryName = (string[]) e.Data.GetData(DataFormats.FileDrop);
                var ext = directoryName[0].Substring(directoryName[0].Length - 4);
                if (ext == ".mp3") //если добавляют файлы
                {
                    AddUserControls(directoryName);
                }
                else
                {
                    var dropfilesList =
                        directoryName.SelectMany(dir => Directory.GetFiles(dir, "*.mp3", SearchOption.AllDirectories))
                            .ToList();

                    AddUserControls(dropfilesList.ToArray());
                }
            }
        }

        private void FillArtistList(IEnumerable<string> selectedFiles)
        {
            var albumsNamesList = new List<List<string>>();
            var artistsNamesList = new List<string>();
            var artistsList = new List<Author>();
            foreach (var path in selectedFiles)
            {
                var mp3File = TagLibFile.Create(path);
                var author = new Author(mp3File.Tag.FirstPerformer);
                var album = new Album(mp3File.Tag.Year + " - " + mp3File.Tag.Album);
                if (artistsNamesList.IndexOf(mp3File.Tag.FirstPerformer) == -1)
                {
                    artistsNamesList.Add(mp3File.Tag.FirstPerformer);
                    albumsNamesList.Add(new List<string> {mp3File.Tag.Year + " - " + mp3File.Tag.Album});

                    author.albums.Add(album);
                    author.albums[0].songs.Add(path);
                    artistsList.Add(author);
                }
                else
                {
                    var i = artistsNamesList.IndexOf(mp3File.Tag.FirstPerformer);
                    if (albumsNamesList[i].Contains(mp3File.Tag.Year + " - " + mp3File.Tag.Album) == false)
                    {
                        albumsNamesList[i].Add(mp3File.Tag.Year + " - " + mp3File.Tag.Album);
                        artistsList[i].albums.Add(album);
                        artistsList[i].albums[artistsList[i].albums.Count - 1].songs.Add(path);
                    }
                    else
                    {
                        var j = albumsNamesList[i].IndexOf(mp3File.Tag.Year + " - " + mp3File.Tag.Album);
                        artistsList[i].albums[j].songs.Add(path);
                    }
                }
            }
            SetFilesOnForm(artistsList);
        }

        private void SetFilesOnForm(IEnumerable<Author> artistsList)
        {
            FilesTableLayoutPanel.RowStyles.Clear();
            var list = new List<CheckBoxArtist>();
            var i = 0;
            foreach (var author in artistsList)
            {
                list.Add(new CheckBoxArtist());
                list[i].tableLayoutPanel2.RowStyles.Clear();
                list[i].tableLayoutPanel1.RowStyles.Clear();
                FilesTableLayoutPanel.RowCount = i + 1;
                FilesTableLayoutPanel.Controls.Add(list[i], 0, i);
                list[i].Location = new Point(0, 0);
                list[i].Dock = DockStyle.Fill;
                list[i].Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
                list[i].Margin = new Padding(4);
                list[i].Name = author._name;
                list[i].TabIndex = i;
                list[i].artistCheckBox.Text = author._name;
                list[i].AddNewAlbum(author.albums);
                FilesTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, list[i].Size.Height));
                i++;
            }
        }

        /// <summary>
        ///     Получение списка всех чеканых файлов
        /// </summary>
        /// <returns></returns>
        private List<string> GetListCheckedFiles()
        {
            var list = FilesTableLayoutPanel.Controls;
            if (list.Count == 0)
            {
                return null;
            }
            var files =
                list.Cast<CheckBoxArtist>().SelectMany(artist => artist.getCheckedTracksFromArtist()).ToList();
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
            if (_filesList.Any())
            {
                FilesTableLayoutPanel.Controls.Clear();
                FillArtistList(_filesList.ToArray());
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
                        if (album.checkedListBox1.GetItemChecked(i))
                        {
                            album.checkedListBox1.Items.RemoveAt(i);
                        }
                        else i++;
                    } while (album.checkedListBox1.CheckedItems.Count != 0);
                }
            }
        }

        private void deleteAllFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilesTableLayoutPanel.Controls.Clear();
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

            _filesList.Clear();

            var expression = _template.expressionFiles;
            foreach (var file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                var mp3File = TagLibFile.Create(file);
                StaticMethods.TagsToUpper(mp3File);
                var tempName = StaticMethods.GetNewNameByTemplate(expression, mp3File.Tag);
                tempName = StaticMethods.DeleteBannedSymbols(tempName);

                var folder = Path.GetDirectoryName(file);
                var finalPath = String.Format("{0}\\{1}.mp3", folder, tempName);

                var i = 1;
                bool skip = false;
                while (File.Exists(finalPath))
                {
                    if (finalPath == file)
                    {
                        break;
                    }
                    if (MessageBox.Show(String.Format("File with name {0} is already exists. Skip this file?", finalPath), Resources.Please_Attention, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    {
                        finalPath = String.Format("{0}\\{1}({2}).mp3", folder, tempName, i);
                        i++;
                    }
                    else
                    {
                        skip = true;

                        break;
                    }
                }

                if (!skip)
                {
                    try
                    {
                        File.Move(file, finalPath);
                        _history.WriteLine("File Renamed. From " + file + " To " + finalPath);
                    }
                    catch (IOException ex)
                    {
                        _history.WriteLine(ex);
                    }
                }

                _filesList.Add(finalPath);
            }

            refreshToolStripMenuItem_Click(sender, EventArgs.Empty);
        }

        private void playCheckedFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var files = GetListCheckedFiles();
            var player = files == null ? new PlayerForm() : new PlayerForm(files.ToArray());
            player.Show();
        }
        #endregion

    }
}