using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class FichaRepository(RdsDbContext context) : IFichaRepository
{
    public async Task<Ficha?> ObterPorIdAsync(int id)
        => await context.Fichas.FirstOrDefaultAsync(f => f.Codigo == id);

    public async Task<IEnumerable<Ficha>> ListarPorFilialAsync(int codigoFilial)
        => await context.Fichas
            .Where(f => f.CodigoFilial == codigoFilial)
            .OrderByDescending(f => f.DataFicha)
            .ToListAsync();

    public async Task<IEnumerable<Ficha>> ListarPorDataAsync(
        int codigoFilial, DateOnly dataInicio, DateOnly dataFim)
        => await context.Fichas
            .Where(f => f.CodigoFilial == codigoFilial
                     && f.DataFicha >= dataInicio
                     && f.DataFicha <= dataFim)
            .OrderByDescending(f => f.DataFicha)
            .ToListAsync();

    public async Task AdicionarAsync(Ficha ficha)
        => await context.Fichas.AddAsync(ficha);

    public Task AtualizarAsync(Ficha ficha)
    {
        context.Fichas.Update(ficha);
        return Task.CompletedTask;
    }
}
