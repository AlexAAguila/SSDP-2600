using System;
using System.Collections.Generic;

namespace WildPath.Models;

public partial class Transaction
{
    public int PkTransactionId { get; set; }

    public string FkEmail { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime PurchaseDate { get; set; }

    public string ShippingMethod { get; set; } = null!;

    public virtual Profile FkEmailNavigation { get; set; } = null!;

    public virtual ICollection<Return> Returns { get; set; } = new List<Return>();
}
