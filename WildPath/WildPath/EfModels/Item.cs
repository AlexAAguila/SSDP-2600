﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WildPath.EfModels;

public partial class Item
{
    public int PkItemId { get; set; }

    public string Supplier { get; set; } = null!;
    [Display(Name = "Item Name")]
    public string ItemName { get; set; } = null!;

    [StringLength(1000, ErrorMessage = "The details cannot exceed 1000 characters.")]
    [Display(Name = "Item Details")]
    public string ItemDetails { get; set; } = null!;

    public double Price { get; set; }

    public int Stock { get; set; }

    public string Category { get; set; } = null!;

    public double Weight { get; set; }

    public string? Size { get; set; }

    public string? Colour { get; set; }

    public string? ItemImageId { get; set; }
}
