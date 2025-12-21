using Bokhandel_Labb.Commands;
using Bokhandel_Labb.DTOs;
using Bokhandel_Labb.Helpers;
using Bokhandel_Labb.Models;
using Bokhandel_Labb.Views;
using GongSolutions.Wpf.DragDrop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using static Bokhandel_Labb.Commands.Logger;
namespace Bokhandel_Labb.ViewModels
    {
    public class BokbyteViewModel : BaseViewModel
        {
        private readonly BokhandelContext _context;
        private bool _isUpdatingButiker = false;
        
        // Collections
        public ObservableCollection<ButikDTO> AllaButiker { get; set; }
        public ObservableCollection<LagerSaldoDTO> Butik1Böcker { get; set; }
        public ObservableCollection<LagerSaldoDTO> Butik2Böcker { get; set; }
        

        private LagerSaldoDTO _valdBok;
        public LagerSaldoDTO ValdBok
            {
            get => _valdBok;
            set
                {
                if (SetProperty(ref _valdBok, value))
                    {
                    if (value != null)
                        {
                        VisaInfo($"Bok: {value.Titel} från butik: {value.ButiksNamn} vald.");
                        }
                    }
                }
            }
        
        // Butiker
        private ObservableCollection<ButikDTO> _tillgängligaButiker1;
        public ObservableCollection<ButikDTO> TillgängligaButiker1
            {
            get => _tillgängligaButiker1;
            set => SetProperty(ref _tillgängligaButiker1, value);
            }

        private ObservableCollection<ButikDTO> _tillgängligaButiker2;
        public ObservableCollection<ButikDTO> TillgängligaButiker2
            {
            get => _tillgängligaButiker2;
            set => SetProperty(ref _tillgängligaButiker2, value);
            }

        private ButikDTO _valdButik1;
        public ButikDTO ValdButik1
            {
            get => _valdButik1;
            set
                {
                if (SetProperty(ref _valdButik1, value))
                    {
                    LaddaButik1Böcker();
                    UppdateraTillgängligaButiker();
                    }
                }
            }

        private ButikDTO _valdButik2;
        public ButikDTO ValdButik2
            {
            get => _valdButik2;
            set
                {
                if (SetProperty(ref _valdButik2, value))
                    {
                    LaddaButik2Böcker();
                    UppdateraTillgängligaButiker();
                    }
                }
            }

        // Statusmeddelande
        private string _statusMeddelande = "Läs hjälpavsnitt(F1) för användning av systemet.";
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

        // Sök

        private string _sökText;
        public string SökText
            {
            get => _sökText;
            set
                {
                if (SetProperty(ref _sökText, value))
                    {
                    LaddaButik1Böcker(_sökText);
                    LaddaButik2Böcker(_sökText);
                    }
                }
            }
        // Commands
        public ICommand SparaÄndringarCommand { get; }
        public ICommand ÅterställCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ÄndraLagersaldoCommand { get; }
        public ICommand HjälpCommand { get; }
        public IDropTarget DropHandler { get; }

        public BokbyteViewModel()
            {
            _context = new BokhandelContext();
            DropHandler = new BokDropHandler(this);

            // Initialisera collections
            AllaButiker = new ObservableCollection<ButikDTO>();
            Butik1Böcker = new ObservableCollection<LagerSaldoDTO>();
            Butik2Böcker = new ObservableCollection<LagerSaldoDTO>();
            TillgängligaButiker1 = new ObservableCollection<ButikDTO>();
            TillgängligaButiker2 = new ObservableCollection<ButikDTO>();

            // Gong dragndrop collection event
            Butik1Böcker.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Butik1Böcker));
            Butik2Böcker.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Butik2Böcker));

            // Initialisera commands
            SparaÄndringarCommand = new RelayCommand(SparaÄndringar);
            ÅterställCommand = new RelayCommand(Återställ);
            CancelCommand = new RelayCommand(Avbryt);
            ÄndraLagersaldoCommand = new RelayCommand(ÄndraLagersaldo);
            HjälpCommand = new RelayCommand(VisaHjälp);

            // Ladda data
            LaddaAllaButiker();
            }
        private void ÄndraLagersaldo()
            {
            if (ValdBok == null)
                {
                VisaVarning("Välj en bok först för att ändra lagersaldo");
                return;
                }

            var bokTitel = ValdBok.Titel;
            var antalILager = ValdBok.AntalILager;

            var tillhörButik1 = Butik1Böcker.Contains(ValdBok);
            var sourceCollection = tillhörButik1 ? Butik1Böcker : Butik2Böcker;
            var sourceButik = tillhörButik1 ? ValdButik1 : ValdButik2;

            var dialog = new BokbyteÄndraLagerBokDialog(
                bokTitel,
                antalILager);

            dialog.Owner = Application.Current.Windows.OfType<Views.BokbyteView>().FirstOrDefault();

            if (dialog.ShowDialog() == true)
                {
                if (dialog.Antal == 0)
                    {
                    // Ta bort från collection om antal är 0
                    sourceCollection.Remove(ValdBok);
                    VisaVarning($"⚠ '{bokTitel}' har satts till 0 och kommer tas bort från {sourceButik.ButiksNamn}. Klicka 'Spara Ändringar' för att verkställa.");
                    }
                else
                    {
                    ValdBok.AntalILager = dialog.Antal;
                    VisaSucces($"✓ Lagersaldo för '{bokTitel}' uppdaterat till {dialog.Antal}");
                    }
                }
            }

        private void VisaHjälp()
            {
            MessageBox.Show(
                "Inventering - Hjälp\n\n" +
                "• Dra böcker mellan butiker för att flytta lagersaldo\n" +
                "• Dra böcker till papperskorgen för att ta bort från butik\n" +
                "• Dubbelklicka en bok eller tryck Alt + E för att ändra lagersaldo\n" +
                "• Klicka 'Spara Ändringar' för att verkställa ändringar\n" +
                "• Klicka 'Återställ' för att ångra osparade ändringar\n\n" +
                "Tangentbordsgenvägar:\n" +
                "• Alt + S - Spara ändringar\n" +
                "• Alt + Z - Återställ\n" +
                "• Alt + E - Ändra lagersaldo\n" +
                "• F1 - Visa hjälp\n" +
                "• Alt + Q - Avsluta",
                "Hjälp",
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
            }
        private void LaddaAllaButiker()
            {
            try
                {
                var butiker = _context.Butikers.ToList();

                AllaButiker.Clear();
                foreach (var butik in butiker)
                    {
                    AllaButiker.Add(new ButikDTO
                        {
                        ButikId = butik.Id,
                        ButiksNamn = butik.Butiksnamn,
                        Adress = butik.Adress
                        });
                    }
                UppdateraTillgängligaButiker();

                if (TillgängligaButiker1.Count >= 1)
                    ValdButik1 = TillgängligaButiker1[0];
                if (TillgängligaButiker2.Count >= 1)
                    ValdButik2 = TillgängligaButiker2[0];
                }
            catch (Exception ex)
                {
                VisaFel($"Fel vid laddning av butiker: {ex.Message}");
                }
            }

        private void LaddaButik1Böcker(string sökText = "")
            {
            if (ValdButik1 == null)
                return;
            try
                {
                if (string.IsNullOrWhiteSpace(sökText))
                    {
                    var lagerSaldo = _context.LagerSaldos
                        .Where(ls => ls.ButikId == ValdButik1.ButikId)
                        .Include(ls => ls.IsbnNavigation)
                            .ThenInclude(b => b.Författares)
                        .ToList();

                    Butik1Böcker.Clear();
                    foreach (var saldo in lagerSaldo)
                        {
                        var författare = string.Join(", ",
                            saldo.IsbnNavigation.Författares.Select(f => f.Förnamn + " " + f.Efternamn));

                        Butik1Böcker.Add(new LagerSaldoDTO
                            {
                            Isbn = saldo.Isbn,
                            Titel = saldo.IsbnNavigation.Titel,
                            FörfattareNamn = författare,
                            AntalILager = saldo.Antal,
                            ButikId = ValdButik1.ButikId,
                            });
                        }
                    }
                else
                    {
                    var lagerSaldo = _context.LagerSaldos
                        .Where(ls => ls.ButikId == ValdButik1.ButikId)
                        .Include(ls => ls.IsbnNavigation)
                            .ThenInclude(b => b.Författares)
                        .ToList();

                    Butik1Böcker.Clear();
                    foreach (var saldo in lagerSaldo)
                        {
                        var författare = string.Join(", ",
                            saldo.IsbnNavigation.Författares.Select(f => f.Förnamn + " " + f.Efternamn));
                        if (saldo.IsbnNavigation.Titel.Contains(sökText, StringComparison.OrdinalIgnoreCase) ||
                            författare.Contains(sökText, StringComparison.OrdinalIgnoreCase))
                            {
                            Butik1Böcker.Add(new LagerSaldoDTO
                                {
                                Isbn = saldo.Isbn,
                                Titel = saldo.IsbnNavigation.Titel,
                                FörfattareNamn = författare,
                                AntalILager = saldo.Antal,
                                ButikId = ValdButik1.ButikId,
                                });
                            }
                        }
                    }
                }
            catch (Exception ex)
                {
                VisaFel($"Fel vid laddning av böcker från {ValdButik1.ButiksNamn}: {ex.Message}");
                }
            }

        private void LaddaButik2Böcker(string sökText = "")
            {
            if (ValdButik2 == null)
                return;
                try
                {
                if (string.IsNullOrWhiteSpace(sökText))
                    {
                    var lagerSaldo = _context.LagerSaldos
                        .Where(ls => ls.ButikId == ValdButik2.ButikId)
                        .Include(ls => ls.IsbnNavigation)
                            .ThenInclude(b => b.Författares)
                        .ToList();

                    Butik2Böcker.Clear();
                    foreach (var saldo in lagerSaldo)
                        {
                        var författare = string.Join(", ",
                            saldo.IsbnNavigation.Författares.Select(f => f.Förnamn + " " + f.Efternamn));

                        Butik2Böcker.Add(new LagerSaldoDTO
                            {
                            Isbn = saldo.Isbn,
                            Titel = saldo.IsbnNavigation.Titel,
                            FörfattareNamn = författare,
                            AntalILager = saldo.Antal,
                            ButikId = ValdButik2.ButikId,
                            });
                        }
                    }
                else
                    {
                    var lagerSaldo = _context.LagerSaldos
                        .Where(ls => ls.ButikId == ValdButik2.ButikId)
                        .Include(ls => ls.IsbnNavigation)
                            .ThenInclude(b => b.Författares)
                        .ToList();

                    Butik2Böcker.Clear();
                    foreach (var saldo in lagerSaldo)
                        {
                        var författare = string.Join(", ",
                            saldo.IsbnNavigation.Författares.Select(f => f.Förnamn + " " + f.Efternamn));
                        if (saldo.IsbnNavigation.Titel.Contains(sökText, StringComparison.OrdinalIgnoreCase) ||
                            författare.Contains(sökText, StringComparison.OrdinalIgnoreCase))
                            {
                            Butik2Böcker.Add(new LagerSaldoDTO
                                {
                                Isbn = saldo.Isbn,
                                Titel = saldo.IsbnNavigation.Titel,
                                FörfattareNamn = författare,
                                AntalILager = saldo.Antal,
                                ButikId = ValdButik2.ButikId,
                                });
                            }
                        }
                    }
                }
            catch ( Exception ex)
                {
                VisaFel($"Fel vid laddning av böcker från {ValdButik2.ButiksNamn}: {ex.Message}");
                }
            }

        private void SparaÄndringar()
            {
            try
                {
                // Hämta ALLA ISBN från båda butikerna i databasen
                var allaISBNButik1 = _context.LagerSaldos
                    .Where(ls => ls.ButikId == ValdButik1.ButikId)
                    .Select(ls => ls.Isbn)
                    .ToList();

                var allaISBNButik2 = _context.LagerSaldos
                    .Where(ls => ls.ButikId == ValdButik2.ButikId)
                    .Select(ls => ls.Isbn)
                    .ToList();

                // Uppdatera LagerSaldo för Butik 1
                foreach (var bok in Butik1Böcker)
                    {
                    var saldo = _context.LagerSaldos
                        .FirstOrDefault(ls => ls.ButikId == ValdButik1.ButikId && ls.Isbn == bok.Isbn);

                    if (saldo != null)
                        {
                        if (saldo.Antal != bok.AntalILager)
                            {
                            int skillnad = bok.AntalILager - saldo.Antal;
                            saldo.Antal = bok.AntalILager;

                            LoggaHändelse(_context, "Admin", ValdButik1.ButiksNamn, ValdButik1.ButikId,
                                $"'{bok.Titel}' lagersaldo ändrat: {saldo.Antal + skillnad} > {bok.AntalILager} ({skillnad:+0;-#})", "✏️");
                            }
                        }
                    else if (bok.AntalILager > 0)
                        {
                        _context.LagerSaldos.Add(new LagerSaldo
                            {
                            ButikId = ValdButik1.ButikId,
                            Isbn = bok.Isbn,
                            Antal = bok.AntalILager
                            });

                            LoggaHändelse(_context, "Admin", ValdButik1.ButiksNamn, ValdButik1.ButikId,
                            $"'{bok.Titel}' tillagd i lagret ({bok.AntalILager} st)", "➕");
                        }
                    }

                // Ta bort böcker från Butik 1 som finns i DB men inte i collection
                var borttagnaBöckerButik1 = allaISBNButik1
                    .Where(isbn => !Butik1Böcker.Any(b => b.Isbn == isbn))
                    .ToList();

                foreach (var isbn in borttagnaBöckerButik1)
                    {
                    var saldo = _context.LagerSaldos
                        .FirstOrDefault(ls => ls.ButikId == ValdButik1.ButikId && ls.Isbn == isbn);
                    if (saldo != null)
                        {
                        // Hämta boktitel för loggning
                        var bokInfo = _context.Böckers.FirstOrDefault(b => b.Isbn == isbn);
                        string bokTitel = bokInfo?.Titel ?? isbn;

                        _context.LagerSaldos.Remove(saldo);

                        LoggaHändelse(_context, "Admin", ValdButik1.ButiksNamn, ValdButik1.ButikId,
                            $"'{bokTitel}' borttagen från lagret", "🗑️");
                        }
                    }

                // Uppdatera LagerSaldo för Butik 2
                foreach (var bok in Butik2Böcker)
                    {
                    var saldo = _context.LagerSaldos
                        .FirstOrDefault(ls => ls.ButikId == ValdButik2.ButikId && ls.Isbn == bok.Isbn);

                    if (saldo != null)
                        {
                        if (saldo.Antal != bok.AntalILager)
                            {
                            int skillnad = bok.AntalILager - saldo.Antal;
                            saldo.Antal = bok.AntalILager;

                            LoggaHändelse(_context, "Admin", ValdButik2.ButiksNamn, ValdButik2.ButikId,
                                $"'{bok.Titel}' lagersaldo ändrat: {saldo.Antal + skillnad} > {bok.AntalILager} ({skillnad:+0;-#})", "✏️");
                            }
                        }
                    else if (bok.AntalILager > 0)
                        {
                        _context.LagerSaldos.Add(new LagerSaldo
                            {
                            ButikId = ValdButik2.ButikId,
                            Isbn = bok.Isbn,
                            Antal = bok.AntalILager
                            });

                        LoggaHändelse(_context, "Admin", ValdButik2.ButiksNamn, ValdButik2.ButikId,
                            $"'{bok.Titel}' tillagd i lagret ({bok.AntalILager} st)", "➕");
                        }
                    }

                // Ta bort böcker från Butik 2 som finns i DB men inte i collection
                var borttagnaBöckerButik2 = allaISBNButik2
                    .Where(isbn => !Butik2Böcker.Any(b => b.Isbn == isbn))
                    .ToList();

                foreach (var isbn in borttagnaBöckerButik2)
                    {
                    var saldo = _context.LagerSaldos
                        .FirstOrDefault(ls => ls.ButikId == ValdButik2.ButikId && ls.Isbn == isbn);
                    if (saldo != null)
                        {
                        // Hämta boktitel för loggning
                        var bokInfo = _context.Böckers.FirstOrDefault(b => b.Isbn == isbn);
                        string bokTitel = bokInfo?.Titel ?? isbn;

                        _context.LagerSaldos.Remove(saldo);

                        LoggaHändelse(_context, "Admin", ValdButik2.ButiksNamn, ValdButik2.ButikId,
                            $"'{bokTitel}' borttagen från lagret", "🗑️");
                        }
                    }

                // Tar bort LagerSaldo med 0 böcker
                var tomtLager = _context.LagerSaldos
                    .Where(ls => ls.Antal <= 0 &&
                        ( ls.ButikId == ValdButik1.ButikId || ls.ButikId == ValdButik2.ButikId ))
                    .ToList();

                _context.LagerSaldos.RemoveRange(tomtLager);

                // Sparar till databas
                _context.SaveChanges();

                VisaSucces("✓ Ändringar sparade!");

                // Ladda om för att visa korrekt data
                LaddaButik1Böcker();
                LaddaButik2Böcker();
                }
            catch (Exception ex)
                {
                VisaFel($"✘ Fel vid sparande: {ex.InnerException?.Message}");
                }
            }

        private void UppdateraTillgängligaButiker()
            {
            if (_isUpdatingButiker)
                return;

            _isUpdatingButiker = true;

            TillgängligaButiker1 = new ObservableCollection<ButikDTO>(
                AllaButiker.Where(b => ValdButik2 == null || b.ButikId != ValdButik2.ButikId));

            TillgängligaButiker2 = new ObservableCollection<ButikDTO>(
                AllaButiker.Where(b => ValdButik1 == null || b.ButikId != ValdButik1.ButikId));

            _isUpdatingButiker = false;
            }

        private void Återställ()
            {
            LaddaButik1Böcker();
            LaddaButik2Böcker();
            VisaInfo("Ändringar återställda");
            }

        private void Avbryt()
            {
            Application.Current.Windows.OfType<Views.BokbyteView>().FirstOrDefault()?.Close();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Focus();
            }

        // Helper metoder för statusmeddelanden
        public void VisaSucces(string meddelande)
            {
            StatusTextFärg = System.Windows.Media.Brushes.Green;
            StatusMeddelande = meddelande;
            }

        public void VisaFel(string meddelande)
            {
            StatusTextFärg = System.Windows.Media.Brushes.Red;
            StatusMeddelande = meddelande;
            }

        public void VisaInfo(string meddelande)
            {
            StatusTextFärg = System.Windows.Media.Brushes.Black;
            StatusMeddelande = meddelande;
            }

        public void VisaVarning(string meddelande)
            {
            StatusTextFärg = System.Windows.Media.Brushes.Orange;
            StatusMeddelande = meddelande;
            }
        }
    }