using Bokhandel_Labb.Commands;
using System.Windows;
using System.Windows.Input;

namespace Bokhandel_Labb.ViewModels
    {
    public class AntalInputDialogViewModel : BaseViewModel
        {
        private string _bokTitel;
        public string BokTitel
            {
            get => _bokTitel;
            set => SetProperty(ref _bokTitel, value);
            }

        private string _tillButikNamn;
        public string TillButikNamn
            {
            get => _tillButikNamn;
            set => SetProperty(ref _tillButikNamn, value);
            }

        private string _frånButikNamn;
        public string FrånButikNamn
            {
            get => _frånButikNamn;
            set => SetProperty(ref _frånButikNamn, value);
            }

        private int _maxAntal;
        public int MaxAntal
            {
            get => _maxAntal;
            set => SetProperty(ref _maxAntal, value);
            }

        private string _antalText;
        public string AntalText
            {
            get => _antalText;
            set => SetProperty(ref _antalText, value);
            }

        public int Antal { get; private set; }

        public ICommand OKCommand { get; }
        public ICommand CancelCommand { get; }

        private Window _window;

        public AntalInputDialogViewModel(string bokTitel, int maxAntal, string tillButikNamn, string frånButikNamn, Window window)
            {
            BokTitel = bokTitel;
            MaxAntal = maxAntal;
            TillButikNamn = tillButikNamn;
            FrånButikNamn = frånButikNamn;
            AntalText = "1";
            _window = window;

            OKCommand = new RelayCommand(OK);
            CancelCommand = new RelayCommand(Cancel);
            }

        private void OK()
            {
            if (int.TryParse(AntalText, out int antal))
                {
                if (antal > 0 && antal <= MaxAntal)
                    {
                    Antal = antal;
                    _window.DialogResult = true;
                    _window.Close();
                    }
                else
                    {
                    MessageBox.Show($"Antal böcker stämmer inte överens med tillgängligt antal: {MaxAntal}",
                        "Ogiltigt antal",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    }
                }
            else
                {
                MessageBox.Show("Ange ett giltigt tal",
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