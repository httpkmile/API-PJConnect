using WebApiPJConnect.Domain.Enums;

namespace WebApiPJConnect.Application.DTOs.Companies
{
    public class CompanyResponseDto
    {
        public Guid Id { get; set; }
        public string TradeName { get; set; } = default!;
        public string LegalName { get; set; } = default!;
        public string Cnpj { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public CompanyType Type { get; set; }
        public bool Active { get; set; }
    }
}
