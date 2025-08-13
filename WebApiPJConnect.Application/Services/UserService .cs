// WebApiPJConnect.Application/Services/UserService.cs
using WebApiPJConnect.Application.DTOs.Users;
using WebApiPJConnect.Application.Interfaces;
using WebApiPJConnect.Domain.Shared;
using WebApiPJConnect.Domain.Users;
using WebApiPJConnect.Domain.Interfaces;
using WebApiPJConnect.Domain.Enums;

public class UserService : IUserService
{
    private readonly ICompanyRepository _companies;

    public UserService(ICompanyRepository companies) => _companies = companies;

    public async Task<UserResponseDto?> AddUserAsync(Guid companyId, AddUserRequestDto req, CancellationToken ct)
    {
        var cpf = new Cpf(req.Cpf);

        // regra rápida via repo (evita carregar coleção)
        if (await _companies.ExistsUserCpfAsync(companyId, cpf, ct))
            throw new InvalidOperationException("Já existe usuário com este CPF nesta empresa.");

        var user = new CompanyUser(companyId, req.Name, cpf, req.Profile);

        var inserted = await _companies.AddUserAsync(companyId, user, ct);
        if (inserted is null) return null;

        return new UserResponseDto
        {
            Id = inserted.Id,
            CompanyId = inserted.CompanyId,
            Name = inserted.Name,
            Cpf = inserted.Cpf.Value,
            Profile = inserted.Profile
        };
    }

    public async Task<IReadOnlyList<UserResponseDto>> QueryUsersByProfileAsync(UserProfile profile, CancellationToken ct)
    {
        var items = await _companies.QueryUsersByProfileAsync(profile, ct);
        return items.Select(u => new UserResponseDto
        {
            Id = u.Id,
            CompanyId = u.CompanyId,
            Name = u.Name,
            Cpf = u.Cpf.Value,   // VO -> string (tem ValueConverter no mapeamento)
            Profile = u.Profile
        }).ToList();
    }
}
