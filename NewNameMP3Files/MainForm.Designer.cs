using System;
using System.Collections.Generic;
using System.IO;
using NewNameMP3Files.Code;

namespace NewNameMP3Files
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newNameMP3FilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changesHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewInFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewInProgramToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makePlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.organizeFilesToDirectoriesByTagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeCODEForTorrentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.русскийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.белорусскийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.templateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webMoneyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yandexMoneyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.countRenamedFilesStatusStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressRenamedFilesStatusStrip = new System.Windows.Forms.ToolStripProgressBar();
            this.SongListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCheckedFromListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllFromListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameCheckedFilesByTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SongListContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Chouse Working Directory";
            this.folderBrowserDialog1.SelectedPath = "D:\\1";
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            this.saveFileDialog1.FileName = "discografy";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "Torrents Discography File";
            // 
            // saveFileDialog2
            // 
            this.saveFileDialog2.DefaultExt = "m3u";
            this.saveFileDialog2.FileName = "playlist";
            this.saveFileDialog2.RestoreDirectory = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newNameMP3FilesToolStripMenuItem,
            this.historyToolStripMenuItem,
            this.actionsToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(834, 27);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newNameMP3FilesToolStripMenuItem
            // 
            this.newNameMP3FilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFilesToolStripMenuItem,
            this.openDirectoryToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.newNameMP3FilesToolStripMenuItem.Name = "newNameMP3FilesToolStripMenuItem";
            this.newNameMP3FilesToolStripMenuItem.Size = new System.Drawing.Size(41, 23);
            this.newNameMP3FilesToolStripMenuItem.Text = "File";
            // 
            // openFilesToolStripMenuItem
            // 
            this.openFilesToolStripMenuItem.Name = "openFilesToolStripMenuItem";
            this.openFilesToolStripMenuItem.Size = new System.Drawing.Size(172, 24);
            this.openFilesToolStripMenuItem.Text = "Open Files";
            this.openFilesToolStripMenuItem.Click += new System.EventHandler(this.openFilesToolStripMenuItem_Click);
            // 
            // openDirectoryToolStripMenuItem
            // 
            this.openDirectoryToolStripMenuItem.Name = "openDirectoryToolStripMenuItem";
            this.openDirectoryToolStripMenuItem.Size = new System.Drawing.Size(172, 24);
            this.openDirectoryToolStripMenuItem.Text = "Open Directory";
            this.openDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openDirectoryToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(172, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // historyToolStripMenuItem
            // 
            this.historyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changesHistoryToolStripMenuItem,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.historyToolStripMenuItem.Name = "historyToolStripMenuItem";
            this.historyToolStripMenuItem.Size = new System.Drawing.Size(53, 23);
            this.historyToolStripMenuItem.Text = "Tools";
            // 
            // changesHistoryToolStripMenuItem
            // 
            this.changesHistoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewInFileToolStripMenuItem1,
            this.viewInProgramToolStripMenuItem1});
            this.changesHistoryToolStripMenuItem.Name = "changesHistoryToolStripMenuItem";
            this.changesHistoryToolStripMenuItem.Size = new System.Drawing.Size(122, 24);
            this.changesHistoryToolStripMenuItem.Text = "History";
            // 
            // viewInFileToolStripMenuItem1
            // 
            this.viewInFileToolStripMenuItem1.Name = "viewInFileToolStripMenuItem1";
            this.viewInFileToolStripMenuItem1.Size = new System.Drawing.Size(215, 24);
            this.viewInFileToolStripMenuItem1.Text = "View History Directory";
            this.viewInFileToolStripMenuItem1.Click += new System.EventHandler(this.viewInFileToolStripMenuItem1_Click);
            // 
            // viewInProgramToolStripMenuItem1
            // 
            this.viewInProgramToolStripMenuItem1.Name = "viewInProgramToolStripMenuItem1";
            this.viewInProgramToolStripMenuItem1.Size = new System.Drawing.Size(215, 24);
            this.viewInProgramToolStripMenuItem1.Text = "View In Program";
            this.viewInProgramToolStripMenuItem1.Click += new System.EventHandler(this.viewInProgramToolStripMenuItem1_Click);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(122, 24);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(122, 24);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.makePlaylistToolStripMenuItem,
            this.organizeFilesToDirectoriesByTagsToolStripMenuItem,
            this.makeCODEForTorrentsToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(66, 23);
            this.actionsToolStripMenuItem.Text = "Actions";
            // 
            // makePlaylistToolStripMenuItem
            // 
            this.makePlaylistToolStripMenuItem.Name = "makePlaylistToolStripMenuItem";
            this.makePlaylistToolStripMenuItem.Size = new System.Drawing.Size(302, 24);
            this.makePlaylistToolStripMenuItem.Text = "Make Playlist";
            this.makePlaylistToolStripMenuItem.Click += new System.EventHandler(this.makePlaylistToolStripMenuItem_Click);
            // 
            // organizeFilesToDirectoriesByTagsToolStripMenuItem
            // 
            this.organizeFilesToDirectoriesByTagsToolStripMenuItem.Name = "organizeFilesToDirectoriesByTagsToolStripMenuItem";
            this.organizeFilesToDirectoriesByTagsToolStripMenuItem.Size = new System.Drawing.Size(302, 24);
            this.organizeFilesToDirectoriesByTagsToolStripMenuItem.Text = "Organize Files To Directories by Tags";
            this.organizeFilesToDirectoriesByTagsToolStripMenuItem.Click += new System.EventHandler(this.organizeFilesToDirectoriesByTagsToolStripMenuItem_Click);
            // 
            // makeCODEForTorrentsToolStripMenuItem
            // 
            this.makeCODEForTorrentsToolStripMenuItem.Name = "makeCODEForTorrentsToolStripMenuItem";
            this.makeCODEForTorrentsToolStripMenuItem.Size = new System.Drawing.Size(302, 24);
            this.makeCODEForTorrentsToolStripMenuItem.Text = "Make CODE for Torrents";
            this.makeCODEForTorrentsToolStripMenuItem.Click += new System.EventHandler(this.makeCODEForTorrentsToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeLanguageToolStripMenuItem,
            this.templateToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(70, 23);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // changeLanguageToolStripMenuItem
            // 
            this.changeLanguageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.русскийToolStripMenuItem,
            this.белорусскийToolStripMenuItem});
            this.changeLanguageToolStripMenuItem.Name = "changeLanguageToolStripMenuItem";
            this.changeLanguageToolStripMenuItem.Size = new System.Drawing.Size(189, 24);
            this.changeLanguageToolStripMenuItem.Text = "Change Language";
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Checked = true;
            this.englishToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.englishToolStripMenuItem.Text = "English";
            // 
            // русскийToolStripMenuItem
            // 
            this.русскийToolStripMenuItem.Enabled = false;
            this.русскийToolStripMenuItem.Name = "русскийToolStripMenuItem";
            this.русскийToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.русскийToolStripMenuItem.Text = "Русский";
            // 
            // белорусскийToolStripMenuItem
            // 
            this.белорусскийToolStripMenuItem.Enabled = false;
            this.белорусскийToolStripMenuItem.Name = "белорусскийToolStripMenuItem";
            this.белорусскийToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.белорусскийToolStripMenuItem.Text = "Беларускiй";
            // 
            // templateToolStripMenuItem
            // 
            this.templateToolStripMenuItem.Name = "templateToolStripMenuItem";
            this.templateToolStripMenuItem.Size = new System.Drawing.Size(189, 24);
            this.templateToolStripMenuItem.Text = "Templates";
            this.templateToolStripMenuItem.Click += new System.EventHandler(this.templateToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.donateToolStripMenuItem,
            this.userGuideToolStripMenuItem,
            this.aboutProgramToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(49, 23);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.webMoneyToolStripMenuItem,
            this.yandexMoneyToolStripMenuItem,
            this.dToolStripMenuItem});
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(173, 24);
            this.donateToolStripMenuItem.Text = "Donate";
            this.donateToolStripMenuItem.Visible = false;
            // 
            // webMoneyToolStripMenuItem
            // 
            this.webMoneyToolStripMenuItem.Name = "webMoneyToolStripMenuItem";
            this.webMoneyToolStripMenuItem.Size = new System.Drawing.Size(165, 24);
            this.webMoneyToolStripMenuItem.Text = "WebMoney";
            this.webMoneyToolStripMenuItem.Click += new System.EventHandler(this.webMoneyToolStripMenuItem_Click);
            // 
            // yandexMoneyToolStripMenuItem
            // 
            this.yandexMoneyToolStripMenuItem.Name = "yandexMoneyToolStripMenuItem";
            this.yandexMoneyToolStripMenuItem.Size = new System.Drawing.Size(165, 24);
            this.yandexMoneyToolStripMenuItem.Text = "YandexMoney";
            this.yandexMoneyToolStripMenuItem.Click += new System.EventHandler(this.yandexMoneyToolStripMenuItem_Click);
            // 
            // userGuideToolStripMenuItem
            // 
            this.userGuideToolStripMenuItem.Name = "userGuideToolStripMenuItem";
            this.userGuideToolStripMenuItem.Size = new System.Drawing.Size(173, 24);
            this.userGuideToolStripMenuItem.Text = "User Guide";
            this.userGuideToolStripMenuItem.Visible = false;
            // 
            // aboutProgramToolStripMenuItem
            // 
            this.aboutProgramToolStripMenuItem.Name = "aboutProgramToolStripMenuItem";
            this.aboutProgramToolStripMenuItem.Size = new System.Drawing.Size(173, 24);
            this.aboutProgramToolStripMenuItem.Text = "About Program";
            this.aboutProgramToolStripMenuItem.Click += new System.EventHandler(this.aboutProgramToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "mp3";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.countRenamedFilesStatusStrip,
            this.progressRenamedFilesStatusStrip});
            this.statusStrip1.Location = new System.Drawing.Point(0, 491);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(834, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // countRenamedFilesStatusStrip
            // 
            this.countRenamedFilesStatusStrip.Name = "countRenamedFilesStatusStrip";
            this.countRenamedFilesStatusStrip.Size = new System.Drawing.Size(24, 17);
            this.countRenamedFilesStatusStrip.Text = "0/0";
            // 
            // progressRenamedFilesStatusStrip
            // 
            this.progressRenamedFilesStatusStrip.Name = "progressRenamedFilesStatusStrip";
            this.progressRenamedFilesStatusStrip.Size = new System.Drawing.Size(100, 16);
            // 
            // SongListContextMenuStrip
            // 
            this.SongListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.deselectAllToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.deleteCheckedFromListToolStripMenuItem,
            this.deleteAllFromListToolStripMenuItem,
            this.renameCheckedFilesByTemplateToolStripMenuItem});
            this.SongListContextMenuStrip.Name = "SongListContextMenuStrip";
            this.SongListContextMenuStrip.Size = new System.Drawing.Size(262, 136);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // deselectAllToolStripMenuItem
            // 
            this.deselectAllToolStripMenuItem.Name = "deselectAllToolStripMenuItem";
            this.deselectAllToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.deselectAllToolStripMenuItem.Text = "Deselect All";
            this.deselectAllToolStripMenuItem.Click += new System.EventHandler(this.deselectAllToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // deleteCheckedFromListToolStripMenuItem
            // 
            this.deleteCheckedFromListToolStripMenuItem.Name = "deleteCheckedFromListToolStripMenuItem";
            this.deleteCheckedFromListToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.deleteCheckedFromListToolStripMenuItem.Text = "Delete Checked From List";
            this.deleteCheckedFromListToolStripMenuItem.Click += new System.EventHandler(this.deleteCheckedFromListToolStripMenuItem_Click);
            // 
            // deleteAllFromListToolStripMenuItem
            // 
            this.deleteAllFromListToolStripMenuItem.Name = "deleteAllFromListToolStripMenuItem";
            this.deleteAllFromListToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.deleteAllFromListToolStripMenuItem.Text = "Delete All From List";
            this.deleteAllFromListToolStripMenuItem.Click += new System.EventHandler(this.deleteAllFromListToolStripMenuItem_Click);
            // 
            // renameCheckedFilesByTemplateToolStripMenuItem
            // 
            this.renameCheckedFilesByTemplateToolStripMenuItem.Name = "renameCheckedFilesByTemplateToolStripMenuItem";
            this.renameCheckedFilesByTemplateToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.renameCheckedFilesByTemplateToolStripMenuItem.Text = "Rename Checked Files by Template";
            this.renameCheckedFilesByTemplateToolStripMenuItem.Click += new System.EventHandler(this.renameCheckedFilesByTemplateToolStripMenuItem_Click);
            // 
            // FilesTableLayoutPanel
            // 
            this.FilesTableLayoutPanel.AllowDrop = true;
            this.FilesTableLayoutPanel.AutoScroll = true;
            this.FilesTableLayoutPanel.ColumnCount = 1;
            this.FilesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.FilesTableLayoutPanel.ContextMenuStrip = this.SongListContextMenuStrip;
            this.FilesTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilesTableLayoutPanel.Location = new System.Drawing.Point(0, 27);
            this.FilesTableLayoutPanel.Name = "FilesTableLayoutPanel";
            this.FilesTableLayoutPanel.RowCount = 1;
            this.FilesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.FilesTableLayoutPanel.Size = new System.Drawing.Size(834, 464);
            this.FilesTableLayoutPanel.TabIndex = 3;
            this.FilesTableLayoutPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.FilesTableLayoutPanel_DragEnter);
            // 
            // dToolStripMenuItem
            // 
            this.dToolStripMenuItem.Name = "dToolStripMenuItem";
            this.dToolStripMenuItem.Size = new System.Drawing.Size(165, 24);
            this.dToolStripMenuItem.Text = "D";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(834, 513);
            this.Controls.Add(this.FilesTableLayoutPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "New Name MP3 Files ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.SongListContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newNameMP3FilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changesHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewInFileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem viewInProgramToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeLanguageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem русскийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem белорусскийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem templateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem webMoneyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yandexMoneyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutProgramToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makePlaylistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem organizeFilesToDirectoriesByTagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeCODEForTorrentsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip SongListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deselectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCheckedFromListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllFromListToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel FilesTableLayoutPanel;
        private System.Windows.Forms.ToolStripMenuItem renameCheckedFilesByTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel countRenamedFilesStatusStrip;
        private System.Windows.Forms.ToolStripProgressBar progressRenamedFilesStatusStrip;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem;
    }
}

