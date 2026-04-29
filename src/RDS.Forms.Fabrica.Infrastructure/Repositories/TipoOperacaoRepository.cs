using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Domain.Entities;
using RDS.Forms.Fabrica.Domain.Interfaces;
using RDS.Forms.Fabrica.Infrastructure.Data;

namespace RDS.Forms.Fabrica.Infrastructure.Repositories;

public class TipoOperacaoRepository(RdsDbContext context) : ITipoOperacaoRepository
{
    public async Task<TipoOperacao?> ObterPorIdAsync(int id)
        => await context.TiposOperacao.FirstOrDefaultAsync(t => t.Codigo == id);

    public async Task<IEnumerable<TipoOperacao>> ListarPorFilialAsync(int codigoFilial)
        => await context.TiposOperacao
            .Where(t => t.CodigoFilial == codigoFilial)
            .OrderBy(t => t.Nome)
            .ToListAsync();

    public async Task<IEnumerable<TipoOperacaoPasso>> ListarPassosPorTipoOperacaoAsync(int codigoTipoOperacao)
        => await context.TiposOperacaoPasso
            .Where(t => t.CodigoTipoOperacao == codigoTipoOperacao)
            .OrderBy(t => t.Sequencia)
            .ToListAsync();

    public async Task<IEnumerable<TipoOperacaoPassoCfg>> ListarConfiguracoesPorTipoOperacaoAsync(int codigoTipoOperacao)
        => await context.TiposOperacaoPassoCfg
            .Where(t => t.CodigoTipoOperacao == codigoTipoOperacao)
            .ToListAsync();
}
