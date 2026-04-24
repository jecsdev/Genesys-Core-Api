namespace Genesis_Core_Api.Models
{
    public class PlanBenefit
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;

        // Foreign Key
        public int ServicePlanId { get; set; }

        // Navigation
        public ServicePlan? ServicePlan { get; set; }
    }
}
