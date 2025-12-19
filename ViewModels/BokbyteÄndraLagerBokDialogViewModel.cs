using Bokhandel_Labb.Commands;
using System.Windows;
using System.Windows.Input;

namespace Bokhandel_Labb.ViewModels
    {
    public class BokbyteÄndraLagerBokDialogViewModel : BaseViewModel
        {
        private string _bokTitel;
        public string BokTitel
            {
            get => _bokTitel;
            set => SetProperty(ref _bokTitel, value);
            }

        private int _antalILager;
        public int AntalILager
            {
            get => _antalILager;
            set => SetProperty(ref _antalILager, value);
            }

        private string _antalText;
        public string AntalText
            {
            get => _antalText;
            set
                {
                if (SetProperty(ref _antalText, value))
                    {
                    // Uppdatera varning när text ändras
                    VisaVarning = value == "0";
                    }
                }
            }

        private bool _visaVarning;
        public bool VisaVarning
            {
            get => _visaVarning;
            set => SetProperty(ref _visaVarning, value);
            }

        public int Antal { get; private set; }
        public ICommand OKCommand { get; }
        public ICommand CancelCommand { get; }
        private Window _window;

        public BokbyteÄndraLagerBokDialogViewModel(string bokTitel, int antalILager, Window window)
            {
            BokTitel = bokTitel;
            AntalILager = antalILager;
            AntalText = antalILager.ToString(); // Visa nuvarande antal
            _window = window;
            OKCommand = new RelayCommand(OK);
            CancelCommand = new RelayCommand(Cancel);
            }

        private void OK()
            {
            if (int.TryParse(AntalText, out int antal))
                {
                if (antal >= 0) // Tillåt 0
                    {
                    // Visa bekräftelse om användaren sätter till 0
                    if (antal == 0)
                        {
                        var result = MessageBox.Show(
                            $"Är du säker på att du vill sätta lagersaldot till 0?\n\n'{BokTitel}' kommer att tas bort från butikens lager när du sparar ändringar.",
                            "Bekräfta noll lagersaldo",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result != MessageBoxResult.Yes)
                            {
                            return; // Avbryt om användaren inte bekräftar
                            }
                        }

                    Antal = antal;
                    _window.DialogResult = true;
                    _window.Close();
                    }
                else
                    {
                    MessageBox.Show(
                        "Lagersaldot kan inte vara negativt",
                        "Ogiltigt antal",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    }
                }
            else
                {
                MessageBox.Show(
                    "Ange ett giltigt tal",
                    "Ogiltigt antal",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                }
            }

        private void Cancel()
            {
            _window.DialogResult = false;
            _window.Close();
            }
        }
    }