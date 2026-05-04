using System.Net.Http.Json;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Infrastructure.ExternalServices;

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
