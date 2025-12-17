using System;
using System.Collections.Generic;

namespace Bokhandel_Labb.Models;

public partial class Kunder
{
    public int Id { get; set; }

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefon { get; set; }

    public string? Adress { get; set; }

    public string? Postnummer { get; set; }

    public string? Stad { get; set; }

    public DateOnly Registreringsdatum { get; set; }

    public virtual ICollection<Ordrar> Ordrars { get; set; } = new List<Ordrar>();
}
