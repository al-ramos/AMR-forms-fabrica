using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class ProdutoRepository(RdsDbContext context) : IProdutoRepository
{
    public async Task<Produto?> ObterPorIdAsync(int id)
        => await context.Produtos.FirstOrDefaultAsync(p => p.Codigo == id);

    public async Task<IEnumerable<Produto>> ListarPorBusinessUnitAsync(string codigoBusinessUnit)
        => await context.Produtos
            .Where(p => p.CodigoBusinessUnit == codigoBusinessUnit)
            .OrderBy(p => p.Nome)
            .ToListAsync();

    public async Task<Produto?> ObterPorEanAsync(string codigoEan)
        => await context.Produtos.FirstOrDefaultAsync(p => p.CodigoEan == codigoEan);
}
