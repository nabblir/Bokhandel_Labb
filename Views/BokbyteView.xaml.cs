using Bokhandel_Labb.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Bokhandel_Labb.Views
    {
    public partial class BokbyteView : Window
        {
        public BokbyteView(BokbyteViewModel viewModel)
            {
            InitializeComponent();
            DataContext = viewModel;
            }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            {
            if (DataContext is BokbyteViewModel viewModel && viewModel.ValdBok != null)
                {
                viewModel.ÄndraLagersaldoCommand?.Execute(null);
                }
            }
        }
    }