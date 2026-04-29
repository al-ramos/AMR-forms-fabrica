using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Infrastructure.Repositories;

public class FichaBalancaRepository(RdsDbContext context) : IFichaBalancaRepository
{
    public async Task<IEnumerable<FichaBalanca>> ListarPorFichaAsync(int codigoFicha)
        => await context.FichasBalanca
            .Where(fb => fb.CodigoFicha == codigoFicha)
            .ToListAsync();

    public async Task AdicionarAsync(FichaBalanca fichaBalanca)
        => await context.FichasBalanca.AddAsync(fichaBalanca);
}
