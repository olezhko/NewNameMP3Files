using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Data;
using System.Xml;
using GalaSoft.MvvmLight;
using System.IO;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace NewNameMP3Files.ViewModel
{
    public class AboutViewModel:ViewModelBase
    {

        public string Title { get; set; }
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string Description {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }
        public string Product
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }
        public string Copyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string Company
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        public string LinkText { get; set; }
        public string LinkUri { get; set; }

        public AboutViewModel()
        {
            LinkText = "https://github.com/olezhko/NewNameMP3Files";
            ExitCommand = new RelayCommand<Window>(ExitMethod);
        }

        private void ExitMethod(Window obj)
        {
            if (obj!=null)
            {
                obj.Hide();
            }
        }


        public RelayCommand<Window> ExitCommand { get; private set; }
    }
}
