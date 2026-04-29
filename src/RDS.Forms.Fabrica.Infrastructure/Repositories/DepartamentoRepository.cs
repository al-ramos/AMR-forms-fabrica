using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Infrastructure.Repositories;

public class DepartamentoRepository(RdsDbContext context) : IDepartamentoRepository
{
    public async Task<IEnumerable<Departamento>> ListarPorFilialAsync(int codigoFilial)
        => await context.Departamentos
            .Where(d => d.CodigoFilial == codigoFilial)
            .OrderBy(d => d.DescricaoProduto)
            .ToListAsync();
}
