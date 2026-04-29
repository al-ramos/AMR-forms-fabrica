using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Infrastructure.Repositories;

public class FichaLoadDetalheRepository(RdsDbContext context) : IFichaLoadDetalheRepository
{
    public async Task<IEnumerable<FichaLoadDetalhe>> ListarPorFichaAsync(int codigoFicha)
        => await context.FichasLoadDetalhe
            .Where(f => f.CodigoFicha == codigoFicha)
            .ToListAsync();

    public async Task AdicionarAsync(FichaLoadDetalhe detalhe)
        => await context.FichasLoadDetalhe.AddAsync(detalhe);
}
