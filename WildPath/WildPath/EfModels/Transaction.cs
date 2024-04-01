using System;
using System.Collections.Generic;

namespace WildPath.EfModels;

public partial class Transaction
{
    public string PaymentId { get; set; } = null!;

    public string CreateTime { get; set; } = null!;

    public string PayerName { get; set; } = null!;

    public string PayerEmail { get; set; } = null!;

    public string Amount { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string ShippingMethod { get; set; } = null!;

    public int FkAddressId { get; set; }

    public string? FkUserId { get; set; }

    public virtual Address FkAddress { get; set; } = null!;
}
