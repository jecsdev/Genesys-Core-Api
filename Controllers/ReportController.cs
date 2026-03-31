using ClosedXML.Excel;
using Genesis_Core_Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genesis_Core_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/report/companies
        [HttpGet("companies")]
        [Authorize(Roles = "Administrador, Contabilidad")]
        public async Task<IActionResult> GetCompaniesReport()
        {
            try
            {
                var companies = await _context.Companies
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Empresas");

                ws.Cell(1, 1).Value = "ID";
                ws.Cell(1, 2).Value = "Nombre";
                ws.Cell(1, 3).Value = "RNC";
                ws.Cell(1, 4).Value = "Teléfono";
                ws.Cell(1, 5).Value = "Dirección";
                ws.Cell(1, 6).Value = "Estado";
                ws.Cell(1, 7).Value = "Fecha Registro";

                var headerRange = ws.Range(1, 1, 1, 7);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#F0A500");
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                for (int i = 0; i < companies.Count; i++)
                {
                    var c = companies[i];
                    var row = i + 2;
                    ws.Cell(row, 1).Value = c.Id;
                    ws.Cell(row, 2).Value = c.Name;
                    ws.Cell(row, 3).Value = c.Rnc;
                    ws.Cell(row, 4).Value = c.Phone;
                    ws.Cell(row, 5).Value = c.Address;
                    ws.Cell(row, 6).Value = c.IsActive ? "Activa" : "Inactiva";
                    ws.Cell(row, 7).Value = c.CreatedAt.ToString("dd/MM/yyyy");

                    if (i % 2 == 1)
                        ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#F9FAFB");
                }

                ws.Columns().AdjustToContents();

                byte[] content;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    content = stream.ToArray();
                }
                workbook.Dispose();

                return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"empresas_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, detail = ex.StackTrace });
            }
        }

        // GET: api/report/affiliates
        [HttpGet("affiliates")]
        [Authorize(Roles = "Administrador, Contabilidad")]
        public async Task<IActionResult> GetAffiliatesReport()
        {
            try
            {
                var affiliates = await _context.Affiliates
                    .Include(a => a.Company)
                    .Include(a => a.Dependents)
                    .OrderBy(a => a.LastName)
                    .ToListAsync();

                var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Titulares");

                ws.Cell(1, 1).Value = "Nº Afiliado";
                ws.Cell(1, 2).Value = "Nombre";
                ws.Cell(1, 3).Value = "Apellido";
                ws.Cell(1, 4).Value = "Cédula";
                ws.Cell(1, 5).Value = "Email";
                ws.Cell(1, 6).Value = "Teléfono";
                ws.Cell(1, 7).Value = "Empresa";
                ws.Cell(1, 8).Value = "Cargo";
                ws.Cell(1, 9).Value = "Dependientes";
                ws.Cell(1, 10).Value = "Estado";
                ws.Cell(1, 11).Value = "Fecha Registro";

                var headerRange = ws.Range(1, 1, 1, 11);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#F0A500");
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                for (int i = 0; i < affiliates.Count; i++)
                {
                    var a = affiliates[i];
                    var row = i + 2;
                    ws.Cell(row, 1).Value = a.AffiliateNumber;
                    ws.Cell(row, 2).Value = a.FirstName;
                    ws.Cell(row, 3).Value = a.LastName;
                    ws.Cell(row, 4).Value = a.Identification;
                    ws.Cell(row, 5).Value = a.Email;
                    ws.Cell(row, 6).Value = a.Phone;
                    ws.Cell(row, 7).Value = a.Company?.Name ?? "";
                    ws.Cell(row, 8).Value = a.Position;
                    ws.Cell(row, 9).Value = a.Dependents.Count;
                    ws.Cell(row, 10).Value = a.IsActive ? "Activo" : "Inactivo";
                    ws.Cell(row, 11).Value = a.CreatedAt.ToString("dd/MM/yyyy");

                    if (i % 2 == 1)
                        ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#F9FAFB");
                }

                ws.Columns().AdjustToContents();

                byte[] content;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    content = stream.ToArray();
                }
                workbook.Dispose();

                return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"titulares_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, detail = ex.StackTrace });
            }
        }

        // GET: api/report/dependents
        [HttpGet("dependents")]
        [Authorize(Roles = "Administrador, Contabilidad")]
        public async Task<IActionResult> GetDependentsReport()
        {
            try
            {
                var dependents = await _context.Dependents
                    .Include(d => d.Affiliate)
                        .ThenInclude(a => a!.Company)
                    .OrderBy(d => d.LastName)
                    .ToListAsync();

                var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Dependientes");

                ws.Cell(1, 1).Value = "Nº Dependiente";
                ws.Cell(1, 2).Value = "Nombre";
                ws.Cell(1, 3).Value = "Apellido";
                ws.Cell(1, 4).Value = "Cédula";
                ws.Cell(1, 5).Value = "Parentesco";
                ws.Cell(1, 6).Value = "Teléfono";
                ws.Cell(1, 7).Value = "Titular";
                ws.Cell(1, 8).Value = "Nº Afiliado";
                ws.Cell(1, 9).Value = "Empresa";
                ws.Cell(1, 10).Value = "Estado";
                ws.Cell(1, 11).Value = "Fecha Registro";

                var headerRange = ws.Range(1, 1, 1, 11);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#F0A500");
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                for (int i = 0; i < dependents.Count; i++)
                {
                    var d = dependents[i];
                    var row = i + 2;
                    ws.Cell(row, 1).Value = d.DependentNumber;
                    ws.Cell(row, 2).Value = d.FirstName;
                    ws.Cell(row, 3).Value = d.LastName;
                    ws.Cell(row, 4).Value = d.Identification;
                    ws.Cell(row, 5).Value = d.Relationship;
                    ws.Cell(row, 6).Value = d.Phone;
                    ws.Cell(row, 7).Value = d.Affiliate != null
                        ? $"{d.Affiliate.FirstName} {d.Affiliate.LastName}" : "";
                    ws.Cell(row, 8).Value = d.Affiliate?.AffiliateNumber ?? "";
                    ws.Cell(row, 9).Value = d.Affiliate?.Company?.Name ?? "";
                    ws.Cell(row, 10).Value = d.IsActive ? "Activo" : "Inactivo";
                    ws.Cell(row, 11).Value = d.CreatedAt.ToString("dd/MM/yyyy");

                    if (i % 2 == 1)
                        ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#F9FAFB");
                }

                ws.Columns().AdjustToContents();

                byte[] content;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    content = stream.ToArray();
                }
                workbook.Dispose();

                return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"dependientes_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, detail = ex.StackTrace });
            }
        }

        // GET: api/report/affiliates-by-company
        [HttpGet("affiliates-by-company")]
        [Authorize(Roles = "Administrador, Contabilidad")]
        public async Task<IActionResult> GetAffiliatesByCompanyReport()
        {
            try
            {
                var companies = await _context.Companies
                    .Include(c => c.Affiliates)
                        .ThenInclude(a => a.Dependents)
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Titulares por Empresa");

                ws.Cell(1, 1).Value = "Empresa";
                ws.Cell(1, 2).Value = "Nº Afiliado";
                ws.Cell(1, 3).Value = "Nombre Completo";
                ws.Cell(1, 4).Value = "Cédula";
                ws.Cell(1, 5).Value = "Cargo";
                ws.Cell(1, 6).Value = "Dependientes";
                ws.Cell(1, 7).Value = "Estado";

                var headerRange = ws.Range(1, 1, 1, 7);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#F0A500");
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int row = 2;
                foreach (var company in companies)
                {
                    foreach (var a in company.Affiliates.OrderBy(a => a.LastName))
                    {
                        ws.Cell(row, 1).Value = company.Name;
                        ws.Cell(row, 2).Value = a.AffiliateNumber;
                        ws.Cell(row, 3).Value = $"{a.FirstName} {a.LastName}";
                        ws.Cell(row, 4).Value = a.Identification;
                        ws.Cell(row, 5).Value = a.Position;
                        ws.Cell(row, 6).Value = a.Dependents.Count;
                        ws.Cell(row, 7).Value = a.IsActive ? "Activo" : "Inactivo";

                        if (row % 2 == 1)
                            ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#F9FAFB");
                        row++;
                    }
                }

                ws.Columns().AdjustToContents();

                byte[] content;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    content = stream.ToArray();
                }
                workbook.Dispose();

                return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"titulares_por_empresa_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, detail = ex.StackTrace });
            }
        }
    }
}