using System;
using System.Collections.Generic;

namespace Bokhandel_Labb.Models;

public partial class Butiker
{
    public int Id { get; set; }

    public string Butiksnamn { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string Postnummer { get; set; } = null!;

    public string Stad { get; set; } = null!;

    public string? Telefon { get; set; }

    public virtual ICollection<LagerSaldo> LagerSaldos { get; set; } = new List<LagerSaldo>();

    public virtual ICollection<Ordrar> Ordrars { get; set; } = new List<Ordrar>();
}
