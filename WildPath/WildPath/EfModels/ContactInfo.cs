using System;
using System.Collections.Generic;

namespace WildPath.EfModels;

public partial class ContactInfo
{
    public int PkShippingProfile { get; set; }

    public int FkMailingAddressId { get; set; }

    public int? FkShippingAddressId { get; set; }

    public long? Phone { get; set; }

    public virtual Address FkMailingAddress { get; set; } = null!;

    public virtual Address? FkShippingAddress { get; set; }
}
