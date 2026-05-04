using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class BusinessUnitRepository(RdsDbContext context) : IBusinessUnitRepository
{
    public async Task<BusinessUnit?> ObterPorCodigoAsync(string codigo)
        => await context.BusinessUnits.FirstOrDefaultAsync(b => b.Codigo == codigo);

    public async Task<IEnumerable<BusinessUnit>> ListarPorFilialAsync(int codigoFilial)
        => await context.BusinessUnits.ToListAsync();
}
