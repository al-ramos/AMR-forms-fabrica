using MediatR;
using Microsoft.Extensions.Logging;
using AMR.Forms.Fabrica.Application.Features.Pedidos.Commands;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Application.Features.Pedidos.Handlers;

public class SincronizarPedidosHandler(
    IErpHttpClient erpClient,
    IPedidoRepository pedidoRepo,
    IUnitOfWork uow,
    ILogger<SincronizarPedidosHandler> logger
) : IRequestHandler<SincronizarPedidosCommand, SincronizarPedidosResult>
{
    public async Task<SincronizarPedidosResult> Handle(
        SincronizarPedidosCommand request, CancellationToken ct)
    {
        int inseridos = 0, ignorados = 0;

        IEnumerable<PedidoErpDto> pedidosErp;
        try
        {
            pedidosErp = await erpClient.ObterPedidosAprovadosAsync(request.CodigoFilial, ct);
        }
        catch (Exception ex)
        {
            logger.LogWarning("AMR Core offline — sincronização adiada. Erro: {Msg}", ex.Message);
            return new SincronizarPedidosResult(0, 0, 0);
        }

        foreach (var dto in pedidosErp)
        {
            try
            {
                var existente = await pedidoRepo.ObterPorIdAsync(dto.Codigo);

                if (existente is null)
                {
                    var pedido = new Pedido(
                        dto.Codigo,
                        dto.CodigoFilial,
                        null,
                        dto.CodigoAddressNumber,
                        dto.DataPedido
                    );

                    foreach (var item in dto.Itens)
                        pedido.AdicionarItem(new PedidoItem(
                            dto.Codigo,
                            dto.CodigoFilial,
                            item.CodigoProduto,
                            item.Quantidade,
                            item.UnidadeMedida
                        ));

                    pedido.MarcarSincronizado();
                    await pedidoRepo.AdicionarAsync(pedido);
                    inseridos++;
                }
                else
                {
                    ignorados++;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Erro ao sincronizar pedido {Cod}: {Msg}", dto.Codigo, ex.Message);
            }
        }

        await uow.SaveChangesAsync(ct);

        logger.LogInformation(
            "Sincronização filial {Filial}: +{I} inseridos | ={G} ignorados",
            request.CodigoFilial, inseridos, ignorados);

        return new SincronizarPedidosResult(inseridos, 0, ignorados);
    }
}
