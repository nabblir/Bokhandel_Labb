using Bokhandel_Labb.ViewModels;
using System.Windows;

namespace Bokhandel_Labb.Views
    {
    public partial class BokbyteÄndraLagerBokDialog : Window
        {
        public int Antal { get; private set; }

        public BokbyteÄndraLagerBokDialog(string bokTitel, int antalILager)
            {
            InitializeComponent();

            Owner = Application.Current.Windows.OfType<BokbyteView>().FirstOrDefault();

            var viewModel = new BokbyteÄndraLagerBokDialogViewModel(bokTitel, antalILager, this);
            DataContext = viewModel;
            }

        protected override void OnClosed(EventArgs e)
            {
            base.OnClosed(e);
            if (DataContext is BokbyteÄndraLagerBokDialogViewModel vm)
                {
                Antal = vm.Antal;
                }
            }
        }
    }