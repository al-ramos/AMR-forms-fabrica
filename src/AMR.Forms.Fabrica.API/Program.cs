using AMR.Forms.Fabrica.API.Services;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.ExternalServices;
using AMR.Forms.Fabrica.Application;
using AMR.Forms.Fabrica.Infrastructure;
using AMR.Forms.Fabrica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Rate Limiting — 100 req/min por IP ────────────────────────────────────────
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = System.Threading.RateLimiting.PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        System.Threading.RateLimiting.RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new System.Threading.RateLimiting.FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
            }));
    options.OnRejected = async (ctx, ct) =>
    {
        ctx.HttpContext.Response.Headers.RetryAfter = "60";
        await ctx.HttpContext.Response.WriteAsync("Too many requests. Retry after 60 seconds.", ct);
    };
});

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

// Redirect raiz para Swagger em dev (facilita preview e testes locais)
if (app.Environment.IsDevelopment())
    app.MapGet("/", () => Results.Redirect("/swagger/index.html")).ExcludeFromDescription();

app.UseCors("AllowReact");
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();

app.Run();