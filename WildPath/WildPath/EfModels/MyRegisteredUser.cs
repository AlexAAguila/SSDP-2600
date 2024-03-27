using System;
using System.Collections.Generic;

namespace WildPath.EfModels;

public partial class MyRegisteredUser
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? FkMailingAdressId { get; set; }

    public int? FkShippingAdressId { get; set; }

    public long? Phone { get; set; }

    public virtual Address? FkMailingAdress { get; set; }

    public virtual Address? FkShippingAdress { get; set; }
}
