namespace Genesis_Core_Api.Models.dto
{
    public class AffiliateDto
    {
        public int Id { get; set; }
        public string AffiliateNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Position { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public int DependentsCount { get; set; }
    }
}
