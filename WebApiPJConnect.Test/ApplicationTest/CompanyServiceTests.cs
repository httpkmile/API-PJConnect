using FluentAssertions;
using Moq;
using WebApiPJConnect.Application.DTOs.Companies;
using WebApiPJConnect.Application.Services;
using WebApiPJConnect.Domain.Entities;
using WebApiPJConnect.Domain.Enums;
using WebApiPJConnect.Domain.Interfaces;
using WebApiPJConnect.Domain.Shared;
using Xunit;

namespace WebApiPJConnect.Tests.Application
{
    public class CompanyServiceTests
    {
        [Fact]
        public async Task AddAsync_Should_Create_When_Cnpj_NotExists()
        {
            var repo = new Mock<ICompanyRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetByCnpjAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Company?)null);
            repo.Setup(r => r.AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var svc = new CompanyService(repo.Object);

            var req = new CreateCompanyRequestDto
            {
                TradeName = "Padaria",
                LegalName = "Padaria ME",
                Cnpj = "11222333000181",
                Street = "Av",
                Number = "1",
                City = "SP",
                State = "SP",
                ZipCode = "01001000",
                Type = CompanyType.MEI
            };

            var res = await svc.AddAsync(req, CancellationToken.None);

            res.TradeName.Should().Be("Padaria");
            repo.VerifyAll();
        }

        [Fact]
        public async Task AddAsync_Should_Throw_When_Cnpj_Exists()
        {
            var repo = new Mock<ICompanyRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetByCnpjAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Company("X", "Y", new Cnpj("11222333000181"),
                    new Address("a", "1", "c", "SP", "01001000"), CompanyType.MEI));

            var svc = new CompanyService(repo.Object);

            var req = new CreateCompanyRequestDto
            {
                TradeName = "X",
                LegalName = "Y",
                Cnpj = "11222333000181",
                Street = "Av",
                Number = "1",
                City = "SP",
                State = "SP",
                ZipCode = "01001000",
                Type = CompanyType.MEI
            };

            await FluentActions.Awaiting(() => svc.AddAsync(req, CancellationToken.None))
                .Should().ThrowAsync<InvalidOperationException>();
            repo.VerifyAll();
        }
    }
}
