using Genesis_Core_Api.Data;
using Genesis_Core_Api.Models;
using Genesis_Core_Api.Models.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis_Core_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AffiliatePaymentDto>>> GetPayments()
        {
            return await _context.AffiliatePayments
                .Include(p => p.Affiliate)
                .OrderByDescending(p => p.PaymentDate)
                .Select(p => new AffiliatePaymentDto
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    DueDate = p.DueDate,
                    PaymentMethod = p.PaymentMethod,
                    ReferenceNumber = p.ReferenceNumber,
                    Status = p.Status.ToString(),
                    Notes = p.Notes,
                    AffiliateId = p.AffiliateId,
                    AffiliateName = p.Affiliate != null
                        ? $"{p.Affiliate.FirstName} {p.Affiliate.LastName}" : "",
                    AffiliateNumber = p.Affiliate != null
                        ? p.Affiliate.AffiliateNumber : ""
                })
                .ToListAsync();
        }

        // GET: api/payment/affiliate/5
        [HttpGet("affiliate/{affiliateId}")]
        public async Task<ActionResult<IEnumerable<AffiliatePaymentDto>>> GetPaymentsByAffiliate(int affiliateId)
        {
            return await _context.AffiliatePayments
                .Include(p => p.Affiliate)
                .Where(p => p.AffiliateId == affiliateId)
                .OrderByDescending(p => p.PaymentDate)
                .Select(p => new AffiliatePaymentDto
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    DueDate = p.DueDate,
                    PaymentMethod = p.PaymentMethod,
                    ReferenceNumber = p.ReferenceNumber,
                    Status = p.Status.ToString(),
                    Notes = p.Notes,
                    AffiliateId = p.AffiliateId,
                    AffiliateName = p.Affiliate != null
                        ? $"{p.Affiliate.FirstName} {p.Affiliate.LastName}" : "",
                    AffiliateNumber = p.Affiliate != null
                        ? p.Affiliate.AffiliateNumber : ""
                })
                .ToListAsync();
        }

        // GET: api/payment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AffiliatePaymentDto>> GetPayment(int id)
        {
            var payment = await _context.AffiliatePayments
                .Include(p => p.Affiliate)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
                return NotFound();

            return new AffiliatePaymentDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                DueDate = payment.DueDate,
                PaymentMethod = payment.PaymentMethod,
                ReferenceNumber = payment.ReferenceNumber,
                Status = payment.Status.ToString(),
                Notes = payment.Notes,
                AffiliateId = payment.AffiliateId,
                AffiliateName = payment.Affiliate != null
                    ? $"{payment.Affiliate.FirstName} {payment.Affiliate.LastName}" : "",
                AffiliateNumber = payment.Affiliate != null
                    ? payment.Affiliate.AffiliateNumber : ""
            };
        }

        // POST: api/payment
        [HttpPost]
        public async Task<ActionResult<AffiliatePaymentDto>> PostPayment(CreateAffiliatePaymentDto dto)
        {
            var affiliate = await _context.Affiliates.FindAsync(dto.AffiliateId);
            if (affiliate == null)
                return BadRequest(new { message = "Titular no encontrado." });

            var payment = new AffiliatePayment
            {
                Amount = dto.Amount,
                PaymentDate = dto.PaymentDate,
                DueDate = dto.DueDate,
                PaymentMethod = dto.PaymentMethod,
                ReferenceNumber = dto.ReferenceNumber,
                Status = dto.Status,
                Notes = dto.Notes,
                AffiliateId = dto.AffiliateId
            };

            _context.AffiliatePayments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, new AffiliatePaymentDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                DueDate = payment.DueDate,
                PaymentMethod = payment.PaymentMethod,
                ReferenceNumber = payment.ReferenceNumber,
                Status = payment.Status.ToString(),
                Notes = payment.Notes,
                AffiliateId = payment.AffiliateId,
                AffiliateName = $"{affiliate.FirstName} {affiliate.LastName}",
                AffiliateNumber = affiliate.AffiliateNumber
            });
        }

        // PUT: api/payment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, CreateAffiliatePaymentDto dto)
        {
            var payment = await _context.AffiliatePayments.FindAsync(id);

            if (payment == null)
                return NotFound();

            payment.Amount = dto.Amount;
            payment.PaymentDate = dto.PaymentDate;
            payment.DueDate = dto.DueDate;
            payment.PaymentMethod = dto.PaymentMethod;
            payment.ReferenceNumber = dto.ReferenceNumber;
            payment.Status = dto.Status;
            payment.Notes = dto.Notes;
            payment.AffiliateId = dto.AffiliateId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/payment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.AffiliatePayments.FindAsync(id);

            if (payment == null)
                return NotFound();

            _context.AffiliatePayments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/payment/account-status/5
        [HttpGet("account-status/{affiliateId}")]
        public async Task<IActionResult> GetAccountStatus(int affiliateId)
        {
            var affiliate = await _context.Affiliates
                .Include(a => a.ServicePlan)
                    .ThenInclude(p => p!.Benefits)
                .Include(a => a.Dependents)
                .Include(a => a.Payments)
                .FirstOrDefaultAsync(a => a.Id == affiliateId);

            if (affiliate == null)
                return NotFound();

            // Calcular monto mensual
            var extraDependents = Math.Max(0, affiliate.Dependents.Count - (affiliate.ServicePlan?.IncludedDependents ?? 4));
            var extraCost = extraDependents * (affiliate.ServicePlan?.ExtraDependentPrice ?? 0);
            var monthlyAmount = (affiliate.ServicePlan?.BasePrice ?? 0) + extraCost;

            // Último pago
            var lastPayment = affiliate.Payments.OrderByDescending(p => p.PaymentDate).FirstOrDefault();

            // Pagos pendientes
            var pendingPayments = affiliate.Payments.Count(p => p.Status == PaymentStatus.Pending);
            var overduePayments = affiliate.Payments.Count(p => p.Status == PaymentStatus.Overdue);

            return Ok(new
            {
                affiliateId = affiliate.Id,
                affiliateName = $"{affiliate.FirstName} {affiliate.LastName}",
                affiliateNumber = affiliate.AffiliateNumber,
                plan = affiliate.ServicePlan == null ? null : new
                {
                    id = affiliate.ServicePlan.Id,
                    name = affiliate.ServicePlan.Name,
                    basePrice = affiliate.ServicePlan.BasePrice,
                    includedDependents = affiliate.ServicePlan.IncludedDependents,
                    extraDependentPrice = affiliate.ServicePlan.ExtraDependentPrice,
                    benefits = affiliate.ServicePlan.Benefits.Select(b => b.Description).ToList()
                },
                dependentsCount = affiliate.Dependents.Count,
                extraDependents,
                extraCost,
                monthlyAmount,
                lastPaymentDate = lastPayment?.PaymentDate,
                lastPaymentStatus = lastPayment?.Status.ToString(),
                pendingPayments,
                overduePayments,
                accountStatus = overduePayments > 0 ? "Atrasado" : pendingPayments > 0 ? "Pendiente" : "Al día",
                recentPayments = affiliate.Payments
                    .OrderByDescending(p => p.PaymentDate)
                    .Take(5)
                    .Select(p => new
                    {
                        id = p.Id,
                        amount = p.Amount,
                        paymentDate = p.PaymentDate,
                        dueDate = p.DueDate,
                        status = p.Status.ToString(),
                        paymentMethod = p.PaymentMethod,
                        referenceNumber = p.ReferenceNumber
                    }).ToList()
            });
        }
    }
}