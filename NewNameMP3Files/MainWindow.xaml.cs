using System.Windows;
using NewNameMP3Files.Properties;
using NewNameMP3Files.ViewModel;

namespace NewNameMP3Files
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => 
            {
                Settings.Default.Save();
                ViewModelLocator.Cleanup();
            };
        }
    }
}