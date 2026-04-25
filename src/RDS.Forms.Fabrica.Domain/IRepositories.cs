using RDS.Forms.Fabrica.Domain.Entities;

namespace RDS.Forms.Fabrica.Domain.Interfaces;

public interface IFichaRepository
{
    Task<Ficha?> ObterPorIdAsync(int id);
    Task<IEnumerable<Ficha>> ListarPorFilialAsync(int codigoFilial);
    Task<IEnumerable<Ficha>> ListarPorDataAsync(int codigoFilial, DateOnly dataInicio, DateOnly dataFim);
    Task AdicionarAsync(Ficha ficha);
    Task AtualizarAsync(Ficha ficha);
}

public interface IFilialRepository
{
    Task<Filial?> ObterPorIdAsync(int id);
    Task<IEnumerable<Filial>> ListarTodosAsync();
}

public interface ITipoOperacaoRepository
{
    Task<TipoOperacao?> ObterPorIdAsync(int id);
    Task<IEnumerable<TipoOperacao>> ListarPorFilialAsync(int codigoFilial);
    Task<IEnumerable<TipoOperacaoPasso>> ListarPassosPorTipoOperacaoAsync(int codigoTipoOperacao);
    Task<IEnumerable<TipoOperacaoPassoCfg>> ListarConfiguracoesPorTipoOperacaoAsync(int codigoTipoOperacao);
}

public interface IProdutoRepository
{
    Task<Produto?> ObterPorIdAsync(int id);
    Task<IEnumerable<Produto>> ListarPorBusinessUnitAsync(string codigoBusinessUnit);
    Task<Produto?> ObterPorEanAsync(string codigoEan);
}

public interface IPedidoRepository
{
    Task<Pedido?> ObterPorIdAsync(int id);
    Task<IEnumerable<Pedido>> ListarPorFilialAsync(int codigoFilial);
    Task<IEnumerable<PedidoItem>> ListarItensPorPedidoAsync(int codigoPedido);
}

public interface INotaFiscalRepository
{
    Task<NotaFiscal?> ObterPorChaveAsync(int numero, string serie);
    Task<IEnumerable<NotaFiscal>> ListarPorFichaAsync(int codigoFicha);
    Task<IEnumerable<NotaFiscal>> ListarPorFilialEDataAsync(int codigoFilial, DateOnly dataInicio, DateOnly dataFim);
    Task AdicionarAsync(NotaFiscal notaFiscal);
    Task AtualizarAsync(NotaFiscal notaFiscal);
}

public interface INotaFiscalDetalheRepository
{
    Task<IEnumerable<NotaFiscalDetalhe>> ListarPorNotaFiscalAsync(int numeroNf, string serie);
}

public interface IVeiculoRepository
{
    Task<Veiculo?> ObterPorPlacaAsync(string placa);
    Task<IEnumerable<Veiculo>> ListarTodosAsync();
    Task<IEnumerable<Veiculo>> ListarPorFilialAsync(int codigoFilial);
    Task AdicionarAsync(Veiculo veiculo);
    Task AtualizarAsync(Veiculo veiculo);
}

public interface IFichaBalancaRepository
{
    Task<IEnumerable<FichaBalanca>> ListarPorFichaAsync(int codigoFicha);
    Task AdicionarAsync(FichaBalanca fichaBalanca);
}

public interface IFichaLoadDetalheRepository
{
    Task<IEnumerable<FichaLoadDetalhe>> ListarPorFichaAsync(int codigoFicha);
    Task AdicionarAsync(FichaLoadDetalhe detalhe);
}

public interface ILogSistemaRepository
{
    Task RegistrarAsync(LogSistema log);
    Task<IEnumerable<LogSistema>> ListarPendentesPorFilialAsync(int codigoFilial);
}

public interface IDepartamentoRepository
{
    Task<IEnumerable<Departamento>> ListarPorFilialAsync(int codigoFilial);
}

public interface IBusinessUnitRepository
{
    Task<BusinessUnit?> ObterPorCodigoAsync(string codigo);
    Task<IEnumerable<BusinessUnit>> ListarPorFilialAsync(int codigoFilial);
}
