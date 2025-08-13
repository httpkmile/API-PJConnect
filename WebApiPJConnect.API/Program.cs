using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApiPJConnect.Infra.Data.Context;
using WebApiPJConnect.Infra.IoC;
// using WebApiPJConnect.Infra.Data.Persistence; // DbInitializer (se você criou essa classe)

var builder = WebApplication.CreateBuilder(args);

// Controllers + enums como string no JSON (opcional)
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IoC registra DbContext (UseSqlServer), Repositórios e Services
builder.Services.AddInfrastructure(builder.Configuration);

// CORS opcional
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors();

app.MapControllers();

// --- aplicar migrations + (opcional) seed ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();              // aplica migrations pendentes

    // Se tiver um seeder, descomente:
    // await DbInitializer.InitializeAsync(db);    // seed idempotente (só quando vazio)
}

await app.RunAsync();
