using System;
using System.Collections.Generic;

namespace Bokhandel_Labb.Models;

public partial class Författare
{
    public int Id { get; set; }

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public DateOnly Födelsedatum { get; set; }

    public virtual ICollection<Böcker> Isbns { get; set; } = new List<Böcker>();
}
