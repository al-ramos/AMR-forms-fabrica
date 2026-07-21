using AMR.Forms.Fabrica.Application.Features.OrdensProducao.DTOs;
using AMR.Forms.Fabrica.Application.Features.OrdensProducao.Queries;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.OrdensProducao.Handlers;

public class GetOrdemProducaoHandler(IOrdemProducaoRepository repo, IProdutoRepository produtoRepo)
    : IRequestHandler<GetOrdemProducaoQuery, OrdemProducaoDto?>
{
    public async Task<OrdemProducaoDto?> Handle(GetOrdemProducaoQuery request, CancellationToken ct)
    {
        var op = await repo.ObterPorIdAsync(request.Id);
        if (op is null) return null;
        var produto = await produtoRepo.ObterPorIdAsync(op.CodigoProduto);
        return MappingExtensions.ToDto(op, produto);
    }
}

public class GetOrdemProducaoPorNumeroHandler(IOrdemProducaoRepository repo, IProdutoRepository produtoRepo)
    : IRequestHandler<GetOrdemProducaoPorNumeroQuery, OrdemProducaoDto?>
{
    public async Task<OrdemProducaoDto?> Handle(GetOrdemProducaoPorNumeroQuery request, CancellationToken ct)
    {
        var op = await repo.ObterPorNumeroAsync(request.Numero);
        if (op is null) return null;
        var produto = await produtoRepo.ObterPorIdAsync(op.CodigoProduto);
        return MappingExtensions.ToDto(op, produto);
    }
}

public class GetOrdensProducaoHandler(IOrdemProducaoRepository repo, IProdutoRepository produtoRepo)
    : IRequestHandler<GetOrdensProducaoQuery, IReadOnlyList<OrdemProducaoDto>>
{
    public async Task<IReadOnlyList<OrdemProducaoDto>> Handle(GetOrdensProducaoQuery request, CancellationToken ct)
    {
        var ops = await repo.ListarPorFilialAsync(request.CodigoFilial, request.Status);
        var result = new List<OrdemProducaoDto>();
        foreach (var op in ops)
        {
            var produto = await produtoRepo.ObterPorIdAsync(op.CodigoProduto);
            result.Add(MappingExtensions.ToDto(op, produto));
        }
        return result.AsReadOnly();
    }
}

public class GetRastreabilidadeHandler(IRastreabilidadeRepository repo, IOrdemProducaoRepository opRepo, IProdutoRepository produtoRepo)
    : IRequestHandler<GetRastreabilidadeQuery, IReadOnlyList<RastreabilidadeItemDto>>
{
    public async Task<IReadOnlyList<RastreabilidadeItemDto>> Handle(GetRastreabilidadeQuery request, CancellationToken ct)
    {
        var itens = await repo.ListarPorOrdemProducaoAsync(request.OrdemProducaoId);
        var op = await opRepo.ObterPorIdAsync(request.OrdemProducaoId);
        var result = new List<RastreabilidadeItemDto>();
        foreach (var item in itens)
        {
            var produto = await produtoRepo.ObterPorIdAsync(item.CodigoProduto);
            result.Add(MappingExtensions.ToDto(item, op?.Numero ?? "", produto));
        }
        return result.AsReadOnly();
    }
}

public class GetRastreabilidadePorLoteHandler(IRastreabilidadeRepository repo, IOrdemProducaoRepository opRepo, IProdutoRepository produtoRepo)
    : IRequestHandler<GetRastreabilidadePorLoteQuery, IReadOnlyList<RastreabilidadeItemDto>>
{
    public async Task<IReadOnlyList<RastreabilidadeItemDto>> Handle(GetRastreabilidadePorLoteQuery request, CancellationToken ct)
    {
        var itens = await repo.ListarPorLoteAsync(request.Lote);
        var result = new List<RastreabilidadeItemDto>();
        foreach (var item in itens)
        {
            var op = await opRepo.ObterPorIdAsync(item.OrdemProducaoId);
            var produto = await produtoRepo.ObterPorIdAsync(item.CodigoProduto);
            result.Add(MappingExtensions.ToDto(item, op?.Numero ?? "", produto));
        }
        return result.AsReadOnly();
    }
}

// ── helpers ──────────────────────────────────────────────────────────────────
file static class MappingExtensions
{
    internal static OrdemProducaoDto ToDto(OrdemProducao op, Produto? produto) => new(
        op.Id, op.Numero, op.CodigoProduto, produto?.Nome, op.CodigoFilial,
        op.QuantidadePlanejada, op.QuantidadeProduzida, op.QuantidadeRejeitada,
        op.QuantidadeRestante, op.PercentualConclusao, op.Status,
        op.DataAbertura, op.DataPrevistaFim, op.DataFechamento,
        op.ObservacaoGeral, op.MotivoCancelamento);

    internal static RastreabilidadeItemDto ToDto(RastreabilidadeItem item, string numOp, Produto? produto) => new(
        item.Id, item.OrdemProducaoId, numOp, item.CodigoProduto, produto?.Nome,
        item.Lote, item.Quantidade, item.TipoMovimento,
        item.DataHoraRegistro, item.CodigoOperador, item.Observacao);
}
