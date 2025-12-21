using Bokhandel_Labb.DTOs;
using Bokhandel_Labb.ViewModels;
using Bokhandel_Labb.Views;
using GongSolutions.Wpf.DragDrop;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bokhandel_Labb.Helpers
    {
    public class BokDropHandler : IDropTarget
        {
        private readonly BokbyteViewModel _viewModel;
        private BokbyteView _view;
        private bool _isOverTrash = false; // STATE TRACKING

        public BokDropHandler(BokbyteViewModel viewModel)
            {
            _viewModel = viewModel;
            }

        public void DragOver(IDropInfo dropInfo)
            {
            var sourceItem = dropInfo.Data as LagerSaldoDTO;
            if (sourceItem == null)
                {
                dropInfo.Effects = DragDropEffects.None;

                // Återställ om vi lämnar drag helt
                if (_isOverTrash)
                    {
                    _isOverTrash = false;
                    }
                return;
                }

            // Lazy load view reference
            if (_view == null)
                {
                _view = Application.Current.Windows.OfType<BokbyteView>().FirstOrDefault();
                }

            var sourceListBox = GetListBoxFromVisualElement(dropInfo.DragInfo.VisualSource);
            var targetElement = dropInfo.VisualTarget;

            // Kolla om target är papperskorgen
            if (IsTrashBorder(targetElement))
                {
                dropInfo.Effects = DragDropEffects.Move;
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;

                // Animera bara om vi inte redan är över papperskorgen
                if (!_isOverTrash)
                    {
                    _isOverTrash = true;
                    }
                return;
                }
            else
                {
                // Vi är inte över papperskorg - återställ om vi var det tidigare
                if (_isOverTrash)
                    {
                    _isOverTrash = false;
                    }
                }

            var targetListBox = GetListBoxFromVisualElement(targetElement);
            if (targetListBox == null)
                {
                dropInfo.Effects = DragDropEffects.None;
                return;
                }

            // Förhindra drop på samma ListBox
            if (sourceListBox == targetListBox)
                {
                dropInfo.Effects = DragDropEffects.None;
                return;
                }

            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            }

        public void Drop(IDropInfo dropInfo)
            {
            var draggedBook = dropInfo.Data as LagerSaldoDTO;
            if (draggedBook == null)
                return;

            // Återställ state och animering
            _isOverTrash = false;

            var sourceListBox = GetListBoxFromVisualElement(dropInfo.DragInfo.VisualSource);
            var targetElement = dropInfo.VisualTarget;

            var sourceCollection = sourceListBox?.Tag?.ToString() == "Butik1"
                ? _viewModel.Butik1Böcker
                : _viewModel.Butik2Böcker;

            var sourceButik = sourceListBox?.Tag?.ToString() == "Butik1"
                ? _viewModel.ValdButik1
                : _viewModel.ValdButik2;

            // Kolla om target är papperskorgen
            if (IsTrashBorder(targetElement))
                {
                var result = MessageBox.Show(
                    $"Är du säker på att du vill ta bort '{draggedBook.Titel}' från {sourceButik.ButiksNamn}?",
                    "Bekräfta borttagning",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                    {
                    
                    sourceCollection.Remove(draggedBook);

                    _viewModel.VisaVarning($"'{draggedBook.Titel}' borttagen från {sourceButik.ButiksNamn}. Klicka 'Spara Ändringar' för att verkställa.");
                    }
                return;
                }

            // Drop på ListBox
            var targetListBox = GetListBoxFromVisualElement(targetElement);
            if (targetListBox == null || sourceListBox == targetListBox)
                return;

            var targetCollection = targetListBox.Tag?.ToString() == "Butik1"
                ? _viewModel.Butik1Böcker
                : _viewModel.Butik2Böcker;

            var targetButik = targetListBox.Tag?.ToString() == "Butik1"
                ? _viewModel.ValdButik1
                : _viewModel.ValdButik2;

            var dialog = new BokbyteFlyttaBokDialog(
                draggedBook.Titel,
                draggedBook.AntalILager,
                targetButik.ButiksNamn,
                sourceButik.ButiksNamn);

            if (dialog.ShowDialog() == true)
                {
                int antalAttFlytta = dialog.Antal;

                draggedBook.AntalILager -= antalAttFlytta;

                if (draggedBook.AntalILager <= 0)
                    {
                    sourceCollection.Remove(draggedBook);
                    }

                var existingBook = targetCollection.FirstOrDefault(b => b.Isbn == draggedBook.Isbn);
                if (existingBook != null)
                    {
                    existingBook.AntalILager += antalAttFlytta;
                    }
                else
                    {
                    targetCollection.Add(new LagerSaldoDTO
                        {
                        Isbn = draggedBook.Isbn,
                        Titel = draggedBook.Titel,
                        FörfattareNamn = draggedBook.FörfattareNamn,
                        AntalILager = antalAttFlytta,
                        ButikId = targetButik.ButikId
                        });
                    }

                _viewModel.VisaSucces($"✓ {antalAttFlytta} st '{draggedBook.Titel}' flyttat till {targetButik.ButiksNamn}");
                }
            }

        private bool IsTrashBorder(object element)
            {
            if (element is Border border)
                {
                // Matcha ENDAST TrashBorder med exakt namn
                if (border.Name == "TrashBorder")
                    {
                    return true;
                    }
                }

            if (element is FrameworkElement fe)
                {
                var parent = fe.Parent;
                while (parent != null)
                    {
                    if (parent is Border parentBorder && parentBorder.Name == "TrashBorder")
                        {
                        return true;
                        }

                    if (parent is FrameworkElement parentFe)
                        parent = parentFe.Parent;
                    else
                        break;
                    }
                }

            return false;
            }

        private ListBox GetListBoxFromVisualElement(object element)
            {
            if (element is ListBox listBox)
                return listBox;

            if (element is FrameworkElement fe)
                {
                var parent = fe.Parent;
                while (parent != null)
                    {
                    if (parent is ListBox lb)
                        return lb;

                    if (parent is FrameworkElement parentFe)
                        parent = parentFe.Parent;
                    else
                        break;
                    }
                }

            return null;
            }
        }
    }