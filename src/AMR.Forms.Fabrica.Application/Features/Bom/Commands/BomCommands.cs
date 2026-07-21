using AMR.Forms.Fabrica.Application.Common;
using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Bom.Commands;

public record AdicionarBomItemCommand(
    int CodigoProdutoPai,
    int CodigoProdutoFilho,
    decimal Quantidade,
    decimal PercentualPerda
) : IRequest<Result<int>>;

public record RemoverBomItemCommand(
    int CodigoProdutoPai,
    int CodigoProdutoFilho
) : IRequest<Result>;

public record AtualizarBomItemCommand(
    int CodigoProdutoPai,
    int CodigoProdutoFilho,
    decimal Quantidade,
    decimal PercentualPerda
) : IRequest<Result>;

public record AtualizarDadosBomProdutoCommand(
    int CodigoProduto,
    string? TipoProduto,
    int LeadTimeDias,
    decimal CustoPadrao
) : IRequest<Result>;
