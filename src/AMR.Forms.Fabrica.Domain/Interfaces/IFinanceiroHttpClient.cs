namespace AMR.Forms.Fabrica.Domain.Interfaces;

/// <summary>
/// Contrato para comunicação com a API AMR.Financeiro.
/// Chamado quando eventos do Fábrica geram obrigações/recebimentos financeiros.
/// </summary>
public interface IFinanceiroHttpClient
{
    /// <summary>
    /// Cria uma ContaPagar no Financeiro.
    /// Chamado ao finalizar uma Ficha (saída do veículo).
    /// </summary>
    Task<int?> CriarContaPagarAsync(CriarContaPagarFinanceiroDto dto, CancellationToken ct = default);

    /// <summary>
    /// Cria uma ContaReceber no Financeiro.
    /// Chamado ao registrar a transmissão de uma Nota Fiscal com valor.
    /// </summary>
    Task<int?> CriarContaReceberAsync(CriarContaReceberFinanceiroDto dto, CancellationToken ct = default);
}

public record CriarContaPagarFinanceiroDto(
    int CdFilial,
    string Descricao,
    decimal Valor,
    DateOnly Vencimento
);

public record CriarContaReceberFinanceiroDto(
    int CdFilial,
    string Descricao,
    decimal Valor,
    DateOnly Vencimento,
    string? DocumentoOrigem = null
);
