using System;
using System.Collections.Generic;

namespace WildPath.EfModels;

public partial class Address
{
    public int PkAddressId { get; set; }

    public int? Unit { get; set; }

    public string Address1 { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public virtual ICollection<ContactInfo> ContactInfoFkMailingAddresses { get; set; } = new List<ContactInfo>();

    public virtual ICollection<ContactInfo> ContactInfoFkShippingAddresses { get; set; } = new List<ContactInfo>();
}
