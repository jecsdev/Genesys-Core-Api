namespace Genesis_Core_Api.Models.dto
{
    public class CreateAffiliatePaymentDto
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime DueDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? ReferenceNumber { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? Notes { get; set; }
        public int AffiliateId { get; set; }
    }
}
