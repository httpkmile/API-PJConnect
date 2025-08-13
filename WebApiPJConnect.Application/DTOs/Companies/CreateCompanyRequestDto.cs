using WebApiPJConnect.Domain.Enums;

namespace WebApiPJConnect.Application.DTOs.Companies
{
    public class CreateCompanyRequestDto
    {
        public string TradeName { get; set; } = default!;
        public string LegalName { get; set; } = default!;
        public string Cnpj { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Number { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public CompanyType Type { get; set; }
        public List<PartnerDto> Partners { get; set; } = new();
    }
}
