using FilesBoxing.Class;
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
            var settings = new Settings();
            settings.FillBySource();
            var viewModel = new MainViewModel(new Factory(), settings);
            DataContext = viewModel;
        }
    }
}
