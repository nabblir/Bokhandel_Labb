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


namespace Bokhandel_Labb.ViewModels
    {
    public class HanteraOrderViewModel : BaseViewModel
        {
        private readonly BokhandelContext _context;
        public ObservableCollection<OrderHistorikDTO> AllaOrdrar { get; set; }

        private OrderHistorikDTO _valdOrder;
        public OrderHistorikDTO ValdOrder
            {
            get => _valdOrder;
            set
                {
                if (SetProperty(ref _valdOrder, value))
                    {
                    if (value != null)
                        {
                        VisaOrderDetaljer(value);
                        }
                    }
                }
            }
        private System.Windows.Media.Brush _leveransFärg = System.Windows.Media.Brushes.White;
        public System.Windows.Media.Brush LeveransFärg
            {
            get => _leveransFärg;
            set => SetProperty(ref _leveransFärg, value);
            }
        private string _statusMeddelande = "Klicka på en order för att få mer information.";
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

        public ICommand CancelCommand { get; }

        private ObservableCollection<ButikDTO> _tillgängligaButiker1;
        public ObservableCollection<ButikDTO> TillgängligaButiker1
            {
            get => _tillgängligaButiker1;
            set => SetProperty(ref _tillgängligaButiker1, value);
            }

        private ButikDTO _valdButik1;
        public ButikDTO ValdButik1
            {
            get => _valdButik1;
            set
                {
                if (SetProperty(ref _valdButik1, value))
                    {
                    HämtaOrder(ValdButik1.ButikId);
                    }
                }
            }

        public HanteraOrderViewModel()
            {
            _context = new BokhandelContext();
            AllaOrdrar = new ObservableCollection<OrderHistorikDTO>();
            TillgängligaButiker1 = new ObservableCollection<ButikDTO>();
            HämtaButiker();

            CancelCommand = new RelayCommand(Avbryt);
            }

        private void HämtaButiker()
            {
            TillgängligaButiker1.Clear();
            TillgängligaButiker1.Add(new ButikDTO
                {
                ButikId = -1,
                ButiksNamn = "Alla butiker"
                });

            var butiker = _context.Butikers
                .Select(b => new ButikDTO
                    {
                    ButikId = b.Id,
                    ButiksNamn = b.Butiksnamn,
                    Adress = b.Adress,
                    Stad = b.Stad
                    })
                .ToList();

            foreach (var butik in butiker)
                {
                TillgängligaButiker1.Add(butik);
                }

            if (TillgängligaButiker1.Count > 0)
                {
                ValdButik1 = TillgängligaButiker1[0];
                }
            }

        private void VisaOrderDetaljer(OrderHistorikDTO order)
            {
            StatusTextFärg = System.Windows.Media.Brushes.DarkBlue;

            var detaljer = $"📋 Order #{order.Id}\n" +
                          $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                          $"🏪 Butik: {order.ButiksNamn}\n" +
                          $"📦 Status: {order.Status}\n" +
                          $"Dubbelklicka för mer information\n" +
                          $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n";

            StatusMeddelande = detaljer;
            }

        public void VisaInfo(string meddelande)
            {
            StatusTextFärg = System.Windows.Media.Brushes.Black;
            StatusMeddelande = meddelande;
            }

        private void HämtaOrder(int butikID)
            {
            AllaOrdrar.Clear();
            if (butikID == -1)
                {
                HämtaAllaOrdrar();
                return;
                }

            var ordrar = _context.Ordrars
                .Include(o => o.OrderRaders)
                    .ThenInclude(or => or.IsbnNavigation)
                .Include(o => o.Kund)
                .Where(o => o.ButikId == butikID)
                .Select(o => new OrderHistorikDTO
                    {
                    Id = o.Id,
                    Orderdatum = o.Orderdatum,
                    Status = o.Status,
                    ButikID = o.ButikId,
                    TotalBelopp = o.TotalBelopp,
                    KundID = o.KundId,
                    AntalBöcker = o.OrderRaders.Sum(r => r.Antal),
                    OrderRader = o.OrderRaders.Select(r => new OrderRadDTO
                        {
                        BokTitel = r.IsbnNavigation.Titel,
                        Antal = r.Antal,
                        Pris = r.Pris,
                        Totalt = r.Antal * r.Pris
                        }).ToList()
                    })
                .ToList();

            foreach (var order in ordrar)
                {
                AllaOrdrar.Add(order);
                }

            }

        private void HämtaAllaOrdrar()
            {
            AllaOrdrar.Clear();
            var ordrar = _context.Ordrars
                .Include(o => o.OrderRaders)
                    .ThenInclude(or => or.IsbnNavigation)
                .Include(o => o.Kund)
                .Select(o => new OrderHistorikDTO
                    {
                    Id = o.Id,
                    Orderdatum = o.Orderdatum,
                    Status = o.Status,
                    ButikID = o.ButikId,
                    TotalBelopp = o.TotalBelopp,
                    KundID = o.KundId,
                    AntalBöcker = o.OrderRaders.Sum(r => r.Antal),
                    OrderRader = o.OrderRaders.Select(r => new OrderRadDTO
                        {
                        BokTitel = r.IsbnNavigation.Titel,
                        Antal = r.Antal,
                        Pris = r.Pris,
                        Totalt = r.Antal * r.Pris
                        }).ToList()
                    })
                .ToList();

            foreach (var order in ordrar)
                {
                AllaOrdrar.Add(order);
                }

            }

        private void Avbryt()
            {
            Application.Current.Windows.OfType<Views.HanteraOrderView>().FirstOrDefault()?.Close();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Focus();
            }
        }
    }