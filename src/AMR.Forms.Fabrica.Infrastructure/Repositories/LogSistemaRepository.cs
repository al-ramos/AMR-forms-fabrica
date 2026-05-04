using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class LogSistemaRepository(RdsDbContext context) : ILogSistemaRepository
{
    public async Task RegistrarAsync(LogSistema log)
        => await context.LogsSistema.AddAsync(log);

    public async Task<IEnumerable<LogSistema>> ListarPendentesPorFilialAsync(int codigoFilial)
        => await context.LogsSistema
            .Where(l => l.CodigoFilial == codigoFilial && l.Pendente == 1)
            .ToListAsync();
}
