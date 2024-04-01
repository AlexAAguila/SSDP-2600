using System;
using System.Collections.Generic;

namespace WildPath.EfModels;

public partial class ShippingInfo
{
    public int PkShippingInfo { get; set; }

    public string? CurrencyCode { get; set; }

    public string? CurrencySymbol { get; set; }

    public double? CaTax { get; set; }

    public double? ShippingRate { get; set; }

    public double? FreeShippingThreshold { get; set; }
}
