using WildPath.EfModels;

namespace WildPath.ViewModels
{
    public class CheckoutVM
    {
        public bool IsLoggedIn { get; set; }
        public string? PayerEmail { get; set; }
        public string? Phone { get; set; }
        public bool hasAddress { get; set; }
        public Address Address { get; set; }
        public int TotalItems { get; set; }
        public double TotalPrice { get; set; }
        public double Shipping { get; set; }
        public double ShippingRate { get; set; }
        public double FreeShippingThreshold { get; set; }
        public bool IsFreeShipping { get; set; }
        public string ShippingMethod { get; set; }
        public double Tax { get; set; }
        public double GrandTotal { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public string TransactionId { get; set; }
        public string TrackingNumber { get; set; }
        public string PayerId { get; set; }
        public string PayerFullName { get; set; }
        public string CaptureId { get; set; }
    }

}