using Genesis_Core_Api.Data;
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

            return Ok(new DashboardStatsDto
            {
                TotalAffiliates = totalAffiliates,
                TotalDependents = totalDependents,
                TotalCompanies = totalCompanies,
                TotalActive = totalActive,
                TotalInactive = totalInactive,
                RecentAffiliations = recentCombined
            });
        }
    }
}