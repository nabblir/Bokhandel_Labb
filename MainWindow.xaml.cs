using Bokhandel_Labb.ViewModels;
using System.Windows;

namespace Bokhandel_Labb
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        public MainWindow()
            {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            }
        }
    }