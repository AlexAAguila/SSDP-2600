using WildPath.EfModels;

namespace WildPath.ViewModels
{
    public class CheckoutVM
    {
        public bool IsLoggedIn { get; set; }

        public double totalPrice { get; set; }

        public int totalItems { get; set; }

        public bool hasAddress { get; set; }

        //public int? PkAddressId { get; set; }

        //public int? Unit { get; set; }

        //public string? Address1 { get; set; } = null!;

        //public string? City { get; set; } = null!;

        //public string? Province { get; set; } = null!;

        //public string? PostalCode { get; set; } = null!;

        public Address Address { get; set; }

    }
}