using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class PlanoManutencaoRepository(RdsDbContext context) : IPlanoManutencaoRepository
{
    public async Task<PlanoManutencao?> ObterPorIdAsync(int id)
        => await context.PlanosManutencao.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<PlanoManutencao>> ListarPorEquipamentoAsync(int equipamentoId, bool apenasAtivos = true)
    {
        var query = context.PlanosManutencao.Where(p => p.EquipamentoId == equipamentoId);
        if (apenasAtivos) query = query.Where(p => p.Ativo);
        return await query.OrderBy(p => p.ProximaExecucao).ToListAsync();
    }

    public async Task<IEnumerable<PlanoManutencao>> ListarVencidosOuProximosAsync(int codigoFilial, int diasAntecedencia = 7)
    {
        var limite = DateTime.UtcNow.AddDays(diasAntecedencia);
        return await context.PlanosManutencao
            .Where(p => p.CodigoFilial == codigoFilial && p.Ativo && p.ProximaExecucao <= limite)
            .OrderBy(p => p.ProximaExecucao)
            .ToListAsync();
    }

    public async Task AdicionarAsync(PlanoManutencao plano)
        => await context.PlanosManutencao.AddAsync(plano);

    public Task AtualizarAsync(PlanoManutencao plano)
    {
        context.PlanosManutencao.Update(plano);
        return Task.CompletedTask;
    }
}

public class OrdemManutencaoRepository(RdsDbContext context) : IOrdemManutencaoRepository
{
    public async Task<OrdemManutencao?> ObterPorIdAsync(int id)
        => await context.OrdensManutencao.FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<OrdemManutencao>> ListarPorFilialAsync(int codigoFilial, StatusOrdemManutencao? status = null)
    {
        var query = context.OrdensManutencao.Where(o => o.CodigoFilial == codigoFilial);
        if (status.HasValue) query = query.Where(o => o.Status == status.Value);
        return await query.OrderBy(o => o.DataPrevista).ToListAsync();
    }

    public async Task<IEnumerable<OrdemManutencao>> ListarPorEquipamentoAsync(int equipamentoId, StatusOrdemManutencao? status = null)
    {
        var query = context.OrdensManutencao.Where(o => o.EquipamentoId == equipamentoId);
        if (status.HasValue) query = query.Where(o => o.Status == status.Value);
        return await query.OrderByDescending(o => o.DataPrevista).ToListAsync();
    }

    public async Task AdicionarAsync(OrdemManutencao ordem)
        => await context.OrdensManutencao.AddAsync(ordem);

    public Task AtualizarAsync(OrdemManutencao ordem)
    {
        context.OrdensManutencao.Update(ordem);
        return Task.CompletedTask;
    }
}
