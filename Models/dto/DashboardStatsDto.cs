namespace Genesis_Core_Api.Models.dto
{
    public class DashboardStatsDto
    {
        public int TotalAffiliates { get; set; }
        public int TotalDependents { get; set; }
        public int TotalCompanies { get; set; }
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
        public List<RecentAffiliationDto> RecentAffiliations { get; set; } = new();
    }
    public class RecentAffiliationDto
    {
        public string FullName { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string Type { get; set; } = null!; // Titular | Dependiente
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
