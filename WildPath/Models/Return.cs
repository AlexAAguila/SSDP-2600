using System;
using System.Collections.Generic;

namespace WildPath.Models;

public partial class Return
{
    public int PkReturnId { get; set; }

    public int FkTransactionId { get; set; }

    public string ReasonForReturn { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Status { get; set; } = null!;

    public virtual Transaction FkTransaction { get; set; } = null!;
}
