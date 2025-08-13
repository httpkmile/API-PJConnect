using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiPJConnect.Application.Interfaces;
using WebApiPJConnect.Application.Services;
using WebApiPJConnect.Domain.Interfaces;
using WebApiPJConnect.Infra.Data.Context;
using WebApiPJConnect.Infra.Data.Repositories;


namespace WebApiPJConnect.Infra.IoC
{
    //teste commit
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName = "Default")
        {
            var cs = configuration.GetConnectionString(connectionStringName)
                     ?? "Server=localhost,1433;Database=PJConnectDev;User Id=sa;Password=Your_strong_Password123;TrustServerCertificate=True";

            services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(cs, sql =>
                {
                    sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    sql.CommandTimeout(60);
                }));

            // Repositórios
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            // Serviços
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
