using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Infrastructure.Repositories;

public class VeiculoRepository(RdsDbContext context) : IVeiculoRepository
{
    public async Task<Veiculo?> ObterPorPlacaAsync(string placa)
        => await context.Veiculos.FirstOrDefaultAsync(v => v.Placa == placa);

    public async Task<IEnumerable<Veiculo>> ListarTodosAsync()
        => await context.Veiculos
            .OrderBy(v => v.Placa)
            .ToListAsync();

    public async Task<IEnumerable<Veiculo>> ListarPorFilialAsync(int codigoFilial)
        => await context.Veiculos
            .Where(v => v.CodigoFilial == codigoFilial)
            .OrderBy(v => v.Placa)
            .ToListAsync();
    
    public async Task AdicionarAsync(Veiculo veiculo)
        => await context.Veiculos.AddAsync(veiculo);

    public Task AtualizarAsync(Veiculo veiculo)
    {
        context.Veiculos.Update(veiculo);
        return Task.CompletedTask;
    }
}
