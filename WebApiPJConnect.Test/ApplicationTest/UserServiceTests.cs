using FluentAssertions;
using Moq;
using WebApiPJConnect.Application.DTOs.Users;
using WebApiPJConnect.Application.Services;
using WebApiPJConnect.Domain.Enums;
using WebApiPJConnect.Domain.Interfaces;
using WebApiPJConnect.Domain.Shared;
using WebApiPJConnect.Domain.Users;
using Xunit;

namespace WebApiPJConnect.Tests.Application
{
    public class UserServiceTests
    {
        [Fact]
        public async Task AddUserAsync_Should_Insert_When_Not_Duplicate()
        {
            var companyId = Guid.NewGuid();
            var repo = new Mock<ICompanyRepository>(MockBehavior.Strict);
            repo.Setup(r => r.ExistsUserCpfAsync(companyId, It.Is<Cpf>(c => c.Value == "52998224725"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            repo.Setup(r => r.AddUserAsync(companyId, It.IsAny<CompanyUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid _, CompanyUser u, CancellationToken __) => u);

            var svc = new UserService(repo.Object);

            var req = new AddUserRequestDto { Name = "Ana", Cpf = "52998224725", Profile = UserProfile.Agencia };
            var res = await svc.AddUserAsync(companyId, req, CancellationToken.None);

            res.Should().NotBeNull();
            res!.Name.Should().Be("Ana");
            repo.VerifyAll();
        }

        [Fact]
        public async Task AddUserAsync_Should_Throw_When_Duplicate_Cpf_In_Company()
        {
            var companyId = Guid.NewGuid();
            var repo = new Mock<ICompanyRepository>(MockBehavior.Strict);
            repo.Setup(r => r.ExistsUserCpfAsync(companyId, It.IsAny<Cpf>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var svc = new UserService(repo.Object);
            var req = new AddUserRequestDto { Name = "Ana", Cpf = "52998224725", Profile = UserProfile.Agencia };

            await FluentActions.Awaiting(() => svc.AddUserAsync(companyId, req, CancellationToken.None))
                .Should().ThrowAsync<InvalidOperationException>();
            repo.VerifyAll();
        }
    }
}
