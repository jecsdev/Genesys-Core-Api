using Genesis_Core_Api.Data;
using Genesis_Core_Api.Models;
using Genesis_Core_Api.Models.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis_Core_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AffiliateController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AffiliateController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Affiliate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AffiliateDto>>> GetAffiliates()
        {
            return await _context.Affiliates
                .Include(a => a.Company)
                .Include(a => a.Dependents)
                .Select(a => new AffiliateDto
                {
                    Id = a.Id,
                    AffiliateNumber = a.AffiliateNumber,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Identification = a.Identification,
                    Email = a.Email,
                    Phone = a.Phone,
                    Address = a.Address,
                    Position = a.Position,
                    IsActive = a.IsActive,
                    CreatedAt = a.CreatedAt,
                    CompanyId = a.CompanyId,
                    CompanyName = a.Company != null ? a.Company.Name : "",
                    DependentsCount = a.Dependents.Count
                })
                .ToListAsync();
        }

        // GET: api/Affiliate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AffiliateDto>> GetAffiliate(int id)
        {
            var a = await _context.Affiliates
                .Include(a => a.Company)
                .Include(a => a.Dependents)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (a == null)
                return NotFound();

            return new AffiliateDto
            {
                Id = a.Id,
                AffiliateNumber = a.AffiliateNumber,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Identification = a.Identification,
                Email = a.Email,
                Phone = a.Phone,
                Address = a.Address,
                Position = a.Position,
                IsActive = a.IsActive,
                CreatedAt = a.CreatedAt,
                CompanyId = a.CompanyId,
                CompanyName = a.Company != null ? a.Company.Name : "",
                DependentsCount = a.Dependents.Count
            };
        }

        // GET: api/Affiliate/cedula/{identification}
        [HttpGet("cedula/{identification}")]
        public async Task<ActionResult<AffiliateDto>> GetAffiliateByCedula(string identification)
        {
            var a = await _context.Affiliates
                .Include(a => a.Company)
                .Include(a => a.Dependents)
                .FirstOrDefaultAsync(a => a.Identification == identification);

            if (a == null)
                return NotFound(new { message = "No se encontró ningún titular con esa cédula." });

            return new AffiliateDto
            {
                Id = a.Id,
                AffiliateNumber = a.AffiliateNumber,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Identification = a.Identification,
                Email = a.Email,
                Phone = a.Phone,
                Address = a.Address,
                Position = a.Position,
                IsActive = a.IsActive,
                CreatedAt = a.CreatedAt,
                CompanyId = a.CompanyId,
                CompanyName = a.Company != null ? a.Company.Name : "",
                DependentsCount = a.Dependents.Count
            };
        }

        // POST: api/Affiliate
        [HttpPost]
        public async Task<ActionResult<AffiliateDto>> PostAffiliate(CreateAffiliateDto dto)
        {
            // Generar número de afiliado automáticamente
            var count = await _context.Affiliates.CountAsync();
            var affiliateNumber = $"AF-{DateTime.UtcNow.Year}-{String.Format("{0:000}", count + 1)}";

            var affiliate = new Affiliate
            {
                AffiliateNumber = affiliateNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Identification = dto.Identification,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                Position = dto.Position,
                IsActive = dto.IsActive,
                CompanyId = dto.CompanyId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Affiliates.Add(affiliate);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAffiliate), new { id = affiliate.Id }, new AffiliateDto
            {
                Id = affiliate.Id,
                AffiliateNumber = affiliate.AffiliateNumber,
                FirstName = affiliate.FirstName,
                LastName = affiliate.LastName,
                Identification = affiliate.Identification,
                Email = affiliate.Email,
                Phone = affiliate.Phone,
                Address = affiliate.Address,
                Position = affiliate.Position,
                IsActive = affiliate.IsActive,
                CreatedAt = affiliate.CreatedAt,
                CompanyId = affiliate.CompanyId,
                CompanyName = "",
                DependentsCount = 0
            });
        }

        // PUT: api/Affiliate/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAffiliate(int id, CreateAffiliateDto dto)
        {
            var affiliate = await _context.Affiliates.FindAsync(id);

            if (affiliate == null)
                return NotFound();

            affiliate.FirstName = dto.FirstName;
            affiliate.LastName = dto.LastName;
            affiliate.Identification = dto.Identification;
            affiliate.Email = dto.Email;
            affiliate.Phone = dto.Phone;
            affiliate.Address = dto.Address;
            affiliate.Position = dto.Position;
            affiliate.IsActive = dto.IsActive;
            affiliate.CompanyId = dto.CompanyId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Affiliate/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAffiliate(int id)
        {
            var affiliate = await _context.Affiliates.FindAsync(id);

            if (affiliate == null)
                return NotFound();

            _context.Affiliates.Remove(affiliate);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}