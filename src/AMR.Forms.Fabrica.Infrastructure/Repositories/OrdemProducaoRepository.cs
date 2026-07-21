using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class OrdemProducaoRepository(RdsDbContext context) : IOrdemProducaoRepository
{
    public async Task<OrdemProducao?> ObterPorIdAsync(int id)
        => await context.OrdensProducao.FirstOrDefaultAsync(o => o.Id == id);

    public async Task<OrdemProducao?> ObterPorNumeroAsync(string numero)
        => await context.OrdensProducao.FirstOrDefaultAsync(o => o.Numero == numero);

    public async Task<IEnumerable<OrdemProducao>> ListarPorFilialAsync(int codigoFilial, StatusOrdemProducao? status = null)
    {
        var query = context.OrdensProducao.Where(o => o.CodigoFilial == codigoFilial);
        if (status.HasValue) query = query.Where(o => o.Status == status.Value);
        return await query.OrderByDescending(o => o.DataAbertura).ToListAsync();
    }

    public async Task AdicionarAsync(OrdemProducao op)
        => await context.OrdensProducao.AddAsync(op);

    public async Task AtualizarAsync(OrdemProducao op)
    {
        context.OrdensProducao.Update(op);
        await Task.CompletedTask;
    }

    public async Task<bool> NumeroJaExisteAsync(string numero)
        => await context.OrdensProducao.AnyAsync(o => o.Numero == numero);
}

public class RastreabilidadeRepository(RdsDbContext context) : IRastreabilidadeRepository
{
    public async Task<IEnumerable<RastreabilidadeItem>> ListarPorOrdemProducaoAsync(int ordemProducaoId)
        => await context.RastreabilidadeItens
            .Where(r => r.OrdemProducaoId == ordemProducaoId)
            .OrderBy(r => r.DataHoraRegistro)
            .ToListAsync();

    public async Task<IEnumerable<RastreabilidadeItem>> ListarPorLoteAsync(string lote)
        => await context.RastreabilidadeItens
            .Where(r => r.Lote == lote)
            .OrderByDescending(r => r.DataHoraRegistro)
            .ToListAsync();

    public async Task AdicionarAsync(RastreabilidadeItem item)
        => await context.RastreabilidadeItens.AddAsync(item);
}
