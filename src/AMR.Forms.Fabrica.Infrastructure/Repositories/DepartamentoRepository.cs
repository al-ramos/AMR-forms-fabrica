using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class DepartamentoRepository(RdsDbContext context) : IDepartamentoRepository
{
    public async Task<IEnumerable<Departamento>> ListarPorFilialAsync(int codigoFilial)
        => await context.Departamentos
            .Where(d => d.CodigoFilial == codigoFilial)
            .OrderBy(d => d.DescricaoProduto)
            .ToListAsync();
}
