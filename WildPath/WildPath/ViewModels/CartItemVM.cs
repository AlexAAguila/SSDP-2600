using System.ComponentModel.DataAnnotations;
using WildPath.EfModels;

namespace WildPath.ViewModels
{
    public class CartItemVM
    {
        public int ItemId { get; set; }


        public string ItemName { get; set; } = null!;

        public string ItemDetails { get; set; } = null!;

        public double Price { get; set; }

        public string Category { get; set; } = null!;

        public string Size { get; set; }

        public string Colour { get; set; }

        public byte[] Image { get; set; }
        public int Quantity { get; set; }
    }
}
