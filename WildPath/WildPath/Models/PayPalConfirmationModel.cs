namespace WildPath.Models
{
    public class PayPalConfirmationModel
    {
        public string TransactionId { get; set; }
        public string Amount { get; set; }
        public string PayerName { get; set; }
    }
}