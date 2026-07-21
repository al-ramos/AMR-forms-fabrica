using Microsoft.EntityFrameworkCore;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using AMR.Forms.Fabrica.Infrastructure.Data;

namespace AMR.Forms.Fabrica.Infrastructure.Repositories;

public class BomRepository(RdsDbContext context) : IBomRepository
{
    public async Task<IEnumerable<BomItem>> ListarItensPorProdutoPaiAsync(int codigoProdutoPai, bool apenasAtivos = true)
    {
        var query = context.BomItens.Where(b => b.CodigoProdutoPai == codigoProdutoPai);
        if (apenasAtivos) query = query.Where(b => b.Ativo);
        return await query.OrderBy(b => b.CodigoProdutoFilho).ToListAsync();
    }

    public async Task<IEnumerable<BomItem>> ListarArvoreCompletaAsync(int codigoProdutoPai)
    {
        // Carrega toda a árvore via CTE recursiva (SQLite suporta WITH RECURSIVE)
        var todos = new List<BomItem>();
        await CarregarRecursivoAsync(codigoProdutoPai, todos);
        return todos;
    }

    private async Task CarregarRecursivoAsync(int codigoProdutoPai, List<BomItem> acumulador)
    {
        var filhos = await context.BomItens
            .Where(b => b.CodigoProdutoPai == codigoProdutoPai && b.Ativo)
            .ToListAsync();

        acumulador.AddRange(filhos);
        foreach (var filho in filhos)
            await CarregarRecursivoAsync(filho.CodigoProdutoFilho, acumulador);
    }

    public async Task<BomItem?> ObterItemAsync(int codigoProdutoPai, int codigoProdutoFilho)
        => await context.BomItens
            .FirstOrDefaultAsync(b => b.CodigoProdutoPai == codigoProdutoPai
                                   && b.CodigoProdutoFilho == codigoProdutoFilho);

    public async Task AdicionarAsync(BomItem item)
        => await context.BomItens.AddAsync(item);

    public Task AtualizarAsync(BomItem item)
    {
        context.BomItens.Update(item);
        return Task.CompletedTask;
    }

    public async Task<bool> ExisteReferenciaCircularAsync(int codigoProdutoPai, int codigoProdutoFilho)
    {
        // Verifica se codigoProdutoPai é descendente de codigoProdutoFilho
        // (o que criaria ciclo ao adicionar filho→pai)
        var visitados = new HashSet<int>();
        return await VerificarCicloAsync(codigoProdutoFilho, codigoProdutoPai, visitados);
    }

    private async Task<bool> VerificarCicloAsync(int raiz, int alvo, HashSet<int> visitados)
    {
        if (visitados.Contains(raiz)) return false;
        visitados.Add(raiz);

        var filhos = await context.BomItens
            .Where(b => b.CodigoProdutoPai == raiz && b.Ativo)
            .Select(b => b.CodigoProdutoFilho)
            .ToListAsync();

        foreach (var filho in filhos)
        {
            if (filho == alvo) return true;
            if (await VerificarCicloAsync(filho, alvo, visitados)) return true;
        }

        return false;
    }
}

public class ProdutoBomRepository(RdsDbContext context) : ProdutoRepository(context), IProdutoBomRepository
{
    private readonly RdsDbContext _ctx = context;

    public async Task<Produto?> ObterComDadosBomAsync(int codigo)
        => await _ctx.Produtos
            .Include(p => p.BomComoFabricado)
            .FirstOrDefaultAsync(p => p.Codigo == codigo);

    public async Task<IEnumerable<Produto>> ListarFabricadosAsync()
        => await _ctx.Produtos
            .Where(p => p.TipoProduto == TiposProduto.Fabricado)
            .OrderBy(p => p.Nome)
            .ToListAsync();

    public Task AtualizarDadosBomAsync(Produto produto)
    {
        _ctx.Produtos.Update(produto);
        return Task.CompletedTask;
    }
}
