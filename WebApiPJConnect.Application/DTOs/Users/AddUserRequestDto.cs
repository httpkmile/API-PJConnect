using WebApiPJConnect.Domain.Enums;

namespace WebApiPJConnect.Application.DTOs.Users
{
    public class AddUserRequestDto
    {
        public string Name { get; set; } = default!;
        public string Cpf { get; set; } = default!;
        public UserProfile Profile { get; set; }
    }
}
