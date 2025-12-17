using System;
using System.Collections.Generic;

namespace Bokhandel_Labb.Models;

public partial class FörsäljningPerButik
{
    public string Butiksnamn { get; set; } = null!;

    public string Stad { get; set; } = null!;

    public int? AntalOrdrar { get; set; }

    public int? AntalUnikaKunder { get; set; }

    public decimal TotalOmsättning { get; set; }

    public string? MedianOrdervärde { get; set; }
}
