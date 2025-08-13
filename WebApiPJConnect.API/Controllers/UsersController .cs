using Microsoft.AspNetCore.Mvc;
using WebApiPJConnect.Application.DTOs.Users;
using WebApiPJConnect.Application.Interfaces;
using WebApiPJConnect.Domain.Enums;

namespace WebApiPJConnect.API.Controllers
{

    [ApiController]
    [Route("api/users")]

    public class UsersController : ControllerBase
    {

        private readonly IUserService _users;
        public UsersController(IUserService users) => _users = users;

        [HttpPost("{companyId:guid}")]
        public async Task<ActionResult<UserResponseDto>> Add(Guid companyId, [FromBody] AddUserRequestDto req, CancellationToken ct)
        {
            try
            {
                var user = await _users.AddUserAsync(companyId, req, ct);
                if (user is null)
                    return NotFound();

                // 201 Created com o DTO no corpo
                return Created(string.Empty, user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("by-profile/{profile}")]
        public async Task<ActionResult<IReadOnlyList<UserResponseDto>>> ByProfile(UserProfile profile, CancellationToken ct)
        {
            var list = await _users.QueryUsersByProfileAsync(profile, ct);
            return Ok(list);
        }
    }
}
