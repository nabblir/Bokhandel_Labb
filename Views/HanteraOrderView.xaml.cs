using Bokhandel_Labb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bokhandel_Labb.Views
{
    /// <summary>
    /// Interaction logic for HanteraOrderView.xaml
    /// </summary>
    public partial class HanteraOrderView : Window
    {
        public HanteraOrderView(HanteraOrderViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Order_DoubleClick(object sender, MouseButtonEventArgs e)
            {
            var viewModel = DataContext as HanteraOrderViewModel;
            if (viewModel?.ValdOrder != null)
                {
                var detaljView = new HanteraOrderSingelDialog(viewModel);
                detaljView.ShowDialog();
                }
            }
        }
}
