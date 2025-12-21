using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bokhandel_Labb.Models
{
    public partial class LoggHistorik
    {
        [Key]
        public int LoggId { get; set; }

        public string User { get; set; } = null!;

        public string Butiksnamn { get; set; } = null!;

        public int ButikId { get; set; }

        public DateTime Datum { get; set; }

        public string Händelse { get; set; } = null!;

        public string LogTyp { get; set; }

        }
}
