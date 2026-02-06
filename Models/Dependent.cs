namespace Genesis_Core_Api.Models
{
    public class Dependent
    {
        public int Id { get; set; }

        public string DependentNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Identification { get; set; } = null!;

        // Foreign Key
        public int AffiliateId { get; set; }
        public Affiliate Affiliate { get; set; } = null!;
    }
}
