using Microsoft.EntityFrameworkCore;
using WebApiPJConnect.Domain.Entities;
using WebApiPJConnect.Domain.Shared; // Cpf
using WebApiPJConnect.Domain.Users;

namespace WebApiPJConnect.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<CompanyUser> Users => Set<CompanyUser>();
        public DbSet<Partner> Partners => Set<Partner>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            // COMPANY
            b.Entity<Company>(e =>
            {
                e.ToTable("Companies");
                e.HasKey(x => x.Id);

                // CNPJ (owned)
                e.OwnsOne(x => x.Cnpj, c =>
                {
                    c.Property(p => p.Value)
                     .HasColumnName("Cnpj")
                     .HasMaxLength(14)
                     .IsRequired();

                    // índice único no valor do CNPJ
                    c.HasIndex(p => p.Value).IsUnique();
                });

                // Address (owned)
                e.OwnsOne(x => x.Address, a =>
                {
                    a.Property(p => p.Street).HasMaxLength(120).IsRequired();
                    a.Property(p => p.Number).HasMaxLength(20).IsRequired();
                    a.Property(p => p.City).HasMaxLength(60).IsRequired();
                    a.Property(p => p.State).HasMaxLength(2).IsRequired();
                    a.Property(p => p.ZipCode).HasMaxLength(8).IsRequired();
                });

                e.Property(x => x.TradeName).HasMaxLength(120).IsRequired();
                e.Property(x => x.LegalName).HasMaxLength(160).IsRequired();
                e.Property(x => x.Type).IsRequired();
                e.Property(x => x.Active).HasDefaultValue(true);

                // Relacionamentos usando as PROPRIEDADES + Field access (backing field)
                e.HasMany(x => x.Partners)
                 .WithOne()
                 .HasForeignKey("CompanyId")
                 .OnDelete(DeleteBehavior.Cascade);
                e.Navigation(x => x.Partners)
                 .UsePropertyAccessMode(PropertyAccessMode.Field);

                e.HasMany(x => x.Users)
                 .WithOne()
                 .HasForeignKey(u => u.CompanyId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.Navigation(x => x.Users)
                 .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            // PARTNER  (CPF com ValueConverter)
            b.Entity<Partner>(e =>
            {
                e.ToTable("Partners");
                e.HasKey(x => x.Id);

                e.Property(x => x.Name).HasMaxLength(120).IsRequired();

                e.Property(x => x.Cpf)
                 .HasConversion(v => v.Value, v => new Cpf(v))
                 .HasColumnName("Cpf")
                 .HasMaxLength(11)
                 .IsRequired();

                // FK shadow foi criada no HasMany acima; se preferir explícita:
                // e.Property<Guid>("CompanyId").IsRequired();

                // índice único: CompanyId + Cpf (usa nomes de coluna)
                e.HasIndex("CompanyId", "Cpf").IsUnique();
            });

            // COMPANY USER (CPF com ValueConverter)
            b.Entity<CompanyUser>(e =>
            {
                e.ToTable("CompanyUsers");
                e.HasKey(x => x.Id);

                e.Property(x => x.Name).HasMaxLength(120).IsRequired();
                e.Property(x => x.Profile).IsRequired();
                e.Property(x => x.CompanyId).IsRequired();

                e.Property(x => x.Cpf).HasConversion(v => v.Value, v => new Cpf(v))
                 .HasColumnName("Cpf")
                 .HasMaxLength(11)
                 .IsRequired();

                // índice único: CompanyId + Cpf
                e.HasIndex(x => new { x.CompanyId, x.Cpf }).IsUnique();
            });
        }
    }
}
