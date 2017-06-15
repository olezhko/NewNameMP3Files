using System;
using System.IO;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NewNameMP3Files.Properties;

namespace NewNameMP3Files.ViewModel
{
    public class OptionsViewModel:ViewModelBase
    {
        public OptionsViewModel()
        {
            AcceptCommand = new RelayCommand<Window>(AcceptMethod);
            ExampleTemplateForFilesTextChangedCommand = new RelayCommand(ExampleTemplateForFiles_TextChangedMethod);
            ExampleTemplateForDirectoryTextChangedCommand = new RelayCommand(ExampleTemplateForDirectory_TextChangedMethod);
            BrowseMusicLibraryPathCommand = new RelayCommand(BrowseMusicLibraryPathMethod);
            ExampleTemplateForFiles = "Example: 07 - Satan's Children";
            ExampleTemplateForDirectory = "Example: ../Ancient/2001 Proxima Centauri/07 - Satan's Children";
            TemplateForFiles = "(n) - (t)";
            TemplateForDirectory = @"\(p)\(y) (a)\(n) - (t)";
        }

        private void BrowseMusicLibraryPathMethod()
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Directory.Exists(fbd.SelectedPath))
                {
                    MusicLibraryPath = fbd.SelectedPath;
                }
            }
        }

        private void AcceptMethod(Window wnd)
        {
            wnd.DialogResult = true;
        }

        private void ExampleTemplateForFiles_TextChangedMethod()
        {
            ExampleTemplateForFiles = "Example: " + UserNameSettings(TemplateForFiles);
        }

        private void ExampleTemplateForDirectory_TextChangedMethod()
        {
            ExampleTemplateForDirectory = "Example: " + UserNameSettings(TemplateForDirectory);
        }

        private string UserNameSettings(string name)
        {
            if (name.Contains("(a)"))
            {
                string a = "Proxima Centauri";
                name = name.Replace("(a)", a);
            }
            if (name.Contains("(y)"))
            {
                name = name.Replace("(y)", "2001");
            }
            if (name.Contains("(t)"))
            {
                string t = "Satan's Children";
                name = name.Replace("(t)", t);
            }
            if (name.Contains("(n)"))
            {
                string n = "07";
                name = name.Replace("(n)", n);
            }
            if (name.Contains("(p)"))
            {
                string p = "Ancient";
                name = name.Replace("(p)", p);
            }
            return name;
        }

        public string TemplateForFiles { get; set; }
        public string TemplateForDirectory { get; set; }
        public string ExampleTemplateForFiles { get; set; }
        public string ExampleTemplateForDirectory { get; set; }

        public string MusicLibraryPath
        {
            get { return Settings.Default.MusicLibraryPath; }
            set
            {
                Settings.Default.MusicLibraryPath = value;
                RaisePropertyChanged(()=> MusicLibraryPath);
            }
        }
        public RelayCommand<Window> AcceptCommand { get; private set; }
        public RelayCommand ExampleTemplateForFilesTextChangedCommand { get; private set; }
        public RelayCommand ExampleTemplateForDirectoryTextChangedCommand { get; private set; }
        public RelayCommand BrowseMusicLibraryPathCommand { get; private set; }
    }
}
