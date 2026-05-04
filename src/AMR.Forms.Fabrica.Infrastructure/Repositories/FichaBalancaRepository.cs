using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class FichaBalancaRepository(RdsDbContext context) : IFichaBalancaRepository
{
    public async Task<IEnumerable<FichaBalanca>> ListarPorFichaAsync(int codigoFicha)
        => await context.FichasBalanca
            .Where(fb => fb.CodigoFicha == codigoFicha)
            .ToListAsync();

    public async Task AdicionarAsync(FichaBalanca fichaBalanca)
        => await context.FichasBalanca.AddAsync(fichaBalanca);
}
