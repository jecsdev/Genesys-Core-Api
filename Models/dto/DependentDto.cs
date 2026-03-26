namespace Genesis_Core_Api.Models.dto
{
    public class DependentDto
    {
        public int Id { get; set; }
        public string DependentNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string Relationship { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AffiliateId { get; set; }
        public string AffiliateName { get; set; } = null!;
        public string AffiliateNumber { get; set; } = null!;
    }
}
