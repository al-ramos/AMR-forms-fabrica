using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.NotasFiscais.Commands;

/// <summary>
/// Registra a transmissão eletrônica de uma NF e cria a ContaReceber correspondente no Financeiro.
/// </summary>
public record RegistrarTransmissaoNfCommand(
    int    Numero,
    string Serie,
    string ChaveNfe,
    string Protocolo,
    decimal? ValorTransmissao
) : IRequest;
