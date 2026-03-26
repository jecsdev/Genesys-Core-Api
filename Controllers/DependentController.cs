using Genesis_Core_Api.Data;
using Genesis_Core_Api.Models;
using Genesis_Core_Api.Models.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis_Core_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DependentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Dependent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DependentDto>>> GetDependents()
        {
            return await _context.Dependents
                .Include(d => d.Affiliate)
                .Select(d => new DependentDto
                {
                    Id = d.Id,
                    DependentNumber = d.DependentNumber,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Identification = d.Identification,
                    Relationship = d.Relationship,
                    Phone = d.Phone,
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt,
                    AffiliateId = d.AffiliateId,
                    AffiliateName = d.Affiliate != null
                        ? $"{d.Affiliate.FirstName} {d.Affiliate.LastName}"
                        : "",
                    AffiliateNumber = d.Affiliate != null
                        ? d.Affiliate.AffiliateNumber
                        : ""
                })
                .ToListAsync();
        }

        // GET: api/Dependent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DependentDto>> GetDependent(int id)
        {
            var d = await _context.Dependents
                .Include(d => d.Affiliate)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (d == null)
                return NotFound();

            return new DependentDto
            {
                Id = d.Id,
                DependentNumber = d.DependentNumber,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Identification = d.Identification,
                Relationship = d.Relationship,
                Phone = d.Phone,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt,
                AffiliateId = d.AffiliateId,
                AffiliateName = d.Affiliate != null
                    ? $"{d.Affiliate.FirstName} {d.Affiliate.LastName}"
                    : "",
                AffiliateNumber = d.Affiliate != null
                    ? d.Affiliate.AffiliateNumber
                    : ""
            };
        }

        // GET: api/Dependent/affiliate/5
        [HttpGet("affiliate/{affiliateId}")]
        public async Task<ActionResult<IEnumerable<DependentDto>>> GetDependentsByAffiliate(int affiliateId)
        {
            return await _context.Dependents
                .Include(d => d.Affiliate)
                .Where(d => d.AffiliateId == affiliateId)
                .Select(d => new DependentDto
                {
                    Id = d.Id,
                    DependentNumber = d.DependentNumber,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Identification = d.Identification,
                    Relationship = d.Relationship,
                    Phone = d.Phone,
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt,
                    AffiliateId = d.AffiliateId,
                    AffiliateName = d.Affiliate != null
                        ? $"{d.Affiliate.FirstName} {d.Affiliate.LastName}"
                        : "",
                    AffiliateNumber = d.Affiliate != null
                        ? d.Affiliate.AffiliateNumber
                        : ""
                })
                .ToListAsync();
        }

        // POST: api/Dependent
        [HttpPost]
        public async Task<ActionResult<DependentDto>> PostDependent(CreateDependentDto dto)
        {
            // Generar número de dependiente automáticamente
            var count = await _context.Dependents
                .Where(d => d.AffiliateId == dto.AffiliateId)
                .CountAsync();

            var affiliate = await _context.Affiliates.FindAsync(dto.AffiliateId);
            if (affiliate == null)
                return BadRequest(new { message = "Titular no encontrado." });

            var dependentNumber = $"{affiliate.AffiliateNumber}-DEP-{String.Format("{0:00}", count + 1)}";

            var dependent = new Dependent
            {
                DependentNumber = dependentNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Identification = dto.Identification,
                Relationship = dto.Relationship,
                Phone = dto.Phone,
                IsActive = dto.IsActive,
                AffiliateId = dto.AffiliateId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Dependents.Add(dependent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDependent), new { id = dependent.Id }, new DependentDto
            {
                Id = dependent.Id,
                DependentNumber = dependent.DependentNumber,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                Identification = dependent.Identification,
                Relationship = dependent.Relationship,
                Phone = dependent.Phone,
                IsActive = dependent.IsActive,
                CreatedAt = dependent.CreatedAt,
                AffiliateId = dependent.AffiliateId,
                AffiliateName = $"{affiliate.FirstName} {affiliate.LastName}",
                AffiliateNumber = affiliate.AffiliateNumber
            });
        }

        // PUT: api/Dependent/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDependent(int id, CreateDependentDto dto)
        {
            var dependent = await _context.Dependents.FindAsync(id);

            if (dependent == null)
                return NotFound();

            dependent.FirstName = dto.FirstName;
            dependent.LastName = dto.LastName;
            dependent.Identification = dto.Identification;
            dependent.Relationship = dto.Relationship;
            dependent.Phone = dto.Phone;
            dependent.IsActive = dto.IsActive;
            dependent.AffiliateId = dto.AffiliateId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Dependent/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDependent(int id)
        {
            var dependent = await _context.Dependents.FindAsync(id);

            if (dependent == null)
                return NotFound();

            _context.Dependents.Remove(dependent);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}