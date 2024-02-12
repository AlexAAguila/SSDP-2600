using System;
using System.Collections.Generic;

namespace WildPath.EfModels;

public partial class Refund
{
    public int FkReturnId { get; set; }

    public int Amount { get; set; }

    public string Status { get; set; } = null!;

    public virtual Return FkReturn { get; set; } = null!;
}
