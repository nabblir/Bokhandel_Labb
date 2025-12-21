using Bokhandel_Labb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bokhandel_Labb.DTO
    {
    public class LoggHistorikDTO : BaseViewModel
        {
        private string _user;
        private string _butiksNamn;
        private DateTime _datum;
        private string _händelse;
        private int _butikId;
        private int _loggID;
        private string _logTyp;
        public string User
            {
            get => _user;
            set => SetProperty(ref _user, value);
            }

        public string ButiksNamn
            {
            get => _butiksNamn;
            set => SetProperty(ref _butiksNamn, value);
            }

        public DateTime Datum
            {
            get => _datum;
            set => SetProperty(ref _datum, value);
            }

        public string Händelse
            {
            get => _händelse;
            set => SetProperty(ref _händelse, value);
            }

        public int ButikId
            {
            get => _butikId;
            set => SetProperty(ref _butikId, value);
            }
        public int LoggID
            {
            get => _loggID;
            set => SetProperty(ref _loggID, value);
            }
        public string LogTyp 
            {
            get => _logTyp;
            set => SetProperty(ref _logTyp, value);
            }
        }
    }
