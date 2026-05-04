using System.ComponentModel.DataAnnotations.Schema;

namespace Genesis_Core_Api.Models
{
    public class Affiliate
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
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
     
        public int CompanyId { get; set; }
        // Navigation
        public Company? Company { get; set; }
        public ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();

        // Plan
        public int ServicePlanId { get; set; }
        public DateTime PlanStartDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public ServicePlan? ServicePlan { get; set; }
        public ICollection<AffiliatePayment> Payments { get; set; } = new List<AffiliatePayment>();
    }
}
