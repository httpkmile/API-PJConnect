using WebApiPJConnect.Domain.Enums;
using WebApiPJConnect.Domain.Shared;

namespace WebApiPJConnect.Domain.Users
{
    public class CompanyUser
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public Cpf Cpf { get; private set; }            // Value Object (class)
        public UserProfile Profile { get; private set; } // Enum
        public Guid CompanyId { get; private set; }
        private CompanyUser() { }
        public CompanyUser(Guid companyId, string name, Cpf cpf, UserProfile profile)
        {
            CompanyId = companyId; Name = name; Cpf = cpf; Profile = profile;
        }
    }
}
