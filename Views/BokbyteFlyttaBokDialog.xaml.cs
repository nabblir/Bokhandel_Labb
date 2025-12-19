using Bokhandel_Labb.ViewModels;
using System.Windows;

namespace Bokhandel_Labb.Views
    {
    public partial class BokbyteFlyttaBokDialog : Window
        {
        public int Antal { get; private set; }

        public BokbyteFlyttaBokDialog(string bokTitel, int maxAntal, string tillButikNamn, string frånButikNamn)
            {
            InitializeComponent();

            Owner = Application.Current.Windows.OfType<BokbyteView>().FirstOrDefault();

            var viewModel = new BokbyteFlyttaBokDialogViewModel(bokTitel, maxAntal, tillButikNamn, frånButikNamn, this);
            DataContext = viewModel;
            }

        protected override void OnClosed(EventArgs e)
            {
            base.OnClosed(e);
            if (DataContext is BokbyteFlyttaBokDialogViewModel vm)
                {
                Antal = vm.Antal;
                }
            }
        }
    }