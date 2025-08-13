using WebApiPJConnect.Application.DTOs.Companies;
using WebApiPJConnect.Domain.Enums;

namespace WebApiPJConnect.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponseDto> AddAsync(CreateCompanyRequestDto req, CancellationToken ct);
        Task<CompanyResponseDto?> GetAsync(Guid id, CancellationToken ct);
        Task<CompanyResponseDto?> UpdateAsync(Guid id, UpdateCompanyRequestDto req, CancellationToken ct);
        Task<bool> InactivateAsync(Guid id, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<CompanyResponseDto>> QueryByTypeAsync(CompanyType type, CancellationToken ct);
    }
}
