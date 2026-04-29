using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Infrastructure.Repositories;

public class NotaFiscalDetalheRepository(RdsDbContext context) : INotaFiscalDetalheRepository
{
    public async Task<IEnumerable<NotaFiscalDetalhe>> ListarPorNotaFiscalAsync(int numeroNf, string serie)
        => await context.NotasFiscaisDetalhe
            .Where(d => d.NumeroNotaFiscal == numeroNf && d.SerieNotaFiscal == serie)
            .ToListAsync();



}
