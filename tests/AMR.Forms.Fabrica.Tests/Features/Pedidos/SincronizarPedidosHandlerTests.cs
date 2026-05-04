using Moq;
using Microsoft.Extensions.Logging;
using AMR.Forms.Fabrica.Application.Features.Pedidos.Commands;
using AMR.Forms.Fabrica.Application.Features.Pedidos.Handlers;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Tests.Features.Pedidos;

public class SincronizarPedidosHandlerTests
{
    private readonly Mock<IErpHttpClient> _erpMock = new();
    private readonly Mock<IPedidoRepository> _repoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<ILogger<SincronizarPedidosHandler>> _logMock = new();

    private SincronizarPedidosHandler CreateHandler() => new(
        _erpMock.Object, _repoMock.Object, _uowMock.Object, _logMock.Object);

    [Fact]
    public async Task Handle_ErpOffline_RetornaZerado()
    {
        _erpMock.Setup(e => e.ObterPedidosAprovadosAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException("offline"));

        var result = await CreateHandler().Handle(new SincronizarPedidosCommand(1), default);

        Assert.Equal(0, result.Inseridos);
        Assert.Equal(0, result.Atualizados);
        Assert.Equal(0, result.Ignorados);
    }

    [Fact]
    public async Task Handle_PedidoNovo_Insere()
    {
        var dto = new PedidoErpDto(1, 1, DateOnly.FromDateTime(DateTime.Today), null, 10, []);
        _erpMock.Setup(e => e.ObterPedidosAprovadosAsync(1, default)).ReturnsAsync([dto]);
        _repoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync((Pedido?)null);

        var result = await CreateHandler().Handle(new SincronizarPedidosCommand(1), default);

        Assert.Equal(1, result.Inseridos);
        _repoMock.Verify(r => r.AdicionarAsync(It.IsAny<Pedido>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_PedidoExistente_Ignora()
    {
        var pedido = new Pedido(1, 1, null, null, DateOnly.FromDateTime(DateTime.Today));
        var dto = new PedidoErpDto(1, 1, DateOnly.FromDateTime(DateTime.Today), null, 10, []);
        _erpMock.Setup(e => e.ObterPedidosAprovadosAsync(1, default)).ReturnsAsync([dto]);
        _repoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(pedido);

        var result = await CreateHandler().Handle(new SincronizarPedidosCommand(1), default);

        Assert.Equal(0, result.Inseridos);
        Assert.Equal(1, result.Ignorados);
        _repoMock.Verify(r => r.AdicionarAsync(It.IsAny<Pedido>()), Times.Never);
    }
}
