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

        // Pagos
        public decimal TotalRevenueThisMonth { get; set; }
        public decimal TotalPendingAmount { get; set; }
        public decimal TotalOverdueAmount { get; set; }
        public int PaidPaymentsCount { get; set; }
        public int PendingPaymentsCount { get; set; }
        public int OverduePaymentsCount { get; set; }
        public List<RecentPaymentDto> RecentPayments { get; set; } = new();
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

    public class RecentPaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; } = null!;
        public string AffiliateName { get; set; } = null!;
        public string AffiliateNumber { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
    }
}
