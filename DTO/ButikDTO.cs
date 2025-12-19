using Bokhandel_Labb.ViewModels;

namespace Bokhandel_Labb.DTOs
    {
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
    }