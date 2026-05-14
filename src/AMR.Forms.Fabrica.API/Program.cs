using AMR.Forms.Fabrica.API.Services;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.ExternalServices;
using AMR.Forms.Fabrica.Application;
using AMR.Forms.Fabrica.Infrastructure;
using AMR.Forms.Fabrica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Serviços ──────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AMR Forms Fábrica API", Version = "v1" });
});

// Clean Architecture layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddHttpClient<IErpHttpClient, ErpHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ErpCore:BaseUrl"] ?? "http://localhost:5000");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IFinanceiroHttpClient, FinanceiroHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Financeiro:BaseUrl"] ?? "http://localhost:5015");
    client.Timeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddHostedService<SincronizacaoPedidosService>();

var app = builder.Build();

// ── Auto Migration + Seed (SQLite) ───────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RdsDbContext>();
    db.Database.Migrate();

    // Seed de filiais padrão se a tabela estiver vazia
    if (!db.Filiais.Any())
    {
        db.Filiais.AddRange(
            new Filial(1, "Filial 01 — Matriz",    null, null),
            new Filial(2, "Filial 02 — Fábrica",   null, null),
            new Filial(3, "Filial 03 — Depósito",  null, null)
        );
        db.SaveChanges();
        app.Logger.LogInformation("Seed: 3 filiais padrão criadas.");
    }
}

// ── Pipeline ──────────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReact");
app.UseAuthorization();
app.MapControllers();

app.Run();