using System;
using System.Collections.Generic;

namespace Bokhandel_Labb.DTO
    {
    public class OrderHistorikDTO
        {
        public int Id { get; set; }
        public DateTime Orderdatum { get; set; }
        public decimal TotalBelopp { get; set; }
        public string Status { get; set; } = null!;
        public int ButikID { get; set; }

        public string ButiksNamn
            {
            get
                {
                return ButikID switch // Hardkodade butiknamn för demoändamål, lazy eftersom detta måste hämtas från databasen
                    {
                        1 => "Bokhandeln Stockholm City",
                        2 => "Bokhandeln Göteborg",
                        3 => "Bokhandeln Malmö",
                        _ => "Okänd"
                        };
                }
            }
        public int KundID { get; set; }
        public int AntalBöcker { get; set; }
        public List<OrderRadDTO> OrderRader { get; set; } = new List<OrderRadDTO>();
        public System.Windows.Media.Brush LeveransFärg
            {
            get
                {
                return Status switch
                    {
                        "Levererad" => System.Windows.Media.Brushes.Green,
                        "Skickad" => System.Windows.Media.Brushes.DodgerBlue,
                        "Pågående" => System.Windows.Media.Brushes.Orange,
                        _ => System.Windows.Media.Brushes.Gray
                        };
                }
            }
        }


    public class OrderRadDTO
        {
        public string BokTitel { get; set; } = null!;
        public string Författare { get; set; } = null!;
        public int Antal { get; set; }
        public decimal Pris { get; set; }
        public decimal Totalt { get; set; }
        }
    }