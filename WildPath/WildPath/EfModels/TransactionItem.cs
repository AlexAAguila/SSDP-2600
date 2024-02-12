using System;
using System.Collections.Generic;

namespace WildPath.EfModels;

public partial class TransactionItem
{
    public int FkTransactionId { get; set; }

    public int FkItemId { get; set; }

    public int Quantity { get; set; }

    public virtual Item FkItem { get; set; } = null!;

    public virtual Transaction FkTransaction { get; set; } = null!;
}
