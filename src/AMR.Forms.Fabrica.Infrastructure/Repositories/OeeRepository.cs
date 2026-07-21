using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class EquipamentoRepository(RdsDbContext context) : IEquipamentoRepository
{
    public async Task<Equipamento?> ObterPorIdAsync(int id)
        => await context.Equipamentos.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<Equipamento>> ListarPorFilialAsync(int codigoFilial, bool apenasAtivos = true)
    {
        var query = context.Equipamentos.Where(e => e.CodigoFilial == codigoFilial);
        if (apenasAtivos) query = query.Where(e => e.Ativo);
        return await query.OrderBy(e => e.CodigoArea).ThenBy(e => e.Nome).ToListAsync();
    }

    public async Task AdicionarAsync(Equipamento equipamento)
        => await context.Equipamentos.AddAsync(equipamento);

    public Task AtualizarAsync(Equipamento equipamento)
    {
        context.Equipamentos.Update(equipamento);
        return Task.CompletedTask;
    }
}

public class RegistroOeeRepository(RdsDbContext context) : IRegistroOeeRepository
{
    public async Task<RegistroOee?> ObterPorIdAsync(int id)
        => await context.RegistrosOee.FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<RegistroOee>> ListarPorEquipamentoAsync(
        int equipamentoId, DateTime? inicio = null, DateTime? fim = null)
    {
        var query = context.RegistrosOee.Where(r => r.EquipamentoId == equipamentoId);
        if (inicio.HasValue) query = query.Where(r => r.DataHoraInicio >= inicio.Value);
        if (fim.HasValue)    query = query.Where(r => r.DataHoraFim   <= fim.Value);
        return await query.OrderByDescending(r => r.DataHoraInicio).ToListAsync();
    }

    public async Task<IEnumerable<RegistroOee>> ListarPorFilialAsync(
        int codigoFilial, DateTime dataInicio, DateTime dataFim)
        => await context.RegistrosOee
            .Where(r => r.CodigoFilial == codigoFilial
                     && r.DataHoraInicio >= dataInicio
                     && r.DataHoraFim   <= dataFim)
            .OrderBy(r => r.EquipamentoId)
            .ThenByDescending(r => r.DataHoraInicio)
            .ToListAsync();

    public async Task AdicionarAsync(RegistroOee registro)
        => await context.RegistrosOee.AddAsync(registro);
}
