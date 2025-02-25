using System;
using System.Collections.Generic;

namespace SaleProject.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public decimal TotalCost { get; set; }

    public virtual ICollection<Concept> Concepts { get; set; } = new List<Concept>();
}
