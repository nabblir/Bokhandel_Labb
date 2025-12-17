using System;
using System.Collections.Generic;

namespace Bokhandel_Labb.Models;

public partial class LagersaldoPerButik
{
    public string Isbn { get; set; } = null!;

    public string Titel { get; set; } = null!;

    public string? BokhandelnStockholmCity { get; set; }

    public string? AkademibokhandelnGöteborg { get; set; }

    public string? StudentbokhandelnMalmö { get; set; }
}
