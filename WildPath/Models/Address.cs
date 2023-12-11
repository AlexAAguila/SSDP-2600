using System;
using System.Collections.Generic;

namespace WildPath.Models;

public partial class Address
{
    public int PkAddressId { get; set; }

    public int? Unit { get; set; }

    public string Address1 { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public virtual ICollection<Profile> ProfileFkMailingAddresses { get; set; } = new List<Profile>();

    public virtual ICollection<Profile> ProfileFkShippingAddresses { get; set; } = new List<Profile>();
}
