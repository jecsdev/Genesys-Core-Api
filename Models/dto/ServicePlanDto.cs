namespace Genesis_Core_Api.Models.dto
{
    public class ServicePlanDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public int IncludedDependents { get; set; }
        public decimal ExtraDependentPrice { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Benefits { get; set; } = new();
        public int AffiliatesCount { get; set; }
    }
}
