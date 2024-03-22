namespace WildPath.Models
{
    public class PayPalConfirmationModel
    {
        public string TransactionId { get; set; }
        public string Amount { get; set; }
        public string PayerName { get; set; }
        public string PayerEmail { get; set; }
        public string PaymentMethod { get; set;}
        public string TrackingNumber { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }


    }
}