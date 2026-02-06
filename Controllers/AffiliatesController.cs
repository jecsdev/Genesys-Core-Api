using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Genesis_Core_Api.Data;
using Genesis_Core_Api.Models;

namespace Genesis_Core_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AffiliatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AffiliatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Affiliates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Affiliate>>> GetAffiliates()
        {
            return await _context.Affiliates.ToListAsync();
        }

        // GET: api/Affiliates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Affiliate>> GetAffiliate(int id)
        {
            var affiliate = await _context.Affiliates.FindAsync(id);

            if (affiliate == null)
            {
                return NotFound();
            }

            return affiliate;
        }

        // PUT: api/Affiliates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAffiliate(int id, Affiliate affiliate)
        {
            if (id != affiliate.Id)
            {
                return BadRequest();
            }

            _context.Entry(affiliate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AffiliateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Affiliates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Affiliate>> PostAffiliate(Affiliate affiliate)
        {
            _context.Affiliates.Add(affiliate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAffiliate", new { id = affiliate.Id }, affiliate);
        }

        // DELETE: api/Affiliates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAffiliate(int id)
        {
            var affiliate = await _context.Affiliates.FindAsync(id);
            if (affiliate == null)
            {
                return NotFound();
            }

            _context.Affiliates.Remove(affiliate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AffiliateExists(int id)
        {
            return _context.Affiliates.Any(e => e.Id == id);
        }
    }
}
