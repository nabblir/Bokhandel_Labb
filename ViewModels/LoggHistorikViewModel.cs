using Bokhandel_Labb.Commands;
using Bokhandel_Labb.DTO;
using Bokhandel_Labb.DTOs;
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
    public class LoggHistorikViewModel : BaseViewModel
        {
        private readonly BokhandelContext _context;
        public ObservableCollection<LoggHistorikDTO> AllaLoggar { get; set; }
        
        private LoggHistorikDTO _valdLogg;
        public LoggHistorikDTO ValdLogg
            {
            get => _valdLogg;
            set
                {
                if (SetProperty(ref _valdLogg, value))
                    {
                    if (value != null)
                        {
                        VisaInfo($"Händelse[#{value.LoggID}]\n{value.Händelse} \n Butik: {value.ButiksNamn}.");
                        }
                    }
                }
            }
        private string _statusMeddelande = "Klicka på en logg för att läsa händelsen.";
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
                    HämtaLoggar(ValdButik1.ButikId);
                    }
                }
            }

        public LoggHistorikViewModel()
            {
            _context = new BokhandelContext();
            AllaLoggar = new ObservableCollection<LoggHistorikDTO>();
            TillgängligaButiker1 = new ObservableCollection<ButikDTO>();
            HämtaButiker();
            }

        private void HämtaButiker()
            {
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

            // Sätt första butiken som vald automatiskt (valfritt)
            if (TillgängligaButiker1.Count > 0)
                {
                ValdButik1 = TillgängligaButiker1[0];
                }
            }

        public void VisaInfo(string meddelande)
            {
            StatusTextFärg = System.Windows.Media.Brushes.Black;
            StatusMeddelande = meddelande;
            }

        private void HämtaLoggar(int butikID)
            {
            AllaLoggar.Clear();
            var loggar = _context.LoggHistorik
                .Select(logg => new LoggHistorikDTO
                    {
                    User = logg.User,
                    ButiksNamn = logg.Butiksnamn,
                    Datum = logg.Datum,
                    Händelse = logg.Händelse,
                    ButikId = logg.ButikId,
                    LoggID = logg.LoggId,
                    LogTyp = logg.LogTyp
                    })
                .Where(l => l.ButikId == butikID)
                .ToList();

            foreach (var logg in loggar)
                {
                AllaLoggar.Add(logg);
                }
            }
        }
    }
