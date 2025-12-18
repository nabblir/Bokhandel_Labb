using Bokhandel_Labb.Commands;
using Bokhandel_Labb.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics; // Debugg lägg till bok error

namespace Bokhandel_Labb.ViewModels
    {
    public class RedigeraBokViewModel : BaseViewModel
        {
        private readonly BokhandelContext _context;
        private List<string> _listISBN;
        private string _bokTitel;
        public string BokTitel
            {
            get => _bokTitel;
            set
                {
                if (SetProperty(ref _bokTitel, value))
                    {
                    ValideraInputs();
                    }
                }
            }

        private string _isbn;
        public string ISBN
            {
            get => _isbn;
            set
                {
                if (SetProperty(ref _isbn, value))
                    {
                    ValideraInputs();
                    }
                }
            }

        private string _forfattare;
        public string Forfattare
            {
            get => _forfattare;
            set
                {
                if (SetProperty(ref _forfattare, value))
                    {
                    ValideraInputs();
                    }
                }
            }

        private string _antal;
        public string BokAntal
            {
            get => _antal;
            set
                {
                if (SetProperty(ref _antal, value))
                    {
                    ValideraInputs();
                    }
                }
            }
        private string _språk;
        public string Språk
            {
            get => _språk;
            set
                {
                if (SetProperty(ref _språk, value))
                    {
                    ValideraInputs();
                    }
                }
            }

        private bool _enableLäggTillBok = false;
        public bool EnableLäggTillBok
            {
            get => _enableLäggTillBok;
            set => SetProperty(ref _enableLäggTillBok, value);
            }

        public ObservableCollection<ButikDTO> Butik { get; set; }

        private ButikDTO _valdButik;
        public ButikDTO ValdButik
            {
            get => _valdButik;
            set
                {
                if (SetProperty(ref _valdButik, value))
                    {
                    _listISBN = LaddaISBNFrånButik(value);
                    ValideraInputs();
                    }
                }
            }

        private string _statusMeddelande;
        public string StatusMeddelande
            {
            get => _statusMeddelande;
            set => SetProperty(ref _statusMeddelande, value);
            }
        private System.Windows.Media.Brush _statusTextFärg = System.Windows.Media.Brushes.Black;
        public System.Windows.Media.Brush StatusTextFärg
            {
            get => _statusTextFärg;
            set => SetProperty(ref _statusTextFärg, value);
            }
        public ICommand OKCommand { get; }
        public ICommand CancelCommand { get; }


        public RedigeraBokViewModel()
            {
            _context = new BokhandelContext();
            Butik = new ObservableCollection<ButikDTO>();

            OKCommand = new RelayCommand(LäggTillBok);
            CancelCommand = new RelayCommand(Avbryt);

            LaddaButiker();
            }

        private void LaddaButiker()
            {
            try
                {
                var butiker = _context.Butikers.ToList();
                Butik.Clear();
                foreach (var butik in butiker)
                    {
                    Butik.Add(new ButikDTO
                        {
                        ButikId = butik.Id,
                        ButiksNamn = butik.Butiksnamn,
                        Adress = butik.Adress
                        });
                    }

                if (Butik.Count > 0)
                    ValdButik = Butik[0];
                }
            catch (Exception ex)
                {
                MessageBox.Show($"Fel vid laddning av butiker: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        private List<string> LaddaISBNFrånButik(ButikDTO butik)
            {
            if (butik == null)
                return new List<string>();

            try
                {
                return _context.LagerSaldos
                    .Where(l => l.ButikId == butik.ButikId)
                    .Select(l => l.Isbn)
                    .ToList();
                }
            catch (Exception ex)
                {
                MessageBox.Show($"Något gick fel när ISBN nummer laddades: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusTextFärg = System.Windows.Media.Brushes.Red;
                StatusMeddelande = "Kunde inte läsa ISBN nummer från butiken!";
                return new List<string>();
                }
            }


        private void ValideraInputs()
            {
            StatusMeddelande = string.Empty;
            StatusTextFärg = System.Windows.Media.Brushes.Black;

            if (!string.IsNullOrEmpty(BokTitel))
                {
                if (string.IsNullOrWhiteSpace(BokTitel) || BokTitel.Trim().Length < 1)
                    {
                    StatusTextFärg = System.Windows.Media.Brushes.Red;
                    StatusMeddelande = "Boktitel måste ha minst en bokstav";
                    EnableLäggTillBok = false;
                    return;
                    }
                }

            if (!string.IsNullOrEmpty(ISBN))
                {
                if (string.IsNullOrWhiteSpace(ISBN))
                    {
                    StatusTextFärg = System.Windows.Media.Brushes.Red;
                    StatusMeddelande = "ISBN får inte vara tom";
                    EnableLäggTillBok = false;
                    return;
                    }

                if (!ISBN.All(char.IsDigit))
                    {
                    StatusTextFärg = System.Windows.Media.Brushes.Red;
                    StatusMeddelande = "ISBN får endast innehålla siffror";
                    EnableLäggTillBok = false;
                    return;
                    }

                if (ISBN.Length != 10 && ISBN.Length != 13)
                    {
                    StatusTextFärg = System.Windows.Media.Brushes.Red;
                    StatusMeddelande = "ISBN måste vara mellan 10 eller 13 siffror";
                    EnableLäggTillBok = false;
                    return;
                    }

                if (_listISBN != null && _listISBN.Contains(ISBN))
                    {
                    StatusTextFärg = System.Windows.Media.Brushes.Red;
                    StatusMeddelande = "ISBN-nummer finns redan i butiken";
                    EnableLäggTillBok = false;
                    return;
                    }
                }

            if (!string.IsNullOrEmpty(Forfattare))
                {
                if (string.IsNullOrWhiteSpace(Forfattare))
                    {
                    StatusTextFärg = System.Windows.Media.Brushes.Red;
                    StatusMeddelande = "Författare får inte vara tom";
                    EnableLäggTillBok = false;
                    return;
                    }

                var namnsDelar = Forfattare.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (namnsDelar.Length < 2)
                    {
                    StatusTextFärg = System.Windows.Media.Brushes.Red;
                    StatusMeddelande = "Författare behöver förnamn och efternamn";
                    EnableLäggTillBok = false;
                    return;
                    }
                }
            
            if (!string.IsNullOrEmpty(Språk))
                {
                if (string.IsNullOrWhiteSpace(Språk) || Språk.Trim().Length < 2)
                    {
                    StatusTextFärg = System.Windows.Media.Brushes.Red;
                    StatusMeddelande = "Språk måste ha minst två bokstäver";
                    EnableLäggTillBok = false;
                    return;
                    }
                }
            if (!string.IsNullOrEmpty(BokAntal))
                {
                if (!int.TryParse(BokAntal, out var antal))
                    {
                    StatusTextFärg = System.Windows.Media.Brushes.Red;
                    StatusMeddelande = "Bok antal måste vara ett nummer";
                    EnableLäggTillBok = false;
                    return;
                    }

                if (antal <= 0)
                    {
                    StatusTextFärg = System.Windows.Media.Brushes.Red;
                    StatusMeddelande = "Bok antal måste vara större än 0";
                    EnableLäggTillBok = false;
                    return;
                    }
                }

            // I know .. i know..
            EnableLäggTillBok =
                !string.IsNullOrWhiteSpace(BokTitel) &&
                BokTitel.Trim().Length >= 1 &&
                !string.IsNullOrWhiteSpace(ISBN) &&
                ISBN.All(char.IsDigit) &&
                ( ISBN.Length == 10 || ISBN.Length == 13 ) &&  // ÄNDRA DENNA RAD
                !string.IsNullOrWhiteSpace(Språk) &&
                Språk.Trim().Length >= 2 &&
                !string.IsNullOrWhiteSpace(BokAntal) &&
                !string.IsNullOrWhiteSpace(Forfattare) &&
                Forfattare.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length >= 2 &&
                int.TryParse(BokAntal, out var validAntal) && validAntal > 0 &&
                ( _listISBN == null || !_listISBN.Contains(ISBN) ) &&
                ValdButik != null;


            if (EnableLäggTillBok &&
                !string.IsNullOrEmpty(BokTitel) &&
                !string.IsNullOrEmpty(ISBN) &&
                !string.IsNullOrEmpty(Forfattare) &&
                !string.IsNullOrEmpty(Språk) &&
                !string.IsNullOrEmpty(BokAntal))
                {
                StatusTextFärg = System.Windows.Media.Brushes.Yellow;
                StatusMeddelande = $"Redo att lägga {BokAntal} styck(en) {BokTitel} till  {ValdButik.ButiksNamn}";
                }
            }

        private void LäggTillBok()
            {
            try
                {
                var namnDelar = Forfattare.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string forNamn = namnDelar[0];
                string efterNamn = string.Join(" ", namnDelar.Skip(1));

                // Konvertera och formatera ISBN
                string formateratISBN = KonverteraISBN(ISBN);

                var nyBok = new Böcker
                    {
                    Titel = BokTitel,
                    Isbn = formateratISBN,
                    Språk = Språk.Trim()
                    };
                _context.Böckers.Add(nyBok);
                _context.SaveChanges();

                var bokFörfattare = new Författare
                    {
                    Förnamn = forNamn,
                    Efternamn = efterNamn
                    };
                _context.Författares.Add(bokFörfattare);
                _context.SaveChanges();

                var lagerSaldo = new LagerSaldo
                    {
                    Isbn = formateratISBN,
                    ButikId = ValdButik.ButikId,
                    Antal = int.Parse(BokAntal)
                    };
                _context.LagerSaldos.Add(lagerSaldo);
                _context.SaveChanges();
                _listISBN.Add(formateratISBN);



                // Rensa fält
                BokTitel = string.Empty;
                ISBN = string.Empty;
                Forfattare = string.Empty;
                Språk = string.Empty;
                BokAntal = string.Empty;
                StatusTextFärg = System.Windows.Media.Brushes.Green;
                StatusMeddelande = $"✓ Bok tillagd i butik {ValdButik.ButiksNamn}! ✓";
                
                }
            catch (Exception ex)
                {
                StatusTextFärg = System.Windows.Media.Brushes.Red;
                StatusMeddelande = $"✘ Internt serverfel ✘";

                string felmeddelande = $"Fel vid tillägg av bok:\n\n{ex.Message}";
                if (ex.InnerException != null)
                    {
                    felmeddelande += $"\n\nInner Exception:\n{ex.InnerException.Message}";
                    }

                MessageBox.Show(felmeddelande, "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        private void Avbryt()
            {
            Application.Current.Windows.OfType<Views.RedigeraBokView>().FirstOrDefault()?.Close();
            }
        /// <summary>
        /// Denna metod konverterar ett ISBN-10 nummer till ISBN-13 format. Eftersom jag inte läste på om ISBN ordentligt..
        /// Fick hjälp av AI för att få fram denna kodsnutt.
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string KonverteraISBN(string isbn)
            {
            isbn = isbn.Replace("-", "").Replace(" ", "").Trim();

            if (isbn.Length == 13)
                {
                return isbn; // Redan ISBN-13
                }
            else if (isbn.Length == 10)
                {
                // Konvertera ISBN-10 till ISBN-13
                string isbn13 = "978" + isbn.Substring(0, 9);

                // Beräkna ny checksiffra för ISBN-13
                int sum = 0;
                for (int i = 0; i < 12; i++)
                    {
                    int digit = int.Parse(isbn13[i].ToString());
                    sum += ( i % 2 == 0 ) ? digit : digit * 3;
                    }
                int checkDigit = ( 10 - ( sum % 10 ) ) % 10;

                return isbn13 + checkDigit;
                }
            else
                {
                throw new Exception($"Ogiltigt ISBN-format: {isbn.Length} siffror");
                }
            }
        }
    }