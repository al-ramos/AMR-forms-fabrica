using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Infrastructure.Repositories;

public class LogSistemaRepository(RdsDbContext context) : ILogSistemaRepository
{
    public async Task RegistrarAsync(LogSistema log)
        => await context.LogsSistema.AddAsync(log);

    public async Task<IEnumerable<LogSistema>> ListarPendentesPorFilialAsync(int codigoFilial)
        => await context.LogsSistema
            .Where(l => l.CodigoFilial == codigoFilial && l.Pendente == 1)
            .ToListAsync();
}
