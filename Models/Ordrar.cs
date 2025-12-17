using System;
using System.Collections.Generic;

namespace Bokhandel_Labb.Models;

public partial class Ordrar
{
    public int Id { get; set; }

    public int KundId { get; set; }

    public int ButikId { get; set; }

    public DateTime Orderdatum { get; set; }

    public decimal TotalBelopp { get; set; }

    public string Status { get; set; } = null!;

    public virtual Butiker Butik { get; set; } = null!;

    public virtual Kunder Kund { get; set; } = null!;

    public virtual ICollection<OrderRader> OrderRaders { get; set; } = new List<OrderRader>();
}
