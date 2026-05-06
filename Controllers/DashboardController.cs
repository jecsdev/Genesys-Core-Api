using Genesis_Core_Api.Data;
using Genesis_Core_Api.Models;
using Genesis_Core_Api.Models.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis_Core_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            var totalAffiliates = await _context.Affiliates.CountAsync();
            var totalDependents = await _context.Dependents.CountAsync();
            var totalCompanies = await _context.Companies.CountAsync();
            var totalActive = await _context.Affiliates.CountAsync(a => a.IsActive);
            var totalInactive = await _context.Affiliates.CountAsync(a => !a.IsActive);

            // Últimas 5 afiliaciones (titulares y dependientes mezclados por fecha)
            var recentAffiliates = await _context.Affiliates
                .Include(a => a.Company)
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a => new RecentAffiliationDto
                {
                    FullName = $"{a.FirstName} {a.LastName}",
                    Identification = a.Identification,
                    CompanyName = a.Company != null ? a.Company.Name : "",
                    Type = "Titular",
                    IsActive = a.IsActive,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            var recentDependents = await _context.Dependents
                .Include(d => d.Affiliate)
                    .ThenInclude(a => a!.Company)
                .OrderByDescending(d => d.CreatedAt)
                .Take(5)
                .Select(d => new RecentAffiliationDto
                {
                    FullName = $"{d.FirstName} {d.LastName}",
                    Identification = d.Identification,
                    CompanyName = d.Affiliate != null && d.Affiliate.Company != null
                        ? d.Affiliate.Company.Name : "",
                    Type = "Dependiente",
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();

            var recentCombined = recentAffiliates
                .Concat(recentDependents)
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .ToList();

            // Estadísticas de pagos
            var now = DateTime.UtcNow;

            var totalRevenueThisMonth = await _context.AffiliatePayments
                .Where(p => p.Status == PaymentStatus.Paid
                         && p.PaymentDate.Year == now.Year
                         && p.PaymentDate.Month == now.Month)
                .SumAsync(p => (decimal?)p.Amount) ?? 0m;

            var totalPendingAmount = await _context.AffiliatePayments
                .Where(p => p.Status == PaymentStatus.Pending)
                .SumAsync(p => (decimal?)p.Amount) ?? 0m;

            var totalOverdueAmount = await _context.AffiliatePayments
                .Where(p => p.Status == PaymentStatus.Overdue)
                .SumAsync(p => (decimal?)p.Amount) ?? 0m;

            var paidCount = await _context.AffiliatePayments
                .CountAsync(p => p.Status == PaymentStatus.Paid
                              && p.PaymentDate.Year == now.Year
                              && p.PaymentDate.Month == now.Month);

            var pendingCount = await _context.AffiliatePayments
                .CountAsync(p => p.Status == PaymentStatus.Pending);

            var overdueCount = await _context.AffiliatePayments
                .CountAsync(p => p.Status == PaymentStatus.Overdue);

            var recentPayments = await _context.AffiliatePayments
                .Include(p => p.Affiliate)
                .OrderByDescending(p => p.PaymentDate)
                .Take(5)
                .Select(p => new RecentPaymentDto
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    Status = p.Status == PaymentStatus.Paid ? "Pagado"
                           : p.Status == PaymentStatus.Overdue ? "Vencido"
                           : "Pendiente",
                    AffiliateName = p.Affiliate != null
                        ? $"{p.Affiliate.FirstName} {p.Affiliate.LastName}" : "",
                    AffiliateNumber = p.Affiliate != null ? p.Affiliate.AffiliateNumber : "",
                    PaymentMethod = p.PaymentMethod
                })
                .ToListAsync();

            return Ok(new DashboardStatsDto
            {
                TotalAffiliates = totalAffiliates,
                TotalDependents = totalDependents,
                TotalCompanies = totalCompanies,
                TotalActive = totalActive,
                TotalInactive = totalInactive,
                RecentAffiliations = recentCombined,
                TotalRevenueThisMonth = totalRevenueThisMonth,
                TotalPendingAmount = totalPendingAmount,
                TotalOverdueAmount = totalOverdueAmount,
                PaidPaymentsCount = paidCount,
                PendingPaymentsCount = pendingCount,
                OverduePaymentsCount = overdueCount,
                RecentPayments = recentPayments
            });
        }
    }
}