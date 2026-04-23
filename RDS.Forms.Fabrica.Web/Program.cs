using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    // Esta linha avisa o .NET: "Não exija os objetos de navegação no JSON!"
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RdsFormasFabricaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RdsFormasFabrica")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Antes do builder.Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // porta do Vite
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Depois do app = builder.Build(), antes do app.MapControllers()
app.UseCors("DevPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();