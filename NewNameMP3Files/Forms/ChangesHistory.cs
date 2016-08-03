using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace NewNameMP3Files.Forms
{
    public partial class ChangesHistory : Form
    {
        public string FileName;
        private readonly object _locked = new object();

        public ChangesHistory()
        {
            InitializeComponent();
            LoadHistory();
            var data = DateTime.Now;
            FileName = Directory.GetCurrentDirectory() + "//History//" + data.Year + "-" + checkDate(data.Month) + "-" +
                       checkDate(data.Day) + "_" + ".nn3h";
        }

        /// <summary>
        ///     Загрузка истории изменений из файла
        /// </summary>
        private void LoadHistory()
        {
            if (Directory.Exists("History"))
            {
                var files = Directory.GetFiles("History");
                foreach (var file in files)
                {
                    var fileCreation = System.IO.File.GetCreationTime(file).ToString(CultureInfo.InvariantCulture);
                    var lines = System.IO.File.ReadAllLines(file);
                    foreach (var line in lines)
                    {
                        if (line.Length > 22)
                        {
                            dataGridView1.Rows.Add(fileCreation, Directory.GetCurrentDirectory() + "\\" + file,
    line.Substring(0, 19), line.Substring(21));
                        }
                    }
                }
            }
        }

        private string checkDate(int info)
        {
            if (info > 9)
            {
                return info.ToString();
            }
            return "0" + info;
        }

        public void WriteLine(object message)
        {
            if (Directory.Exists("History") == false)
            {
                Directory.CreateDirectory("History");
            }
            try
            {
                lock (_locked)
                {
                    using (var sw = System.IO.File.AppendText(FileName))
                    {
                        var data = DateTime.Now;
                        var template = data.Year + "-" + checkDate(data.Month) + "-" + checkDate(data.Day) + " " +
                                       checkDate(data.Hour) + ":" + checkDate(data.Minute) + ":" +
                                       checkDate(data.Second) + ":  " + message;
                        sw.WriteLine(template);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}