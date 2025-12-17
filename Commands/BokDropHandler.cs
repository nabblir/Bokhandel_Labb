using Bokhandel_Labb.ViewModels;
using Bokhandel_Labb.Views;
using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Bokhandel_Labb.Helpers
    {
    public class BokDropHandler : IDropTarget
        {
        private readonly BokbyteViewModel _viewModel;

        public BokDropHandler(BokbyteViewModel viewModel)
            {
            _viewModel = viewModel;
            }

        public void DragOver(IDropInfo dropInfo)
            {
            if (dropInfo.Data is LagerSaldoDTO)
                {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
                }
            }

        public void Drop(IDropInfo dropInfo)
            {
            var bok = dropInfo.Data as LagerSaldoDTO;
            if (bok == null)
                return;

            var sourceCollection = dropInfo.DragInfo.SourceCollection as ObservableCollection<LagerSaldoDTO>;
            var targetCollection = dropInfo.TargetCollection as ObservableCollection<LagerSaldoDTO>;

            // Bestäm från och till butiker
            string frånButik, tillButik;

            if (sourceCollection == _viewModel.Butik1Böcker)
                {
                frånButik = _viewModel.ValdButik1?.ButiksNamn ?? "Okänd";
                tillButik = _viewModel.ValdButik2?.ButiksNamn ?? "Okänd";
                }
            else
                {
                frånButik = _viewModel.ValdButik2?.ButiksNamn ?? "Okänd";
                tillButik = _viewModel.ValdButik1?.ButiksNamn ?? "Okänd";
                }

            // Visa dialog
            var dialog = new AntalInputDialog(bok.Titel, bok.AntalILager, tillButik, frånButik);
            dialog.Owner = Application.Current.Windows.OfType<BokbyteView>().FirstOrDefault();

            if (dialog.ShowDialog() == true)
                {
                var antalAttFlytta = dialog.Antal;

                if (sourceCollection != null && targetCollection != null)
                    {
                    var existingBok = targetCollection.FirstOrDefault(b => b.Isbn == bok.Isbn);

                    if (existingBok != null)
                        {
                        existingBok.AntalILager += antalAttFlytta;
                        }
                    else
                        {
                        targetCollection.Add(new LagerSaldoDTO
                            {
                            Isbn = bok.Isbn,
                            Titel = bok.Titel,
                            FörfattareNamn = bok.FörfattareNamn,
                            AntalILager = antalAttFlytta,
                            ButikId = bok.ButikId
                            });
                        }

                    bok.AntalILager -= antalAttFlytta;

                    if (bok.AntalILager <= 0)
                        {
                        sourceCollection.Remove(bok);
                        }
                    }
                }
            }
        }
    }