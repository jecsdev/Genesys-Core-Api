namespace Genesis_Core_Api.Models.dto
{
    public class AffiliatePaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime DueDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? ReferenceNumber { get; set; }
        public string Status { get; set; } = null!;
        public string? Notes { get; set; }
        public int AffiliateId { get; set; }
        public string AffiliateName { get; set; } = null!;
        public string AffiliateNumber { get; set; } = null!;
    }
}
