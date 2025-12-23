using Bokhandel_Labb.Commands;
using Bokhandel_Labb.DTO;
using Bokhandel_Labb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Bokhandel_Labb.Commands.Logger;

namespace Bokhandel_Labb.ViewModels
    {
    public class RedigeraBokViewModel : BaseViewModel
        {
        private readonly BokhandelContext _context;
        private List<string> _listISBN;
        private CancellationTokenSource _validationCts;

        public bool VisaNyFörfattareFält => ValdFörfattare != null && ValdFörfattare.FörfattarId == -1;
        public bool VisaNyFörlagFält => ValtFörlag != null && ValtFörlag.FörlagId == -1;

        // Properties
        private string _bokTitel;
        public string BokTitel
            {
            get => _bokTitel;
            set
                {
                if (SetProperty(ref _bokTitel, value))
                    {
                    AsyncValidation();
                    }
                }
            }

        private string _pris;
        public string Pris
            {
            get => _pris;
            set
                {
                if (SetProperty(ref _pris, value))
                    {
                    AsyncValidation();
                    }
                }
            }

        private string _antalSidor;
        public string AntalSidor
            {
            get => _antalSidor;
            set
                {
                if (SetProperty(ref _antalSidor, value))
                    {
                    AsyncValidation();
                    }
                }
            }

        private DateTime _datum = DateTime.Now;
        public DateTime Utgivningsdatum
            {
            get => _datum;
            set
                {
                if (SetProperty(ref _datum, value))
                    {
                    AsyncValidation();
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
                    AsyncValidation();
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
                    AsyncValidation();
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
                    AsyncValidation();
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
                    AsyncValidation();
                    }
                }
            }

        public ObservableCollection<FörfattareDTO> FörfattareLista { get; set; }

        private FörfattareDTO _valdFörfattare;
        public FörfattareDTO ValdFörfattare
            {
            get => _valdFörfattare;
            set
                {
                if (SetProperty(ref _valdFörfattare, value))
                    {
                    OnPropertyChanged(nameof(VisaNyFörfattareFält));
                    AsyncValidation();
                    }
                }
            }

        private string _nyFörfattareNamn;
        public string NyFörfattareNamn
            {
            get => _nyFörfattareNamn;
            set
                {
                if (SetProperty(ref _nyFörfattareNamn, value))
                    {
                    AsyncValidation();
                    }
                }
            }

        public ObservableCollection<FörlagDTO> FörlagLista { get; set; }

        private FörlagDTO _valtFörlag;
        public FörlagDTO ValtFörlag
            {
            get => _valtFörlag;
            set
                {
                if (SetProperty(ref _valtFörlag, value))
                    {
                    OnPropertyChanged(nameof(VisaNyFörlagFält));
                    AsyncValidation();
                    }
                }
            }

        private string _nyttFörlagNamn;
        public string NyttFörlagNamn
            {
            get => _nyttFörlagNamn;
            set
                {
                if (SetProperty(ref _nyttFörlagNamn, value))
                    {
                    AsyncValidation();
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
            FörfattareLista = new ObservableCollection<FörfattareDTO>();
            FörlagLista = new ObservableCollection<FörlagDTO>();

            OKCommand = new RelayCommand(LäggTillBok);
            CancelCommand = new RelayCommand(Avbryt);
           
            Utgivningsdatum = DateTime.Now;
            LaddaButiker();
            LaddaFörfattare();
            LaddaFörlag();
            }

        private void LaddaFörfattare()
            {
            try
                {
                var författare = _context.Författares.ToList();
                FörfattareLista.Clear();

                FörfattareLista.Add(new FörfattareDTO
                    {
                    FörfattarId = -1,
                    Förnamn = "➕ Lägg till",
                    Efternamn = "ny författare"
                    });

                foreach (var f in författare)
                    {
                    FörfattareLista.Add(new FörfattareDTO
                        {
                        FörfattarId = f.Id,
                        Förnamn = f.Förnamn,
                        Efternamn = f.Efternamn
                        });
                    }

                if (FörfattareLista.Count > 1)
                    ValdFörfattare = FörfattareLista[1];
                }
            catch (Exception ex)
                {
                MessageBox.Show($"Fel vid laddning av författare: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        private void LaddaFörlag()
            {
            try
                {
                var förlag = _context.Förlags.ToList();
                FörlagLista.Clear();

                FörlagLista.Add(new FörlagDTO
                    {
                    FörlagId = -1,
                    Namn = "➕ Lägg till nytt förlag"
                    });

                FörlagLista.Add(new FörlagDTO
                    {
                    FörlagId = 0,
                    Namn = "(Inget förlag)"
                    });

                foreach (var f in förlag)
                    {
                    FörlagLista.Add(new FörlagDTO
                        {
                        FörlagId = f.Id,
                        Namn = f.Namn,
                        Land = f.Land,
                        Webbplats = f.Webbplats
                        });
                    }

                if (FörlagLista.Count > 1)
                    ValtFörlag = FörlagLista[1];
                }
            catch (Exception ex)
                {
                MessageBox.Show($"Fel vid laddning av förlag: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

        private async void AsyncValidation()
            {
            _validationCts?.Cancel();
            _validationCts = new CancellationTokenSource();

            try
                {
                await Task.Delay(500, _validationCts.Token);
                ValideraInputs();
                }
            catch (TaskCanceledException)
                {
                }
            }

        #region VALIDERING
        private void ValideraInputs()
            {
            StatusMeddelande = string.Empty;
            StatusTextFärg = System.Windows.Media.Brushes.Black;
            EnableLäggTillBok = false;

            if (!ValideraBokTitel())
                return;
            if (!ValideraISBN())
                return;
            if (!ValideraSpråk())
                return;
            if (!ValideraBokAntal())
                return;
            if (!ValideraPris())
                return;
            if (!ValideraAntalSidor())
                return;
            if (!ValideraNyFörfattare())
                return;
            if (!ValideraNyttFörlag())
                return;

            if (AllaFältIfyllda())
                {
                EnableLäggTillBok = true;
                StatusTextFärg = System.Windows.Media.Brushes.DarkSeaGreen;
                StatusMeddelande = $"✓ Redo att lägga till {BokAntal} st {BokTitel} i {ValdButik?.ButiksNamn}";
                }
            }

        private bool ValideraBokTitel()
            {
            if (string.IsNullOrEmpty(BokTitel))
                return true;

            if (string.IsNullOrWhiteSpace(BokTitel))
                {
                VisaFel("✘ Boktitel får inte vara tom");
                return false;
                }

            return true;
            }

        private bool ValideraISBN()
            {
            if (string.IsNullOrEmpty(ISBN))
                return true;

            if (string.IsNullOrWhiteSpace(ISBN))
                {
                VisaFel("✘ ISBN får inte vara tom");
                return false;
                }

            if (!ISBN.All(char.IsDigit))
                {
                VisaFel("✘ ISBN får endast innehålla siffror");
                return false;
                }

            if (ISBN.Length != 10 && ISBN.Length != 13)
                {
                VisaFel("✘ ISBN måste vara 10 eller 13 siffror");
                return false;
                }

            if (_listISBN != null && _listISBN.Contains(ISBN))
                {
                VisaFel("✘ ISBN finns redan i butiken");
                return false;
                }

            return true;
            }

        private bool ValideraSpråk()
            {
            if (string.IsNullOrEmpty(Språk))
                return true;

            if (string.IsNullOrWhiteSpace(Språk) || Språk.Trim().Length < 2)
                {
                VisaFel("✘ Språk måste ha minst 2 bokstäver");
                return false;
                }

            return true;
            }

        private bool ValideraBokAntal()
            {
            if (string.IsNullOrEmpty(BokAntal))
                return true;

            if (!int.TryParse(BokAntal, out var antal))
                {
                VisaFel("✘ Antal måste vara ett nummer");
                return false;
                }

            if (antal <= 0)
                {
                VisaFel("✘ Antal måste vara större än 0");
                return false;
                }

            return true;
            }

        private bool ValideraPris()
            {
            if (string.IsNullOrEmpty(Pris))
                return true;

            if (!decimal.TryParse(Pris, out var pris))
                {
                VisaFel("✘ Pris måste vara ett nummer");
                return false;
                }

            if (pris < 0)
                {
                VisaFel("✘ Pris kan inte vara negativt");
                return false;
                }

            return true;
            }

        private bool ValideraAntalSidor()
            {
            if (string.IsNullOrEmpty(AntalSidor))
                return true;

            if (!int.TryParse(AntalSidor, out var sidor))
                {
                VisaFel("✘ Antal sidor måste vara ett nummer");
                return false;
                }

            if (sidor <= 0)
                {
                VisaFel("✘ Antal sidor måste vara större än 0");
                return false;
                }

            return true;
            }

        private bool ValideraNyFörfattare()
            {
            if (ValdFörfattare == null || ValdFörfattare.FörfattarId != -1)
                return true;

            if (string.IsNullOrWhiteSpace(NyFörfattareNamn))
                {
                VisaFel("✘ Ange namn på ny författare");
                return false;
                }

            var namnDelar = NyFörfattareNamn.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (namnDelar.Length < 2)
                {
                VisaFel("✘ Ange både förnamn och efternamn");
                return false;
                }

            return true;
            }

        private bool ValideraNyttFörlag()
            {
            if (ValtFörlag == null || ValtFörlag.FörlagId != -1)
                return true;

            if (string.IsNullOrWhiteSpace(NyttFörlagNamn))
                {
                VisaFel("✘ Ange namn på nytt förlag");
                return false;
                }

            return true;
            }

        private bool AllaFältIfyllda()
            {
            bool författareOK = ( ValdFörfattare != null && ValdFörfattare.FörfattarId > 0 ) ||
                               ( ValdFörfattare != null && ValdFörfattare.FörfattarId == -1 && !string.IsNullOrWhiteSpace(NyFörfattareNamn) );

            return !string.IsNullOrWhiteSpace(BokTitel) &&
                   !string.IsNullOrWhiteSpace(ISBN) &&
                   författareOK &&
                   !string.IsNullOrWhiteSpace(Språk) &&
                   !string.IsNullOrWhiteSpace(BokAntal) &&
                   ValdButik != null;
            }

        private void VisaFel(string meddelande)
            {
            StatusTextFärg = System.Windows.Media.Brushes.Red;
            StatusMeddelande = meddelande;
            EnableLäggTillBok = false;
            }
        #endregion

        private void LäggTillBok()
            {
            try
                {
                string formateratISBN = KonverteraISBN(ISBN);


                int författareId;
                if (ValdFörfattare.FörfattarId == -1)
                    {

                    var namnDelar = NyFörfattareNamn.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    string forNamn = namnDelar[0];
                    string efterNamn = string.Join(" ", namnDelar.Skip(1));

                    var nyFörfattare = new Författare
                        {
                        Förnamn = forNamn,
                        Efternamn = efterNamn
                        };
                    _context.Författares.Add(nyFörfattare);
                    _context.SaveChanges();
                    författareId = nyFörfattare.Id;
                    }
                else
                    {
                    författareId = ValdFörfattare.FörfattarId;
                    }


                int? förlagId = null;
                if (ValtFörlag != null)
                    {
                    if (ValtFörlag.FörlagId == -1)
                        {

                        var nyttFörlag = new Förlag
                            {
                            Namn = NyttFörlagNamn.Trim()
                            };
                        _context.Förlags.Add(nyttFörlag);
                        _context.SaveChanges();
                        förlagId = nyttFörlag.Id;
                        }
                    else if (ValtFörlag.FörlagId > 0)
                        {
                        förlagId = ValtFörlag.FörlagId;
                        }
                    }


                var nyBok = new Böcker
                    {
                    Titel = BokTitel,
                    Isbn = formateratISBN,
                    Språk = Språk.Trim(),
                    Utgivningsdatum = DateOnly.FromDateTime(Utgivningsdatum),
                    FörlagId = förlagId
                    };


                if (!string.IsNullOrWhiteSpace(Pris) && decimal.TryParse(Pris, out var pris))
                    {
                    nyBok.Pris = pris;
                    }

                if (!string.IsNullOrWhiteSpace(AntalSidor) && int.TryParse(AntalSidor, out var sidor))
                    {
                    nyBok.Sidantal = sidor;
                    }

                _context.Böckers.Add(nyBok);
                _context.SaveChanges();

                // Rawdog SQL för att lägga till i BokFörfattare (måste göras separat pga många-till-många relation)
                _context.Database.ExecuteSqlRaw(
                    "INSERT INTO BokFörfattare (ISBN, FörfattareID) VALUES ({0}, {1})",
                    formateratISBN, författareId);

                var lagerSaldo = new LagerSaldo
                    {
                    Isbn = formateratISBN,
                    ButikId = ValdButik.ButikId,
                    Antal = int.Parse(BokAntal)
                    };
                _context.LagerSaldos.Add(lagerSaldo);
                _context.SaveChanges();

                _listISBN.Add(formateratISBN);
                LoggaHändelse(_context, "Admin", 
                    _valdButik.ButiksNamn,
                    _valdButik.ButikId,
                    $"{BokAntal} st av {BokTitel} tillagd(a) i lagret",
                    "➕");

                BokTitel = string.Empty;
                ISBN = string.Empty;
                NyFörfattareNamn = string.Empty;
                NyttFörlagNamn = string.Empty;
                Språk = string.Empty;
                BokAntal = string.Empty;
                Pris = string.Empty;
                AntalSidor = string.Empty;
                Utgivningsdatum = DateTime.Now;


                if (FörfattareLista.Count > 1)
                    ValdFörfattare = FörfattareLista[1];
                if (FörlagLista.Count > 1)
                    ValtFörlag = FörlagLista[1];

                StatusTextFärg = System.Windows.Media.Brushes.Green;
                StatusMeddelande = $"✓ Bok tillagd i {ValdButik.ButiksNamn}! ✓";

                // Ladda om listor för att inkludera nya författare/förlag
                LaddaFörfattare();
                LaddaFörlag();
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

        private string KonverteraISBN(string isbn)
            {
            isbn = isbn.Replace("-", "").Replace(" ", "").Trim();

            if (isbn.Length == 13)
                {
                return isbn;
                }
            else if (isbn.Length == 10)
                {
                string isbn13 = "978" + isbn.Substring(0, 9);

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