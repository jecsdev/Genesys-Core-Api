using System.ComponentModel.DataAnnotations.Schema;

namespace Genesis_Core_Api.Models
{
    public class Dependent
    {
        public int Id { get; set; }
        public string DependentNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string Relationship { get; set; } = null!; // Hijo, Cónyuge, etc.
        public string Phone { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Foreign Key
        public int AffiliateId { get; set; }
        // Navigation
        [ForeignKey("AffiliateId")]
        public Affiliate? Affiliate { get; set; }
    }
}
