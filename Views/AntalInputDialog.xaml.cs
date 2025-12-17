using Bokhandel_Labb.ViewModels;
using System.Windows;

namespace Bokhandel_Labb.Views
    {
    public partial class AntalInputDialog : Window
        {
        public AntalInputDialogViewModel ViewModel { get; }

        public AntalInputDialog(string bokTitel, int maxAntal, string tillButikNamn, string frånButikNamn)
            {
            InitializeComponent();
            ViewModel = new AntalInputDialogViewModel(bokTitel, maxAntal, tillButikNamn, frånButikNamn, this);
            DataContext = ViewModel;
            }

        public int Antal => ViewModel.Antal;
        }
    }