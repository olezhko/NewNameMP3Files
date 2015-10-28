using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NewNameMP3Files.Properties;

namespace NewNameMP3Files
{
    public partial class PlayerForm : Form
    {
        TimeSpan totalduration = new TimeSpan();
        List<string> files = new List<string>();
        int currentPlayingNumber = 0;
        Silence.Audio PlayFile;
        bool isPlaying = false;
        int currentTimePosition = 0;
        bool isStopButtonClicked = false;
        int currentVolume = 100;
        int countSongs = 0;
        public PlayerForm()
        {
            InitializeComponent();
            comboBoxSearch.SelectedIndex = 0;
            Volume.ValueChanged += new EventHandler(Volume_EditValueChanged);
            Position.Scroll += new EventHandler(Position_Scroll);
        }

        public PlayerForm(string[] list)
        {
            for (int i = 0; i < list.Length; i++ )
            {
                files.Add(list[i]);
            }
            InitializeComponent();
            comboBoxSearch.SelectedIndex = 0;
            Volume.ValueChanged += new EventHandler(Volume_EditValueChanged);           
            Position.Scroll += new EventHandler(Position_Scroll);
            LoadListToDataGrid(files);
        }

        void PlayFile_PlaybackStop(EventArgs e)
        {
            if (isStopButtonClicked == false)
            {
                dataGridView1.Rows[currentPlayingNumber].DefaultCellStyle.BackColor = Color.White;
                currentPlayingNumber++;
                dataGridView1.Rows[currentPlayingNumber].DefaultCellStyle.BackColor = Color.PaleTurquoise;
                PlayFile = new Silence.Audio(files[currentPlayingNumber]);

                buttonPlay.BackgroundImage = Resources.pause;
                CurrentTimer.Interval = 1000;
                CurrentTimer.Enabled = true;
                Position.Maximum = (int)PlayFile.TotalDuration.TotalSeconds;
                Position.TickFrequency = 1;
                PlayFile.Play();
                float a = (float)(Volume.Value) / 100;
                PlayFile.Volume = a;
                isPlaying = true;
            }
            else
            {
                isStopButtonClicked = false;
                labelCurrentPosition.Text = "0:00";
            }
        }

        private void LoadListToDataGrid(List<string> filesList)
        {
            foreach (string file in filesList)
            {
                var mp3file = TagLib.File.Create(file);
                string genre = null;
                int i = 0;
                foreach(string g in mp3file.Tag.Genres)
                {
                    if (i!=0)
                    {
                        genre += "/";
                    }
                    genre += g;
                    i++;
                }
                countSongs++;
                totalduration += mp3file.Properties.Duration;
                dataGridView1.Rows.Add(mp3file.Tag.Track, mp3file.Tag.Title, mp3file.Tag.FirstPerformer, mp3file.Tag.Album, mp3file.Tag.Year, genre, mp3file.Properties.Duration.ToString(), mp3file.Properties.AudioBitrate.ToString() + "kbps", file);
            }
            toolStripStatusLabel1.Text = countSongs + " songs - ["+totalduration+"]";
        }

        private void UpdateStatusStrip()
        {
            TimeSpan totalduration = new TimeSpan();
            for (int i = 0; i < dataGridView1.Rows.Count; i++ )
            {
                TimeSpan duration = new TimeSpan();
                TimeSpan.TryParse(dataGridView1.Rows[i].Cells[6].Value.ToString(),out duration);
                totalduration += duration;
            }
            toolStripStatusLabel1.Text = countSongs + " songs - [" + totalduration + "]";
        }

        private void buttonPlay_Click(object sender, System.EventArgs e)
        {
            string path = files[currentPlayingNumber];
            if (PlayFile == null)
            {
                PlayFile = new Silence.Audio(files[currentPlayingNumber]);
                
                PlayFile.PlaybackStop += new Silence.Audio.PlayStopHandle(PlayFile_PlaybackStop);
            }
            var mp3file = TagLib.File.Create(path);
            switch (PlayFile.State)
            {
                case Silence.Audio.PlayBackState.Paused:
                    buttonPlay.BackgroundImage = Resources.pause;
                    dataGridView1.Rows[currentPlayingNumber].DefaultCellStyle.BackColor = Color.PaleTurquoise;
                    PlayFile.Play();
                    this.Text = "Player     " + mp3file.Tag.FirstPerformer + " - "+mp3file.Tag.Title;
                    isPlaying = true;
                    break;
                case Silence.Audio.PlayBackState.Playing:
                    buttonPlay.BackgroundImage = Resources.play;
                    PlayFile.Pause();
                    isPlaying = false;
                    break;
                case Silence.Audio.PlayBackState.Stopped:
                    buttonPlay.BackgroundImage = Resources.pause;
                    CurrentTimer.Interval = 1000; 
                    CurrentTimer.Enabled = true;                  
                    Position.Maximum = (int)PlayFile.TotalDuration.TotalSeconds;
                    Position.TickFrequency = 1;
                    this.Text = "Player     " + mp3file.Tag.FirstPerformer + " - " + mp3file.Tag.Title;
                    PlayFile.Play();
                    dataGridView1.Rows[currentPlayingNumber].DefaultCellStyle.BackColor = Color.PaleTurquoise;
                    Volume.Value = 100;
                    isPlaying = true;
                    break;
            }
        }

        private void Position_Scroll(object sender, EventArgs e)
        {
            if (PlayFile != null)
            {
                PlayFile.TimePosition = TimeSpan.FromSeconds(Position.Value);
                currentTimePosition = Position.Value;
            }
        }

        private void Volume_EditValueChanged(object sender, EventArgs e)
        {
            float a = (float)(Volume.Value) / 100;
            PlayFile.Volume = a;
            if (Volume.Value>=66)
            {
                buttonVolumeSwitch.BackgroundImage = Properties.Resources.volume_up100;
                currentVolume = 100;
            }
            if (Volume.Value >= 33 && Volume.Value < 66)
            {
                buttonVolumeSwitch.BackgroundImage = Properties.Resources.volume_up66;
                currentVolume = 66;
            }
            if (Volume.Value > 0 && Volume.Value < 33)
            {
                buttonVolumeSwitch.BackgroundImage = Properties.Resources.volume_up33;
                currentVolume = 33;
            }
            if (Volume.Value == 0)
            {
                buttonVolumeSwitch.BackgroundImage = Properties.Resources.volume_down;
                currentVolume = 0;
            }
        }
        
        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            isPlaying = false;
            PlayFile.Stop();
            isStopButtonClicked = true;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (PlayFile!=null)
            {
                PlayFile.Stop();
                isPlaying = false;
            }
            currentTimePosition = 0;
            labelCurrentPosition.Text = "0:00";
            try
            {
                dataGridView1.Rows[currentPlayingNumber].DefaultCellStyle.BackColor = Color.White;
            }
            catch (System.Exception)
            {
                return;
            }    
            currentPlayingNumber = e.RowIndex;
            string path = files[currentPlayingNumber];
            var mp3file = TagLib.File.Create(path);
            dataGridView1.Rows[currentPlayingNumber].DefaultCellStyle.BackColor = Color.PaleTurquoise;
            PlayFile = new Silence.Audio(files[currentPlayingNumber]);
            PlayFile.PlaybackStop += new Silence.Audio.PlayStopHandle(PlayFile_PlaybackStop);
            buttonPlay.BackgroundImage = Resources.pause;
            CurrentTimer.Interval = 1000;
            CurrentTimer.Enabled = true;
            Position.Maximum = (int)PlayFile.TotalDuration.TotalSeconds;
            Position.TickFrequency = 1;
            isStopButtonClicked = true;
            PlayFile.Play();
            float a = (float)(Volume.Value) / 100;
            PlayFile.Volume = a;
            isPlaying = true;
            this.Text = "Player     " + mp3file.Tag.FirstPerformer + " - " + mp3file.Tag.Title;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isPlaying == true)
            {
                Position.Value = currentTimePosition++;
            }
            else
            {
                Position.Value = 0;
            }
            if (PlayFile.TimePosition<PlayFile.TotalDuration)
            {
                if (PlayFile.TimePosition.Seconds < 10)
                {
                    labelCurrentPosition.Text = PlayFile.TimePosition.Minutes.ToString() + ":0" + PlayFile.TimePosition.Seconds.ToString();
                }
                else
                {
                    labelCurrentPosition.Text = PlayFile.TimePosition.Minutes.ToString() + ":" + PlayFile.TimePosition.Seconds.ToString();
                }
            }
            else
            {
                PlayFile.Stop();
                currentPlayingNumber++;
                if (countSongs<=currentPlayingNumber)
                {
                    currentPlayingNumber = 0;
                } 
                PlayFile = new Silence.Audio(files[currentPlayingNumber]);
                PlayFile.Play();
                isStopButtonClicked = false;
            }          
        }

        private void buttonVolumeSwitch_Click(object sender, EventArgs e)
        {
            if (currentVolume == 100)
            {
                Volume.Value = 66;
                currentVolume = 66;
                buttonVolumeSwitch.BackgroundImage = Properties.Resources.volume_up66;
                return;
            }
            if (currentVolume == 66)
            {
                Volume.Value = 33;
                currentVolume = 33;
                buttonVolumeSwitch.BackgroundImage = Properties.Resources.volume_up33;
                return;
            }
            if (currentVolume == 33)
            {
                Volume.Value = 0;
                currentVolume = 0;
                buttonVolumeSwitch.BackgroundImage = Properties.Resources.volume_down;
                return;
            }
            if (currentVolume == 0)
            {
                Volume.Value = 100;
                currentVolume = 100;
                buttonVolumeSwitch.BackgroundImage = Properties.Resources.volume_up100;
                return;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {  
                if (countSongs==0)
                {
                    return;
                }
                countSongs--;
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                UpdateStatusStrip();
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.Text == null || textBoxSearch.Text == "" || comboBoxSearch.SelectedItem == null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Visible = true;
                }
            }
            else
            {
                int index = comboBoxSearch.SelectedIndex;
                //dataGridView1.Rows[2].Visible = false;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[index+1].Value.ToString().Contains(textBoxSearch.Text) == false)
                    {
                        dataGridView1.Rows[i].Visible = false;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Visible = true;
                    }
                }
            }
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                string[] directoryName = (string[])e.Data.GetData(DataFormats.FileDrop);
                string ext = directoryName[0].Substring(directoryName[0].Length - 4);
                if (ext == ".mp3")
                {
                    if (files.Count == 0)
                    {
                        for (int i = 0; i < directoryName.Length; i++)
                        {
                            files.Add(directoryName[i]);
                        }
                    } 
                    else
                    {
                        for (int i = 0; i < directoryName.Length; i++)
                        {
                            files.Add(directoryName[i]);
                        }
                    }
                    LoadListToDataGrid(directoryName.OfType<string>().ToList());
                }
                else
                {
                    foreach (string dir in directoryName)
                    {
                        string[] selectedFiles = System.IO.Directory.GetFiles(dir, "*.mp3", SearchOption.AllDirectories);
                        if (selectedFiles.Length == 0)
                        {
                            MessageBox.Show("Mp3 Files not found!", "Please Attention");
                        }
                        else
                        {
                            if (files.Count == 0)
                            {
                                for (int i = 0; i < selectedFiles.Length; i++)
                                {
                                    files.Add(selectedFiles[i]);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < selectedFiles.Length; i++)
                                {
                                    files.Add(selectedFiles[i]);
                                }
                            }
                            LoadListToDataGrid(selectedFiles.OfType<string>().ToList());
                        }
                    }                    
                }
                toolStripStatusLabel1.Text = countSongs + " songs - [" + totalduration + "]";
            }
        }

        private void PlayerForm_FormClosed(object sender, FormClosingEventArgs e)
        {
            if (PlayFile!=null)
            {
                PlayFile.Stop();
            }
            isStopButtonClicked = true;
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = false;
        }
    }
}
