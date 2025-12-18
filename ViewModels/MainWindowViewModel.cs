using Bokhandel_Labb.Commands;
using Bokhandel_Labb.Models;
using Bokhandel_Labb.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Bokhandel_Labb.ViewModels
    {
    public class MainWindowViewModel : BaseViewModel
        {
        public ICommand ÖppnaBokbyteCommand { get; }
        public ICommand ÖppnaRedigeraBokCommand { get; }

        private string _anslutningsStatus;
        public string AnslutningsStatus
            {
            get => _anslutningsStatus;
            set => SetProperty(ref _anslutningsStatus, value);
            }
        public System.Windows.Media.Brush AnslutningsFärg { get; private set; }

        public MainWindowViewModel()
            {
            ÖppnaBokbyteCommand = new RelayCommand(ÖppnaBokbyte);
            ÖppnaRedigeraBokCommand = new RelayCommand(ÖppnaRedigeraBok);
            TestaAnslutning();
            }

        private void ÖppnaBokbyte()
            {
            var viewModel = new BokbyteViewModel();
            var bokbyteWindow = new BokbyteView(viewModel);
            bokbyteWindow.Owner = Application.Current.MainWindow;
            bokbyteWindow.Show();
            }

        private void ÖppnaRedigeraBok()
            {
            var viewModel = new RedigeraBokViewModel();
            var redigeraBokWindow = new RedigeraBokView(viewModel);
            redigeraBokWindow.Owner = Application.Current.MainWindow;
            redigeraBokWindow.Show();
            }

        private void TestaAnslutning()
            {
            using (var context = new BokhandelContext())
                {
                try
                    {
                    var canConnect = context.Database.CanConnect();
                    if (canConnect)
                        {
                        AnslutningsStatus = "🟢 Ansluten";
                        AnslutningsFärg = System.Windows.Media.Brushes.Green;
                        }
                    else
                        {
                        AnslutningsStatus = "🔴 Ej ansluten";
                        AnslutningsFärg = System.Windows.Media.Brushes.Red;
                        }
                    }
                catch (Exception ex)
                    {
                    MessageBox.Show($"Anslutningsfel: {ex.Message}\n\nInner: {ex.InnerException?.Message}",
                        "Databasfel",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    AnslutningsStatus = "🔴Anslutning till databas ej möjlig. Vänligen kontakta ansvarig IT-avdelning";
                    AnslutningsFärg = System.Windows.Media.Brushes.Red;
                    }
                }
            }
        }
    }