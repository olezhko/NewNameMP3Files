namespace NewNameMP3Files
{
    partial class CheckBoxArtist
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
            this.artistCheckBox = new System.Windows.Forms.CheckBox();
            this.ArtistTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ArtistTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // artistCheckBox
            // 
            this.artistCheckBox.AutoSize = true;
            this.artistCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.artistCheckBox.Location = new System.Drawing.Point(4, 4);
            this.artistCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.artistCheckBox.Name = "artistCheckBox";
            this.artistCheckBox.Size = new System.Drawing.Size(71, 24);
            this.artistCheckBox.TabIndex = 0;
            this.artistCheckBox.Text = "Artist";
            this.artistCheckBox.UseVisualStyleBackColor = true;
            this.artistCheckBox.CheckedChanged += new System.EventHandler(this.artistCheckBox_CheckedChanged);
            // 
            // ArtistTableLayoutPanel
            // 
            this.ArtistTableLayoutPanel.AutoSize = true;
            this.ArtistTableLayoutPanel.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ArtistTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.ArtistTableLayoutPanel.ColumnCount = 1;
            this.ArtistTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ArtistTableLayoutPanel.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.ArtistTableLayoutPanel.Controls.Add(this.artistCheckBox, 0, 0);
            this.ArtistTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArtistTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ArtistTableLayoutPanel.Name = "ArtistTableLayoutPanel";
            this.ArtistTableLayoutPanel.RowCount = 2;
            this.ArtistTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ArtistTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ArtistTableLayoutPanel.Size = new System.Drawing.Size(608, 73);
            this.ArtistTableLayoutPanel.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 35);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(598, 33);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // CheckBoxArtist
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.ArtistTableLayoutPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CheckBoxArtist";
            this.Size = new System.Drawing.Size(608, 73);
            this.ArtistTableLayoutPanel.ResumeLayout(false);
            this.ArtistTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox artistCheckBox;
        public System.Windows.Forms.TableLayoutPanel ArtistTableLayoutPanel;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
