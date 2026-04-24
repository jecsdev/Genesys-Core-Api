using Genesis_Core_Api.Data;
using Genesis_Core_Api.Models;
using Genesis_Core_Api.Models.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis_Core_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicePlanController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicePlanController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/serviceplan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicePlanDto>>> GetServicePlans()
        {
            return await _context.ServicePlans
                .Include(p => p.Benefits)
                .Include(p => p.Affiliates)
                .Select(p => new ServicePlanDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BasePrice = p.BasePrice,
                    IncludedDependents = p.IncludedDependents,
                    ExtraDependentPrice = p.ExtraDependentPrice,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    Benefits = p.Benefits.Select(b => b.Description).ToList(),
                    AffiliatesCount = p.Affiliates.Count
                })
                .ToListAsync();
        }

        // GET: api/serviceplan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicePlanDto>> GetServicePlan(int id)
        {
            var plan = await _context.ServicePlans
                .Include(p => p.Benefits)
                .Include(p => p.Affiliates)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
                return NotFound();

            return new ServicePlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                BasePrice = plan.BasePrice,
                IncludedDependents = plan.IncludedDependents,
                ExtraDependentPrice = plan.ExtraDependentPrice,
                IsActive = plan.IsActive,
                CreatedAt = plan.CreatedAt,
                Benefits = plan.Benefits.Select(b => b.Description).ToList(),
                AffiliatesCount = plan.Affiliates.Count
            };
        }

        // POST: api/serviceplan
        [HttpPost]
        public async Task<ActionResult<ServicePlanDto>> PostServicePlan(CreateServicePlanDto dto)
        {
            var plan = new ServicePlan
            {
                Name = dto.Name,
                Description = dto.Description,
                BasePrice = dto.BasePrice,
                IncludedDependents = dto.IncludedDependents,
                ExtraDependentPrice = dto.ExtraDependentPrice,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                Benefits = dto.Benefits.Select(b => new PlanBenefit
                {
                    Description = b
                }).ToList()
            };

            _context.ServicePlans.Add(plan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServicePlan), new { id = plan.Id }, new ServicePlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                BasePrice = plan.BasePrice,
                IncludedDependents = plan.IncludedDependents,
                ExtraDependentPrice = plan.ExtraDependentPrice,
                IsActive = plan.IsActive,
                CreatedAt = plan.CreatedAt,
                Benefits = plan.Benefits.Select(b => b.Description).ToList(),
                AffiliatesCount = 0
            });
        }

        // PUT: api/serviceplan/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicePlan(int id, CreateServicePlanDto dto)
        {
            var plan = await _context.ServicePlans
                .Include(p => p.Benefits)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
                return NotFound();

            plan.Name = dto.Name;
            plan.Description = dto.Description;
            plan.BasePrice = dto.BasePrice;
            plan.IncludedDependents = dto.IncludedDependents;
            plan.ExtraDependentPrice = dto.ExtraDependentPrice;
            plan.IsActive = dto.IsActive;

            // Actualizar beneficios — eliminar los viejos y agregar los nuevos
            _context.PlanBenefits.RemoveRange(plan.Benefits);
            plan.Benefits = dto.Benefits.Select(b => new PlanBenefit
            {
                Description = b,
                ServicePlanId = plan.Id
            }).ToList();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/serviceplan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicePlan(int id)
        {
            var plan = await _context.ServicePlans.FindAsync(id);

            if (plan == null)
                return NotFound();

            // Soft delete — solo desactiva
            plan.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}