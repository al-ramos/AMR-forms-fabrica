using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RDS.Forms.Fabrica.Domain.Interfaces;
using RDS.Forms.Fabrica.Infrastructure.Data;
using RDS.Forms.Fabrica.Infrastructure.Repositories;

namespace RDS.Forms.Fabrica.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<RdsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("RdsFormasFabrica"),
                sql => sql.MigrationsAssembly(typeof(RdsDbContext).Assembly.FullName)
            )
        );

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RdsDbContext>());
        services.AddScoped<IFichaRepository, FichaRepository>();
        services.AddScoped<IFilialRepository, FilialRepository>();
        services.AddScoped<ITipoOperacaoRepository, TipoOperacaoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<INotaFiscalRepository, NotaFiscalRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IFichaBalancaRepository, FichaBalancaRepository>();
        services.AddScoped<IFichaLoadDetalheRepository, FichaLoadDetalheRepository>();
        services.AddScoped<ILogSistemaRepository, LogSistemaRepository>();
        services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
        services.AddScoped<IBusinessUnitRepository, BusinessUnitRepository>();
        services.AddScoped<INotaFiscalDetalheRepository, NotaFiscalDetalheRepository>();

        return services;
    }
}
