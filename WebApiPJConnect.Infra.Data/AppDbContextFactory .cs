using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebApiPJConnect.Infra.Data.Context;


namespace WebApiPJConnect.Infra.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var conn = "Server=localhost;Database=PJConnectDB;User Id=sa;Password=SuaSenha;";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(conn, sql =>
                {
                    sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    sql.EnableRetryOnFailure();
                })
                .Options;

            return new AppDbContext(options);
        }
    }
}
