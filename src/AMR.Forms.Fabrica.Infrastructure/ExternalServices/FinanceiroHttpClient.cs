using System.Net.Http.Json;
using AMR.Forms.Fabrica.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AMR.Forms.Fabrica.Infrastructure.ExternalServices;

/// <summary>
/// HTTP client para integração com AMR.Financeiro API.
/// </summary>
public class FinanceiroHttpClient(HttpClient http, ILogger<FinanceiroHttpClient> logger) : IFinanceiroHttpClient
{
    public async Task<int?> CriarContaPagarAsync(CriarContaPagarFinanceiroDto dto, CancellationToken ct = default)
    {
        try
        {
            var payload = new
            {
                cdFilial   = dto.CdFilial,
                descricao  = dto.Descricao,
                valor      = dto.Valor,
                vencimento = dto.Vencimento.ToString("yyyy-MM-dd")
            };

            var response = await http.PostAsJsonAsync("/api/contaspagar", payload, ct);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                logger.LogWarning("Financeiro rejeitou ContaPagar [{Status}]: {Body}", response.StatusCode, body);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<IdResponse>(cancellationToken: ct);
            return result?.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao criar ContaPagar no Financeiro para filial {CdFilial}", dto.CdFilial);
            return null;
        }
    }

    public async Task<int?> CriarContaReceberAsync(CriarContaReceberFinanceiroDto dto, CancellationToken ct = default)
    {
        try
        {
            var payload = new
            {
                cdFilial         = dto.CdFilial,
                descricao        = dto.Descricao,
                valor            = dto.Valor,
                vencimento       = dto.Vencimento.ToString("yyyy-MM-dd"),
                documentoOrigem  = dto.DocumentoOrigem
            };

            var response = await http.PostAsJsonAsync("/api/contasreceber", payload, ct);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                logger.LogWarning("Financeiro rejeitou ContaReceber [{Status}]: {Body}", response.StatusCode, body);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<IdResponse>(cancellationToken: ct);
            return result?.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao criar ContaReceber no Financeiro para filial {CdFilial}", dto.CdFilial);
            return null;
        }
    }

    private record IdResponse(int Id);
}
