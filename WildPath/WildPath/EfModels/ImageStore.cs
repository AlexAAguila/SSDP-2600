using System;
using System.Collections.Generic;

namespace WildPath.EfModels;

public partial class ImageStore
{
    public int ImageId { get; set; }

    public string FileName { get; set; } = null!;

    public byte[] Image { get; set; } = null!;
}
