namespace NewNameMP3Files.UserControls
{
    partial class CheckBoxAlbum
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AlbumTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SongsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.checkBoxAlbumName = new System.Windows.Forms.CheckBox();
            this.AlbumTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // AlbumTableLayoutPanel
            // 
            this.AlbumTableLayoutPanel.AutoSize = true;
            this.AlbumTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.AlbumTableLayoutPanel.ColumnCount = 1;
            this.AlbumTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AlbumTableLayoutPanel.Controls.Add(this.SongsCheckedListBox, 0, 1);
            this.AlbumTableLayoutPanel.Controls.Add(this.checkBoxAlbumName, 0, 0);
            this.AlbumTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AlbumTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.AlbumTableLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.AlbumTableLayoutPanel.Name = "AlbumTableLayoutPanel";
            this.AlbumTableLayoutPanel.RowCount = 2;
            this.AlbumTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AlbumTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AlbumTableLayoutPanel.Size = new System.Drawing.Size(434, 58);
            this.AlbumTableLayoutPanel.TabIndex = 3;
            // 
            // SongsCheckedListBox
            // 
            this.SongsCheckedListBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.SongsCheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SongsCheckedListBox.CheckOnClick = true;
            this.SongsCheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SongsCheckedListBox.FormattingEnabled = true;
            this.SongsCheckedListBox.Location = new System.Drawing.Point(5, 35);
            this.SongsCheckedListBox.Margin = new System.Windows.Forms.Padding(4);
            this.SongsCheckedListBox.Name = "SongsCheckedListBox";
            this.SongsCheckedListBox.Size = new System.Drawing.Size(424, 18);
            this.SongsCheckedListBox.TabIndex = 1;
            this.SongsCheckedListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.checkedListBox1_KeyDown);
            // 
            // checkBoxAlbumName
            // 
            this.checkBoxAlbumName.AutoSize = true;
            this.checkBoxAlbumName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxAlbumName.Location = new System.Drawing.Point(5, 5);
            this.checkBoxAlbumName.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAlbumName.Name = "checkBoxAlbumName";
            this.checkBoxAlbumName.Size = new System.Drawing.Size(71, 21);
            this.checkBoxAlbumName.TabIndex = 0;
            this.checkBoxAlbumName.Text = "Album";
            this.checkBoxAlbumName.UseVisualStyleBackColor = true;
            this.checkBoxAlbumName.CheckedChanged += new System.EventHandler(this.checkBoxAlbumName_CheckedChanged);
            // 
            // CheckBoxAlbum
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Controls.Add(this.AlbumTableLayoutPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CheckBoxAlbum";
            this.Size = new System.Drawing.Size(434, 58);
            this.AlbumTableLayoutPanel.ResumeLayout(false);
            this.AlbumTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TableLayoutPanel AlbumTableLayoutPanel;
        public System.Windows.Forms.CheckBox checkBoxAlbumName;
        public System.Windows.Forms.CheckedListBox SongsCheckedListBox;
    }
}
