using Microsoft.AspNetCore.Mvc;
using WebApiPJConnect.Application.DTOs.Companies;
using WebApiPJConnect.Application.Interfaces;
using WebApiPJConnect.Domain.Enums;

namespace WebApiPJConnect.API.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companies;
        public CompaniesController(ICompanyService companies) => _companies = companies;

        [HttpPost]
        public async Task<ActionResult<CompanyResponseDto>> Create([FromBody] CreateCompanyRequestDto req, CancellationToken ct)
        {
            try
            {
                var created = await _companies.AddAsync(req, ct);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                // conflito por CNPJ duplicado
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CompanyResponseDto>> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var c = await _companies.GetAsync(id, ct);
            return c is null ? NotFound() : Ok(c);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CompanyResponseDto>> Update([FromRoute] Guid id, [FromBody] UpdateCompanyRequestDto req, CancellationToken ct)
        {
            var c = await _companies.UpdateAsync(id, req, ct);
            return c is null ? NotFound() : Ok(c);
        }

        [HttpPost("{id:guid}/inactivate")]
        public async Task<IActionResult> Inactivate([FromRoute] Guid id, CancellationToken ct)
        {
            var ok = await _companies.InactivateAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var ok = await _companies.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpGet("by-type/{type}")]
        public async Task<ActionResult<IReadOnlyList<CompanyResponseDto>>> ByType([FromRoute] CompanyType type, CancellationToken ct)
        {
            var list = await _companies.QueryByTypeAsync(type, ct);
            return Ok(list);
        }
    }
}
