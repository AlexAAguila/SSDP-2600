using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WildPath.EfModels;

public partial class Transaction
{
    public string PaymentId { get; set; } = null!;

    [Display(Name = "Create Time")]
    public string CreateTime { get; set; } = null!;
    [Display(Name = "Payer Name")]
    public string PayerName { get; set; } = null!;
    [Display(Name = "Payer Email")]
    public string PayerEmail { get; set; } = null!;

    public string Amount { get; set; } = null!;

    public string Currency { get; set; } = null!;
    [Display(Name = "Payment Method")]
    public string PaymentMethod { get; set; } = null!;
    [Display(Name = "Shipping Method")]
    public string ShippingMethod { get; set; } = null!;

    public int FkAddressId { get; set; }

    public string? FkUserId { get; set; }

    public virtual Address FkAddress { get; set; } = null!;
}
