namespace Genesis_Core_Api.Models.dto
{
    public class CreateDependentDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string Relationship { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public int AffiliateId { get; set; }
    }
}
