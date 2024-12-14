using FilesBoxing.Class.Factory;
using FilesBoxing.ViewModel;

namespace FilesBoxing.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var settings = SettingsHelper.SettingsFileBoxing("SETTINGS.XML");
            var viewModel = new MainViewModel(new FullFileBoxingHandler(), settings);
            DataContext = viewModel;
        }
    }
}
