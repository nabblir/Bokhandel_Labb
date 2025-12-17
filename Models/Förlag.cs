using System;
using System.Collections.Generic;

namespace Bokhandel_Labb.Models;

public partial class Förlag
{
    public int Id { get; set; }

    public string Namn { get; set; } = null!;

    public string Land { get; set; } = null!;

    public int? Grundat { get; set; }

    public string? Webbplats { get; set; }

    public virtual ICollection<Böcker> Böckers { get; set; } = new List<Böcker>();
}
