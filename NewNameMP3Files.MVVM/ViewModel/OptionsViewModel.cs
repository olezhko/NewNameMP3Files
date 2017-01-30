using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace NewNameMP3Files.MVVM.ViewModel
{
    public class OptionsViewModel:ViewModelBase
    {
        public string expressionFiles;
        public string expressionDirectory;
        public OptionsViewModel()
        {
            AcceptCommand = new RelayCommand(AcceptMethod);


            expressionFiles = textBox1.Text;
            expressionDirectory = textBox2.Text;
            toolTip1.SetToolTip(textBox2, "In this field you enter a path from working directory.\nThis display how will be organized you working directory.\n'/' -> next dir");
            toolTip1.SetToolTip(label9, "In this field you enter a path from working directory.\nThis display how will be organized you working directory.");
        }

        private void AcceptMethod()
        {
            expressionFiles = textBox1.Text;
            expressionDirectory = textBox2.Text;
            Close();
        }
        //label14
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label14.Text = "Example: " + UserNameSettings(textBox1.Text);
        }
        //13
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label13.Text = "Example: " + UserNameSettings(textBox2.Text);
        }

        private string UserNameSettings(string name)
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




        public RelayCommand AcceptCommand
        {
            get; private set; }
    }
}
