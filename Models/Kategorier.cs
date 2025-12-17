using System;
using System.Collections.Generic;

namespace Bokhandel_Labb.Models;

public partial class Kategorier
{
    public int Id { get; set; }

    public string Namn { get; set; } = null!;

    public string? Beskrivning { get; set; }

    public virtual ICollection<Böcker> Böckers { get; set; } = new List<Böcker>();
}
