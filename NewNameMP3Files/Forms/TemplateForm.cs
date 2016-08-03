using System;
using System.Windows.Forms;

namespace NewNameMP3Files
{
    public partial class TemplateForm : Form
    {
        public string expressionFiles;
        public string expressionDirectory;
        public TemplateForm()
        {
            InitializeComponent();
            expressionFiles = textBox1.Text;
            expressionDirectory = textBox2.Text;
            toolTip1.SetToolTip(textBox2,"In this field you enter a path from working directory.\nThis display how will be organized you working directory.\n'/' -> next dir");
            toolTip1.SetToolTip(label9, "In this field you enter a path from working directory.\nThis display how will be organized you working directory.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            expressionFiles = textBox1.Text;
            expressionDirectory = textBox2.Text;
            Close();
        }
        //label14
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label14.Text = "Example: " + userNameSettings(textBox1.Text);
        }
        //13
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label13.Text = "Example: " + userNameSettings(textBox2.Text);
        }

        private string userNameSettings(string name)
        {
            if (name.Contains("(a)"))
            {
                string a = labelAlbum.Text;
                name = name.Replace("(a)", a);
            }
            if (name.Contains("(y)"))
            {
                name = name.Replace("(y)", labelYear.Text);
            }
            if (name.Contains("(t)"))
            {
                string t = labelTitle.Text;
                name = name.Replace("(t)", t);
            }
            if (name.Contains("(n)"))
            {
                string n = labelNumber.Text;
                name = name.Replace("(n)", n);
            }
            if (name.Contains("(p)"))
            {
                string p = labelPerfomer.Text;
                name = name.Replace("(p)", p);
            }
            return name;
        }
    }
}
