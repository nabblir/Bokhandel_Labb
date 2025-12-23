using Bokhandel_Labb.ViewModels;

namespace Bokhandel_Labb.DTO
    {
    public class FörlagDTO : BaseViewModel
        {
        private int _förlagId;
        private string _namn;
        private string _land;
        private string _webbplats;

        public int FörlagId
            {
            get => _förlagId;
            set => SetProperty(ref _förlagId, value);
            }

        public string Namn
            {
            get => _namn;
            set => SetProperty(ref _namn, value);
            }

        public string Land
            {
            get => _land;
            set => SetProperty(ref _land, value);
            }

        public string Webbplats
            {
            get => _webbplats;
            set => SetProperty(ref _webbplats, value);
            }
        }
    }