using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class PedidoRepository(RdsDbContext context) : IPedidoRepository
{
    public async Task<Pedido?> ObterPorIdAsync(int id)
        => await context.Pedidos.FirstOrDefaultAsync(p => p.Codigo == id);

    public async Task<IEnumerable<Pedido>> ListarPorFilialAsync(int codigoFilial)
        => await context.Pedidos
            .Where(p => p.CodigoFilial == codigoFilial)
            .OrderByDescending(p => p.DataPedido)
            .ToListAsync();

    public async Task<IEnumerable<PedidoItem>> ListarItensPorPedidoAsync(int codigoPedido)
        => await context.PedidoItens
            .Where(i => i.CodigoPedido == codigoPedido)
            .ToListAsync();

    public async Task AdicionarAsync(Pedido pedido)
        => await context.Pedidos.AddAsync(pedido);

    public Task AtualizarAsync(Pedido pedido)
    {
        context.Pedidos.Update(pedido);
        return Task.CompletedTask;
    }

    public async Task AdicionarItemAsync(PedidoItem item)
        => await context.PedidoItens.AddAsync(item);
}
