namespace Genesis_Core_Api.Models
{
    public class Affiliate
    {
        public int Id { get; set; }

        public string AffiliateNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Identification { get; set; } = null!; 

        // Foreign Key
        public int CompanyId { get; set; }

        // Navigation
        public ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();
    }
}
