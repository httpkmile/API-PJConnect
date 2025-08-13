using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiPJConnect.API.Controllers;
using WebApiPJConnect.Application.DTOs.Users;
using WebApiPJConnect.Application.Interfaces;
using WebApiPJConnect.Domain.Enums;
using WebApiPJConnect.Domain.Users;
using Xunit;

namespace WebApiPJConnect.Tests.API
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task Add_Should_Return_Created_When_Ok()
        {
            // Arrange
            var svc = new Mock<IUserService>();
            var expectedDto = new UserResponseDto
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                Name = "Ana",
                Cpf = "52998224725",
                Profile = UserProfile.Agencia
            };

            svc.Setup(s => s.AddUserAsync(expectedDto.CompanyId, It.IsAny<AddUserRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(expectedDto);

            var ctrl = new UsersController(svc.Object);

            var request = new AddUserRequestDto
            {
                Name = "Ana",
                Cpf = "52998224725",
                Profile = UserProfile.Agencia
            };

            // Act
            var res = await ctrl.Add(expectedDto.CompanyId, request, CancellationToken.None);

            // Assert
            var createdResult = res.Result as CreatedResult;
            createdResult.Should().NotBeNull("o retorno deve ser 201 Created");
            createdResult!.Value.Should().BeEquivalentTo(expectedDto);
        }



        [Fact]
        public async Task Add_Should_Return_NotFound_When_Company_Not_Exists()
        {
            var svc = new Mock<IUserService>();
            svc.Setup(s => s.AddUserAsync(It.IsAny<Guid>(), It.IsAny<AddUserRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((UserResponseDto?)null);

            var ctrl = new UsersController(svc.Object);
            var res = await ctrl.Add(Guid.NewGuid(), new AddUserRequestDto { Name = "Ana", Cpf = "52998224725", Profile = UserProfile.Agencia }, CancellationToken.None);

            res.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetByProfile_Should_Return_Ok_List()
        {
            var svc = new Mock<IUserService>();
            svc.Setup(s => s.QueryUsersByProfileAsync(UserProfile.Agencia, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<UserResponseDto> { new UserResponseDto { Name = "Ana", Profile = UserProfile.Agencia } });

            var ctrl = new UsersController(svc.Object);
            var res = await ctrl.ByProfile(UserProfile.Agencia, CancellationToken.None);

            (res.Result as OkObjectResult)!.Value.Should().BeAssignableTo<IReadOnlyList<UserResponseDto>>();
        }
    }
}
