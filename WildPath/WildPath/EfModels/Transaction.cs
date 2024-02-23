using System;
using System.Collections.Generic;

namespace WildPath.EfModels;

public partial class Transaction
{
    public string PaymentId { get; set; } = null!;

    public string? CreateTime { get; set; }

    public string? PayerName { get; set; }

    public string? PayerEmail { get; set; }

    public string? Amount { get; set; }

    public string? Currency { get; set; }

    public string? PaymentMethod { get; set; }
}
