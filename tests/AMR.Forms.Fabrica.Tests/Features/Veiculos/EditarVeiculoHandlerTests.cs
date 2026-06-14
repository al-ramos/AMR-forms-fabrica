using Moq;
using AMR.Forms.Fabrica.Application.Features.Veiculos.Commands;
using AMR.Forms.Fabrica.Application.Features.Veiculos.Handlers;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Tests.Features.Veiculos;

public class EditarVeiculoHandlerTests
{
    private readonly Mock<IVeiculoRepository> _repoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();

    private EditarVeiculoHandler CreateHandler() => new(_repoMock.Object, _uowMock.Object);

    [Fact]
    public async Task Handle_VeiculoExistente_AtualizaCampos()
    {
        var veiculo = new Veiculo("ABC-1234", 1, "SP", "OLD-RNTC");
        _repoMock.Setup(r => r.ObterPorPlacaAsync("ABC-1234")).ReturnsAsync(veiculo);

        var command = new EditarVeiculoCommand("ABC-1234", 2, "RJ", "NEW-RNTC");
        await CreateHandler().Handle(command, default);

        Assert.Equal(2, veiculo.CodigoFilial);
        Assert.Equal("RJ", veiculo.UfVeiculo);
        Assert.Equal("NEW-RNTC", veiculo.CodigoRntc);
    }

    [Fact]
    public async Task Handle_VeiculoExistente_SalvaAlteracoes()
    {
        var veiculo = new Veiculo("XYZ-9999", 1, "SP", null);
        _repoMock.Setup(r => r.ObterPorPlacaAsync("XYZ-9999")).ReturnsAsync(veiculo);

        var command = new EditarVeiculoCommand("XYZ-9999", 1, "MG", null);
        await CreateHandler().Handle(command, default);

        _repoMock.Verify(r => r.AtualizarAsync(veiculo), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_VeiculoNaoEncontrado_LancaKeyNotFoundException()
    {
        _repoMock.Setup(r => r.ObterPorPlacaAsync(It.IsAny<string>())).ReturnsAsync((Veiculo?)null);

        var command = new EditarVeiculoCommand("NNN-0000", 1, "SP", null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            CreateHandler().Handle(command, default));
    }

    [Fact]
    public async Task Handle_VeiculoExistente_RetornaPlaca()
    {
        var veiculo = new Veiculo("DEF-5678", 1, "SP", null);
        _repoMock.Setup(r => r.ObterPorPlacaAsync("DEF-5678")).ReturnsAsync(veiculo);

        var command = new EditarVeiculoCommand("DEF-5678", 2, "RS", null);
        var result = await CreateHandler().Handle(command, default);

        Assert.Equal("DEF-5678", result);
    }
}
