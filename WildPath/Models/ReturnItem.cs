using System;
using System.Collections.Generic;

namespace WildPath.Models;

public partial class ReturnItem
{
    public int FkReturnId { get; set; }

    public int FkItemId { get; set; }

    public int Quantity { get; set; }

    public virtual Item FkItem { get; set; } = null!;

    public virtual Return FkReturn { get; set; } = null!;
}
