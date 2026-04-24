namespace Genesis_Core_Api.Models
{
    public class ServicePlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public int IncludedDependents { get; set; } = 4;
        public decimal ExtraDependentPrice { get; set; } 
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<PlanBenefit> Benefits { get; set; } = new List<PlanBenefit>();
        public ICollection<Affiliate> Affiliates { get; set; } = new List<Affiliate>();
    }
}
