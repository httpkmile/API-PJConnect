// src/WebApiPJConnect.Infra.Data/Repositories/CompanyRepository.cs
using Microsoft.EntityFrameworkCore;
using WebApiPJConnect.Domain.Entities;
using WebApiPJConnect.Domain.Enums;
using WebApiPJConnect.Domain.Interfaces;
using WebApiPJConnect.Domain.Shared;
using WebApiPJConnect.Domain.Users;
using WebApiPJConnect.Infra.Data.Context;

namespace WebApiPJConnect.Infra.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _ctx;
        public CompanyRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task AddAsync(Company company, CancellationToken ct)
        {
            await _ctx.Companies.AddAsync(company, ct);
            await _ctx.SaveChangesAsync(ct);
        }

        public Task<Company?> GetByIdAsync(Guid id, CancellationToken ct) =>
            _ctx.Companies
                .Include(c => c.Partners)
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<Company?> GetByCnpjAsync(string cnpj, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return null;

            var digits = new string(cnpj.Where(char.IsDigit).ToArray());

            return await _ctx.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => EF.Property<string>(c.Cnpj, "Value") == digits, ct);

        }


        public async Task UpdateAsync(Company company, CancellationToken ct)
        {
            // Atualiza apenas scalars (evita mexer nas coleções)
            var db = await _ctx.Companies.FirstOrDefaultAsync(c => c.Id == company.Id, ct);
            if (db is null) return;

            db.Update(company.TradeName, company.LegalName, company.Address, company.Type);
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Company company, CancellationToken ct)
        {
            if (company is null) return;

            if (_ctx.Entry(company).State == EntityState.Detached)
                _ctx.Attach(company);

            _ctx.Companies.Remove(company);
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<Company>> QueryByTypeAsync(CompanyType type, CancellationToken ct) =>
            await _ctx.Companies
                .Where(c => c.Type == type)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IReadOnlyList<CompanyUser>> QueryUsersByProfileAsync(UserProfile profile, CancellationToken ct) =>
            await _ctx.Users
                .AsNoTracking()
                .Where(u => u.Profile == profile)
                .OrderBy(u => u.Name)
                .ToListAsync(ct);

     

        public async Task<CompanyUser?> AddUserAsync(Guid companyId, CompanyUser user, CancellationToken ct)
        {
            // Garante que a empresa existe
            var existsCompany = await _ctx.Companies.AnyAsync(c => c.Id == companyId, ct);
            if (!existsCompany) return null;

            // Regra: CPF único por empresa
            if (await ExistsUserCpfAsync(companyId, user.Cpf, ct))
                throw new InvalidOperationException("Já existe usuário com este CPF nesta empresa.");

            await _ctx.Users.AddAsync(user, ct);
            await _ctx.SaveChangesAsync(ct);
            return user;
        }

        public Task<bool> ExistsUserCpfAsync(Guid companyId, Cpf cpf, CancellationToken ct) =>
            _ctx.Users.AnyAsync(u => u.CompanyId == companyId && u.Cpf == new Cpf(cpf.Value), ct);

        public async Task<Partner?> AddPartnerAsync(Guid companyId, Partner partner, CancellationToken ct)
        {
            var existsCompany = await _ctx.Companies.AnyAsync(c => c.Id == companyId, ct);
            if (!existsCompany) return null;

            // Define FK shadow se necessário
            var fk = _ctx.Entry(partner).Property<Guid?>("CompanyId");
            if (fk != null && !fk.CurrentValue.HasValue)
                fk.CurrentValue = companyId;

            await _ctx.Partners.AddAsync(partner, ct);
            await _ctx.SaveChangesAsync(ct);
            return partner;
        }
    }
}
