using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class NotaFiscalRepository(RdsDbContext context) : INotaFiscalRepository
{
    public async Task<NotaFiscal?> ObterPorChaveAsync(int numero, string serie)
        => await context.NotasFiscais
            .FirstOrDefaultAsync(nf => nf.Numero == numero && nf.SerieNotaFiscal == serie);

    public async Task<IEnumerable<NotaFiscal>> ListarPorFichaAsync(int codigoFicha)
        => await context.NotasFiscais
            .Where(nf => nf.CodigoFicha == codigoFicha)
            .ToListAsync();

    public async Task<IEnumerable<NotaFiscal>> ListarPorFilialEDataAsync(
        int codigoFilial, DateOnly dataInicio, DateOnly dataFim)
        => await context.NotasFiscais
            .Where(nf => nf.CodigoFilial == codigoFilial
                      && nf.DataEmissao >= dataInicio
                      && nf.DataEmissao <= dataFim)
            .OrderByDescending(nf => nf.DataEmissao)
            .ToListAsync();

    public async Task AdicionarAsync(NotaFiscal notaFiscal)
        => await context.NotasFiscais.AddAsync(notaFiscal);

    public Task AtualizarAsync(NotaFiscal notaFiscal)
    {
        context.NotasFiscais.Update(notaFiscal);
        return Task.CompletedTask;
    }
}
