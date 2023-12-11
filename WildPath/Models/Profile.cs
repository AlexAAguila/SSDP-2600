using System;
using System.Collections.Generic;

namespace WildPath.Models;

public partial class Profile
{
    public string PkEmail { get; set; } = null!;

    public int FkMailingAddressId { get; set; }

    public int? FkShippingAddressId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public long? Phone { get; set; }

    public string IsAdmin { get; set; } = null!;

    public virtual Address FkMailingAddress { get; set; } = null!;

    public virtual Address? FkShippingAddress { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
