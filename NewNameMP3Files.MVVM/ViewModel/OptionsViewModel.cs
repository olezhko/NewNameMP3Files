using System;
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
            ExampleTemplateForFiles_TextChangedCommand = new RelayCommand(ExampleTemplateForFiles_TextChangedMethod);
            ExampleTemplateForDirectory_TextChangedCommand = new RelayCommand(ExampleTemplateForDirectory_TextChangedMethod);
            ExampleTemplateForFiles = "Example: 07 - Satan's Children";
            ExampleTemplateForDirectory = "Example: ../Ancient/2001 Proxima Centauri/07 - Satan's Children";
            expressionFiles = TemplateForFiles;
            expressionDirectory = TemplateForDirectory;
        }

        private void AcceptMethod()
        {
            expressionFiles = TemplateForFiles;
            expressionDirectory = TemplateForDirectory;
            //Close();
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

        public RelayCommand AcceptCommand {get; private set; }

        public RelayCommand ExampleTemplateForFiles_TextChangedCommand { get; private set; }
        public RelayCommand ExampleTemplateForDirectory_TextChangedCommand { get; private set; }
    }
}
