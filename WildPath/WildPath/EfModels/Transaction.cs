using System;
using System.Collections.Generic;

namespace WildPath.EfModels;

public partial class Transaction
{
    public int PkTransactionId { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly PurchaseDate { get; set; }

    public string ShippingMethod { get; set; } = null!;
}
