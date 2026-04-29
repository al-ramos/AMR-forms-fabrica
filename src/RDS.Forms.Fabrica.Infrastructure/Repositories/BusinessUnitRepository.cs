using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Infrastructure.Repositories;

public class BusinessUnitRepository(RdsDbContext context) : IBusinessUnitRepository
{
    public async Task<BusinessUnit?> ObterPorCodigoAsync(string codigo)
        => await context.BusinessUnits.FirstOrDefaultAsync(b => b.Codigo == codigo);

    public async Task<IEnumerable<BusinessUnit>> ListarPorFilialAsync(int codigoFilial)
        => await context.BusinessUnits.ToListAsync();
}
