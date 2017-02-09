using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Data;
using System.Xml;
using GalaSoft.MvvmLight;
using System.IO;

namespace NewNameMP3Files.MVVM.ViewModel
{
    public class AboutViewModel:ViewModelBase
    {

        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Product { get; set; }
        public string Copyright { get; set; }

        public string Company { get; set; }
        public string LinkText { get; set; }
        public string LinkUri { get; set; }

        public AboutViewModel()
        {
            Title = "NewNameMp3Files. About";
            Version = "1.0.0.0";
            Description = "";
            Product = "NewNameMp3Files";
            Company = "O.L.A Company";
            Copyright = "Copyright 2017";
        }
    }
}
