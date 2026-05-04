using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class NotaFiscalDetalheRepository(RdsDbContext context) : INotaFiscalDetalheRepository
{
    public async Task<IEnumerable<NotaFiscalDetalhe>> ListarPorNotaFiscalAsync(int numeroNf, string serie)
        => await context.NotasFiscaisDetalhe
            .Where(d => d.NumeroNotaFiscal == numeroNf && d.SerieNotaFiscal == serie)
            .ToListAsync();



}
