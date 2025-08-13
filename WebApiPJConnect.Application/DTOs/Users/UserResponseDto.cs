using WebApiPJConnect.Domain.Enums;

namespace WebApiPJConnect.Application.DTOs.Users
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Cpf { get; set; } = default!;
        public UserProfile Profile { get; set; }
        public Guid CompanyId { get; set; }
    }
}
