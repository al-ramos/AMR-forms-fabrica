using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class FilialRepository(RdsDbContext context) : IFilialRepository
{
    public async Task<Filial?> ObterPorIdAsync(int id)
        => await context.Filiais.FirstOrDefaultAsync(f => f.Codigo == id);

    public async Task<IEnumerable<Filial>> ListarTodosAsync()
        => await context.Filiais.OrderBy(f => f.Nome).ToListAsync();
}
