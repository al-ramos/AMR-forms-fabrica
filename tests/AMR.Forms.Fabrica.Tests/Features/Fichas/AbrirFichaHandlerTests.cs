using Moq;
using AMR.Forms.Fabrica.Application.Features.Fichas.Commands;
using AMR.Forms.Fabrica.Application.Features.Fichas.Handlers;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Tests.Features.Fichas;

public class AbrirFichaHandlerTests
{
    private readonly Mock<IFichaRepository> _repoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();

    private AbrirFichaHandler CreateHandler() => new(_repoMock.Object, _uowMock.Object);

    [Fact]
    public async Task Handle_CriaFichaEAdicionaNoRepositorio()
    {
        var command = new AbrirFichaCommand(CodigoFilial: 1, PlacaVeiculo: "ABC-1234", CodigoTipoOperacao: 10);

        await CreateHandler().Handle(command, default);

        _repoMock.Verify(r => r.AdicionarAsync(It.Is<Ficha>(f =>
            f.CodigoFilial == 1 &&
            f.PlacaVeiculo == "ABC-1234" &&
            f.CodigoTipoOperacao == 10)), Times.Once);
    }

    [Fact]
    public async Task Handle_SalvaAlteracoesViaUnitOfWork()
    {
        var command = new AbrirFichaCommand(1, "XYZ-9999", 5);

        await CreateHandler().Handle(command, default);

        _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_RetornaCodigoDaFichaCriada()
    {
        var command = new AbrirFichaCommand(1, "DEF-5678", 3);

        var result = await CreateHandler().Handle(command, default);

        Assert.Equal(0, result); // código 0 pois EF não atribui ID em memória
    }
}
