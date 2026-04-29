using MediatR;

namespace RDS.Forms.Fabrica.Application.Features.Pedidos.Commands;

public record SincronizarPedidosCommand(int CodigoFilial) : IRequest<SincronizarPedidosResult>;

public record SincronizarPedidosResult(int Inseridos, int Atualizados, int Ignorados);
