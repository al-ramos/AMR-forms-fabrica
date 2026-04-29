using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Infrastructure.Repositories;

public class FilialRepository(RdsDbContext context) : IFilialRepository
{
    public async Task<Filial?> ObterPorIdAsync(int id)
        => await context.Filiais.FirstOrDefaultAsync(f => f.Codigo == id);

    public async Task<IEnumerable<Filial>> ListarTodosAsync()
        => await context.Filiais.OrderBy(f => f.Nome).ToListAsync();
}
