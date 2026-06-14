using Moq;
using AMR.Forms.Fabrica.Application.Features.Veiculos.Commands;
using AMR.Forms.Fabrica.Application.Features.Veiculos.Handlers;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Tests.Features.Veiculos;

public class CadastrarVeiculoHandlerTests
{
    private readonly Mock<IVeiculoRepository> _repoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();

    private CadastrarVeiculoHandler CreateHandler() => new(_repoMock.Object, _uowMock.Object);

    [Fact]
    public async Task Handle_CriaVeiculoEAdicionaNoRepositorio()
    {
        var command = new CadastrarVeiculoCommand("abc-1234", 1, "SP", "RNTC-001");

        await CreateHandler().Handle(command, default);

        _repoMock.Verify(r => r.AdicionarAsync(It.Is<Veiculo>(v =>
            v.Placa == "ABC-1234" &&
            v.CodigoFilial == 1 &&
            v.UfVeiculo == "SP" &&
            v.CodigoRntc == "RNTC-001")), Times.Once);
    }

    [Fact]
    public async Task Handle_SalvaAlteracoesViaUnitOfWork()
    {
        var command = new CadastrarVeiculoCommand("XYZ-9999", 2, "RJ", null);

        await CreateHandler().Handle(command, default);

        _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_RetornaPlacaNormalizadaEmUppercase()
    {
        var command = new CadastrarVeiculoCommand("abc-1234", 1, "SP", null);

        var result = await CreateHandler().Handle(command, default);

        Assert.Equal("ABC-1234", result);
    }

    [Fact]
    public async Task Handle_PlacaSemRntc_CriaVeiculoComRntcNulo()
    {
        var command = new CadastrarVeiculoCommand("GHI-3456", 3, null, null);

        await CreateHandler().Handle(command, default);

        _repoMock.Verify(r => r.AdicionarAsync(It.Is<Veiculo>(v =>
            v.CodigoRntc == null &&
            v.UfVeiculo == null)), Times.Once);
    }
}
