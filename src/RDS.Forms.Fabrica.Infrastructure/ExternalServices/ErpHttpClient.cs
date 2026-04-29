using System.Net.Http.Json;
using RDS.Forms.Fabrica.Domain.Interfaces;

namespace RDS.Forms.Fabrica.Infrastructure.ExternalServices;

public class ErpHttpClient(HttpClient http) : IErpHttpClient
{
    public async Task<IEnumerable<PedidoErpDto>> ObterPedidosAprovadosAsync(
        int codigoFilial, CancellationToken ct = default)
    {
        var result = await http.GetFromJsonAsync<IEnumerable<PedidoErpDto>>(
            $"/api/Pedido?cdFilial={codigoFilial}", ct);

        return result ?? Enumerable.Empty<PedidoErpDto>();
    }
}
