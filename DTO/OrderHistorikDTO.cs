using Bokhandel_Labb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bokhandel_Labb.Models;
using Bokhandel_Labb.ViewModels;

namespace Bokhandel_Labb.DTO
{
    public class OrderHistorikDTO : BaseViewModel
        {
        private string _isbn;
        private string _titel;
        private string _författareNamn;
        private string _butiksNamn;
        private int _antalKöpta;
        private int _butikId;
        private int _totalPris;

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

        public int AntalKöpta
            {
            get => _antalKöpta;
            set => SetProperty(ref _antalKöpta, value);
            }

        public int ButikId
            {
            get => _butikId;
            set => SetProperty(ref _butikId, value);
            }

        public string ButiksNamn
            {
            get
                {
                if (!string.IsNullOrEmpty(_butiksNamn))
                    return _butiksNamn;

                //Fallback: Hämta butiksnamn från databasen om det inte redan är satt
                using (var context = new BokhandelContext())
                    {
                    var butik = context.Butikers.FirstOrDefault(b => b.Id == ButikId);
                    return butik?.Butiksnamn ?? "Okänd butik";
                    }
                }
            set => SetProperty(ref _butiksNamn, value);
            }
        public int TotalPris
            {
            get => _totalPris;
            set => SetProperty(ref _totalPris, value);
            }
        }
}
