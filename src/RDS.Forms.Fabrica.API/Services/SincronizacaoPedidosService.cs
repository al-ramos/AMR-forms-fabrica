using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RDS.Forms.Fabrica.Application.Features.Pedidos.Commands;

namespace RDS.Forms.Fabrica.API.Services;

public class SincronizacaoPedidosService(
    IServiceScopeFactory scopeFactory,
    ILogger<SincronizacaoPedidosService> logger,
    IConfiguration config
) : BackgroundService
{
    private readonly TimeSpan _intervalo =
        TimeSpan.FromMinutes(config.GetValue<int>("ErpCore:IntervaloSincronizacaoMinutos", 5));

    private readonly int[] _filiais =
        config.GetSection("ErpCore:Filiais").Get<int[]>() ?? [1];

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        logger.LogInformation(
            "SincronizacaoPedidosService iniciado — intervalo {Min} min | filiais: {F}",
            _intervalo.TotalMinutes, string.Join(",", _filiais));

        await Task.Delay(TimeSpan.FromSeconds(10), ct);

        while (!ct.IsCancellationRequested)
        {
            foreach (var filial in _filiais)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var result = await mediator.Send(new SincronizarPedidosCommand(filial), ct);

                    logger.LogInformation(
                        "Filial {F} | +{I} | ~{A} | ={G}",
                        filial, result.Inseridos, result.Atualizados, result.Ignorados);
                }
                catch (Exception ex)
                {
                    logger.LogError("Erro na sincronização filial {F}: {Msg}", filial, ex.Message);
                }
            }

            await Task.Delay(_intervalo, ct);
        }
    }
}
