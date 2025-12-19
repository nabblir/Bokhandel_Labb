using Bokhandel_Labb.ViewModels;
using System.Windows;
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

        // Anropas från BokDropHandler
        public void AnimateTrashEnter()
            {
            var storyboard = (Storyboard)this.Resources["TrashScaleUpStoryboard"];
            storyboard?.Begin();
            }

        public void AnimateTrashLeave()
            {
            var storyboard = (Storyboard)this.Resources["TrashScaleDownStoryboard"];
            storyboard?.Begin();
            }
        }
    }