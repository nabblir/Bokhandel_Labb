using Bokhandel_Labb.ViewModels;

namespace Bokhandel_Labb.DTO
    {
    public class FörfattareDTO : BaseViewModel
        {
        private int _författarId;
        private string _förnamn;
        private string _efternamn;
        public int FörfattarId
            {
            get => _författarId;
            set => SetProperty(ref _författarId, value);
            }
        public string Förnamn
            {
            get => _förnamn;
            set => SetProperty(ref _förnamn, value);
            }
        public string Efternamn
            {
            get => _efternamn;
            set => SetProperty(ref _efternamn, value);
            }
        public string Namn => $"{Förnamn} {Efternamn}";
        }
    }