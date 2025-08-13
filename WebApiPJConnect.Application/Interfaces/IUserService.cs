using WebApiPJConnect.Application.DTOs.Users;
using WebApiPJConnect.Domain.Enums;

namespace WebApiPJConnect.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto?> AddUserAsync(Guid companyId, AddUserRequestDto req, CancellationToken ct);
        Task<IReadOnlyList<UserResponseDto>> QueryUsersByProfileAsync(UserProfile profile, CancellationToken ct);
    }
}
