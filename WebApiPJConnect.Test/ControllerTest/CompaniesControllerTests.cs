using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiPJConnect.API.Controllers;
using WebApiPJConnect.Application.DTOs.Companies;
using WebApiPJConnect.Application.Interfaces;
using WebApiPJConnect.Domain.Enums;
using Xunit;

namespace WebApiPJConnect.Tests.API
{
    public class CompaniesControllerTests
    {
        [Fact]
        public async Task Create_Should_Return_Created()
        {
            var svc = new Mock<ICompanyService>();
            var created = new CompanyResponseDto { Id = Guid.NewGuid(), TradeName = "X" };
            svc.Setup(s => s.AddAsync(It.IsAny<CreateCompanyRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(created);

            var ctrl = new CompaniesController(svc.Object);
            var result = await ctrl.Create(new CreateCompanyRequestDto(), CancellationToken.None);

            var action = result.Result as CreatedAtActionResult;
            action.Should().NotBeNull();
            ((CompanyResponseDto)action!.Value!).Id.Should().Be(created.Id);
        }

        [Fact]
        public async Task Create_Should_Return_Conflict_On_Duplicate()
        {
            var svc = new Mock<ICompanyService>();
            svc.Setup(s => s.AddAsync(It.IsAny<CreateCompanyRequestDto>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new InvalidOperationException("Já existe empresa com este CNPJ."));

            var ctrl = new CompaniesController(svc.Object);
            var result = await ctrl.Create(new CreateCompanyRequestDto(), CancellationToken.None);

            result.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task ByType_Should_Return_Ok_List()
        {
            var svc = new Mock<ICompanyService>();
            svc.Setup(s => s.QueryByTypeAsync(CompanyType.MEI, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<CompanyResponseDto> { new CompanyResponseDto { TradeName = "A" } });

            var ctrl = new CompaniesController(svc.Object);
            var res = await ctrl.ByType(CompanyType.MEI, CancellationToken.None);

            (res.Result as OkObjectResult)!.Value.Should().BeAssignableTo<IReadOnlyList<CompanyResponseDto>>();
        }
    }
}
