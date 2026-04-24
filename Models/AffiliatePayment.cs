namespace Genesis_Core_Api.Models
{
    public class AffiliatePayment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime DueDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? ReferenceNumber { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? Notes { get; set; }

        // Foreign Key
        public int AffiliateId { get; set; }

        // Navigation
        public Affiliate? Affiliate { get; set; }
    }
}
