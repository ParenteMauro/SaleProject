using System;
using System.Collections.Generic;

namespace SaleProject.Models;

public partial class Sale
{
    public long Id { get; set; }

    public DateTime Date { get; set; }

    public decimal Total { get; set; }

    public int IdCustomer { get; set; }

    public virtual ICollection<Concept> Concepts { get; set; } = new List<Concept>();

    public virtual Customer IdCustomerNavigation { get; set; } = null!;
}
