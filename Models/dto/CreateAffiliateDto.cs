namespace Genesis_Core_Api.Models.dto
{
    public class CreateAffiliateDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Position { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public int CompanyId { get; set; }
        public int ServicePlanId { get; set; }
    }
}
