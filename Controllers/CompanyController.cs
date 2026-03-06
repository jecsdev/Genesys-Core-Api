using Genesis_Core_Api.Data;
using Genesis_Core_Api.Models;
using Genesis_Core_Api.Models.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis_Core_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CompanyController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            return await _context.Companies
                .Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Rnc = c.Rnc,
                    Phone = c.Phone,
                    Address = c.Address,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        // GET: api/Company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                return NotFound();

            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Rnc = company.Rnc,
                Phone = company.Phone,
                Address = company.Address,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt
            };
        }

        // POST: api/Company
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> PostCompany(CreateCompanyDto dto)
        {
            var company = new Company
            {
                Name = dto.Name,
                Rnc = dto.Rnc,
                Phone = dto.Phone,
                Address = dto.Address,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Rnc = company.Rnc,
                Phone = company.Phone,
                Address = company.Address,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt
            });
        }

        // PUT: api/Company/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, CreateCompanyDto dto)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                return NotFound();

            company.Name = dto.Name;
            company.Rnc = dto.Rnc;
            company.Phone = dto.Phone;
            company.Address = dto.Address;
            company.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Company/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                return NotFound();

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}