using WebApiPJConnect.Domain.Enums;
using WebApiPJConnect.Domain.Shared;
using WebApiPJConnect.Domain.Users;


namespace WebApiPJConnect.Domain.Entities
{
    public class Company
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string TradeName { get; private set; }
        public string LegalName { get; private set; }
        public Cnpj Cnpj { get; private set; }          // Value Object (class)
        public Address Address { get; private set; }    // Value Object (class)
        public CompanyType Type { get; private set; }   // Enum
        public bool Active { get; private set; } = true;

        private readonly List<Partner> _partners = new();      // Partner é ENTIDADE
        public IReadOnlyCollection<Partner> Partners => _partners.AsReadOnly();

        private readonly List<CompanyUser> _users = new();     // CompanyUser é ENTIDADE
        public IReadOnlyCollection<CompanyUser> Users => _users.AsReadOnly();

        private Company() { }
        public Company(string tradeName, string legalName, Cnpj cnpj, Address addr, CompanyType type)
        {
            TradeName = tradeName; LegalName = legalName; Cnpj = cnpj; Address = addr; Type = type;
        }

        public void AddPartner(Partner p) => _partners.Add(p);
        public void AddUser(CompanyUser u)
        {
            if (_users.Any(x => x.Cpf.Value == u.Cpf.Value))
                throw new InvalidOperationException("Já existe usuário com este CPF nesta empresa.");
            _users.Add(u);
        }
        public void Update(string trade, string legal, Address addr, CompanyType type)
        {
            TradeName = trade; LegalName = legal; Address = addr; Type = type;
        }
        public void Inactivate() => Active = false;
    }
}
