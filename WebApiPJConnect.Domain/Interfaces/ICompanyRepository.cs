using WebApiPJConnect.Domain.Entities;
using WebApiPJConnect.Domain.Enums;
using WebApiPJConnect.Domain.Shared;
using WebApiPJConnect.Domain.Users;

namespace WebApiPJConnect.Domain.Interfaces
{
    public interface ICompanyRepository
    {
        Task AddAsync(Company company, CancellationToken ct);
        Task<Company?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Company?> GetByCnpjAsync(string cnpj, CancellationToken ct);
        Task UpdateAsync(Company company, CancellationToken ct);
        Task DeleteAsync(Company company, CancellationToken ct);

        Task<IReadOnlyList<Company>> QueryByTypeAsync(CompanyType type, CancellationToken ct);
        Task<IReadOnlyList<CompanyUser>> QueryUsersByProfileAsync(UserProfile profile, CancellationToken ct);
        Task<CompanyUser?> AddUserAsync(Guid companyId, CompanyUser user, CancellationToken ct);
        Task<bool> ExistsUserCpfAsync(Guid companyId, Cpf cpf, CancellationToken ct);
        Task<Partner?> AddPartnerAsync(Guid companyId, Partner partner, CancellationToken ct);
    }
}