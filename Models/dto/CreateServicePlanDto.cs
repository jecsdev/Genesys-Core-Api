namespace Genesis_Core_Api.Models.dto
{
    public class CreateServicePlanDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public int IncludedDependents { get; set; } = 4;
        public decimal ExtraDependentPrice { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string> Benefits { get; set; } = new();
    }
}
