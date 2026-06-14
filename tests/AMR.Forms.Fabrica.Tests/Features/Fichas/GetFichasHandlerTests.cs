using Moq;
using AMR.Forms.Fabrica.Application.Features.Fichas.Handlers;
using AMR.Forms.Fabrica.Application.Features.Fichas.Queries;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Tests.Features.Fichas;

public class GetFichasHandlerTests
{
    private readonly Mock<IFichaRepository> _repoMock = new();

    private GetFichasHandler CreateHandler() => new(_repoMock.Object);

    private static Ficha CriarFicha(int codigo = 1, int filial = 1)
        => new(codigo, filial, "ABC-1234", "BU01", 10, "João", DateOnly.FromDateTime(DateTime.Today));

    [Fact]
    public async Task Handle_SemFiltroData_ChamaListarPorFilial()
    {
        var fichas = new List<Ficha> { CriarFicha(1), CriarFicha(2) };
        _repoMock.Setup(r => r.ListarPorFilialAsync(1)).ReturnsAsync(fichas);

        var query = new GetFichasQuery(CdFilial: 1, DtInicio: null, DtFim: null);
        var result = await CreateHandler().Handle(query, default);

        Assert.Equal(2, result.Count());
        _repoMock.Verify(r => r.ListarPorFilialAsync(1), Times.Once);
        _repoMock.Verify(r => r.ListarPorDataAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ComFiltroData_ChamaListarPorData()
    {
        var inicio = new DateOnly(2026, 1, 1);
        var fim = new DateOnly(2026, 1, 31);
        var fichas = new List<Ficha> { CriarFicha(1) };
        _repoMock.Setup(r => r.ListarPorDataAsync(1, inicio, fim)).ReturnsAsync(fichas);

        var query = new GetFichasQuery(CdFilial: 1, DtInicio: inicio, DtFim: fim);
        var result = await CreateHandler().Handle(query, default);

        Assert.Single(result);
        _repoMock.Verify(r => r.ListarPorDataAsync(1, inicio, fim), Times.Once);
        _repoMock.Verify(r => r.ListarPorFilialAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_SomenteDataInicio_ChamaListarPorFilial()
    {
        var fichas = new List<Ficha>();
        _repoMock.Setup(r => r.ListarPorFilialAsync(2)).ReturnsAsync(fichas);

        var query = new GetFichasQuery(CdFilial: 2, DtInicio: new DateOnly(2026, 6, 1), DtFim: null);
        var result = await CreateHandler().Handle(query, default);

        _repoMock.Verify(r => r.ListarPorFilialAsync(2), Times.Once);
        _repoMock.Verify(r => r.ListarPorDataAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Never);
    }

    [Fact]
    public async Task Handle_FilialVazia_RetornaColecaoVazia()
    {
        _repoMock.Setup(r => r.ListarPorFilialAsync(99)).ReturnsAsync(new List<Ficha>());

        var query = new GetFichasQuery(CdFilial: 99, DtInicio: null, DtFim: null);
        var result = await CreateHandler().Handle(query, default);

        Assert.Empty(result);
    }
}
