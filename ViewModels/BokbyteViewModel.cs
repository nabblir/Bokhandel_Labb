using Bokhandel_Labb.Commands;
using Bokhandel_Labb.Helpers;
using Bokhandel_Labb.Models;
using GongSolutions.Wpf.DragDrop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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

        // Commands
        public ICommand SparaÄndringarCommand { get; }
        public ICommand ÅterställCommand { get; }
        public ICommand CancelCommand { get; }
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

            // Ladda data
            LaddaAllaButiker();
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
                MessageBox.Show($"Fel vid laddning av butiker: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        private void LaddaButik1Böcker()
            {
            if (ValdButik1 == null)
                return;
            try
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
            catch (Exception ex)
                {
                MessageBox.Show($"Fel vid laddning av böcker för {ValdButik1.ButiksNamn}: {ex.Message}",
                    "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        private void LaddaButik2Böcker()
            {
            if (ValdButik2 == null)
                return;
            try
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
            catch (Exception ex)
                {
                MessageBox.Show($"Fel vid laddning av böcker för {ValdButik2.ButiksNamn}: {ex.Message}",
                    "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        private void SparaÄndringar()
            {
            try
                {
                // Uppdatera LagerSaldo för Butik 1
                foreach (var bok in Butik1Böcker)
                    {
                    var saldo = _context.LagerSaldos
                        .FirstOrDefault(ls => ls.ButikId == ValdButik1.ButikId && ls.Isbn == bok.Isbn);

                    if (saldo != null)
                        {
                        saldo.Antal = bok.AntalILager;
                        }
                    else if (bok.AntalILager > 0)
                        {
                        _context.LagerSaldos.Add(new LagerSaldo
                            {
                            ButikId = ValdButik1.ButikId,
                            Isbn = bok.Isbn,
                            Antal = bok.AntalILager
                            });
                        }
                    }

                // Uppdatera LagerSaldo för Butik 2
                foreach (var bok in Butik2Böcker)
                    {
                    var saldo = _context.LagerSaldos
                        .FirstOrDefault(ls => ls.ButikId == ValdButik2.ButikId && ls.Isbn == bok.Isbn);

                    if (saldo != null)
                        {
                        saldo.Antal = bok.AntalILager;
                        }
                    else if (bok.AntalILager > 0)
                        {
                        _context.LagerSaldos.Add(new LagerSaldo
                            {
                            ButikId = ValdButik2.ButikId,
                            Isbn = bok.Isbn,
                            Antal = bok.AntalILager
                            });
                        }
                    }

                // Tar bort LagerSaldo med 0 böcker
                var tomtLager = _context.LagerSaldos
                    .Where(ls => ls.Antal == 0 &&
                        ( ls.ButikId == ValdButik1.ButikId || ls.ButikId == ValdButik2.ButikId ))
                    .ToList();

                _context.LagerSaldos.RemoveRange(tomtLager);

                // Sparar till databas
                _context.SaveChanges();

                MessageBox.Show("Ändringar sparade!", "Sparat", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            catch (Exception ex)
                {
                MessageBox.Show($"Fel vid sparande: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
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
            }

        private void Avbryt()
            {
            Application.Current.Windows.OfType<Views.BokbyteView>().FirstOrDefault()?.Close();
            }
        }

    // DTOs
    public class ButikDTO : BaseViewModel
        {
        private int _butikId;
        private string _butiksNamn;
        private string _adress;

        public int ButikId
            {
            get => _butikId;
            set => SetProperty(ref _butikId, value);
            }

        public string ButiksNamn
            {
            get => _butiksNamn;
            set => SetProperty(ref _butiksNamn, value);
            }

        public string Adress
            {
            get => _adress;
            set => SetProperty(ref _adress, value);
            }
        }

    public class LagerSaldoDTO : BaseViewModel
        {
        private string _isbn;
        private string _titel;
        private string _författareNamn;
        private int _antalILager;
        private int _butikId;

        public string Isbn
            {
            get => _isbn;
            set => SetProperty(ref _isbn, value);
            }

        public string Titel
            {
            get => _titel;
            set => SetProperty(ref _titel, value);
            }

        public string FörfattareNamn
            {
            get => _författareNamn;
            set => SetProperty(ref _författareNamn, value);
            }

        public int AntalILager
            {
            get => _antalILager;
            set => SetProperty(ref _antalILager, value);
            }

        public int ButikId
            {
            get => _butikId;
            set => SetProperty(ref _butikId, value);
            }
        }
    }