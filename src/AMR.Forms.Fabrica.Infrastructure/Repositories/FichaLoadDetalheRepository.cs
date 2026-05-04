using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class FichaLoadDetalheRepository(RdsDbContext context) : IFichaLoadDetalheRepository
{
    public async Task<IEnumerable<FichaLoadDetalhe>> ListarPorFichaAsync(int codigoFicha)
        => await context.FichasLoadDetalhe
            .Where(f => f.CodigoFicha == codigoFicha)
            .ToListAsync();

    public async Task AdicionarAsync(FichaLoadDetalhe detalhe)
        => await context.FichasLoadDetalhe.AddAsync(detalhe);
}
