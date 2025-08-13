using System.Data;
using WebApiPJConnect.Application.DTOs.Companies;
using WebApiPJConnect.Application.Interfaces;
using WebApiPJConnect.Domain.Entities;
using WebApiPJConnect.Domain.Enums;
using WebApiPJConnect.Domain.Interfaces;
using WebApiPJConnect.Domain.Shared;

namespace WebApiPJConnect.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repo;
        public CompanyService(ICompanyRepository repo) => _repo = repo;

        public async Task<CompanyResponseDto> AddAsync(CreateCompanyRequestDto req, CancellationToken ct)
        {
            // evita violar índice único (IX_Companies_Cnpj)
            var existing = await _repo.GetByCnpjAsync(req.Cnpj, ct);
            if (existing is not null)
                throw new InvalidOperationException("Já existe empresa com este CNPJ.");

            var company = new Company(
                req.TradeName,
                req.LegalName,
                new Cnpj(req.Cnpj),
                new Address(req.Street, req.Number, req.City, req.State, req.ZipCode),
                req.Type
            );

            if (req.Partners is not null)
                foreach (var p in req.Partners)
                    company.AddPartner(new Partner(p.Name, new Cpf(p.Cpf)));

            await _repo.AddAsync(company, ct);
            return Map(company);
        }

        public async Task<CompanyResponseDto?> GetAsync(Guid id, CancellationToken ct)
        {
            var c = await _repo.GetByIdAsync(id, ct);
            return c is null ? null : Map(c);
        }

        public async Task<CompanyResponseDto?> UpdateAsync(Guid id, UpdateCompanyRequestDto req, CancellationToken ct)
        {
            var c = await _repo.GetByIdAsync(id, ct);
            if (c is null) return null;

            c.Update(
                req.TradeName,
                req.LegalName,
                new Address(req.Street, req.Number, req.City, req.State, req.ZipCode),
                req.Type
            );

            await _repo.UpdateAsync(c, ct);
            return Map(c);
        }

        public async Task<bool> InactivateAsync(Guid id, CancellationToken ct)
        {
            var c = await _repo.GetByIdAsync(id, ct);
            if (c is null) return false;

            c.Inactivate();
            await _repo.UpdateAsync(c, ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var c = await _repo.GetByIdAsync(id, ct);
            if (c is null) return false;

            await _repo.DeleteAsync(c, ct); // usa o overload que recebe a entidade
            return true;
        }

        public async Task<IReadOnlyList<CompanyResponseDto>> QueryByTypeAsync(CompanyType type, CancellationToken ct)
        {
            var list = await _repo.QueryByTypeAsync(type, ct);
            return list.Select(Map).ToList();
        }

        private static CompanyResponseDto Map(Company c) => new()
        {
            Id = c.Id,
            TradeName = c.TradeName,
            LegalName = c.LegalName,
            Cnpj = c.Cnpj.Value,
            Type = c.Type,
            Active = c.Active
        };
    }
}
