using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using AMR.Forms.Fabrica.Domain.Entities;
using AMR.Forms.Fabrica.Domain.Enums;
using AMR.Forms.Fabrica.Domain.Interfaces;

namespace AMR.Forms.Fabrica.Infrastructure.Data;

public class RdsDbContext(DbContextOptions<RdsDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Ficha> Fichas => Set<Ficha>();
    public DbSet<Filial> Filiais => Set<Filial>();
    public DbSet<Passo> Passos => Set<Passo>();
    public DbSet<TipoOperacao> TiposOperacao => Set<TipoOperacao>();
    public DbSet<TipoOperacaoPasso> TiposOperacaoPasso => Set<TipoOperacaoPasso>();
    public DbSet<TipoOperacaoPassoCfg> TiposOperacaoPassoCfg => Set<TipoOperacaoPassoCfg>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<BusinessUnit> BusinessUnits => Set<BusinessUnit>();
    public DbSet<NotaFiscal> NotasFiscais => Set<NotaFiscal>();
    public DbSet<NotaFiscalDetalhe> NotasFiscaisDetalhe => Set<NotaFiscalDetalhe>();
    public DbSet<FichaBalanca> FichasBalanca => Set<FichaBalanca>();
    public DbSet<FichaLoadDetalhe> FichasLoadDetalhe => Set<FichaLoadDetalhe>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();
    public DbSet<LogSistema> LogsSistema => Set<LogSistema>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<BomItem> BomItens => Set<BomItem>();
    public DbSet<OrdemProducao> OrdensProducao => Set<OrdemProducao>();
    public DbSet<RastreabilidadeItem> RastreabilidadeItens => Set<RastreabilidadeItem>();
    public DbSet<Equipamento> Equipamentos => Set<Equipamento>();
    public DbSet<RegistroOee> RegistrosOee => Set<RegistroOee>();
    public DbSet<PlanoManutencao> PlanosManutencao => Set<PlanoManutencao>();
    public DbSet<OrdemManutencao> OrdensManutencao => Set<OrdemManutencao>();

    public async Task<int> CommitAsync(CancellationToken ct = default)
        => await SaveChangesAsync(ct);

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasDefaultSchema("rds");

        // ── Conversores DateOnly ──────────────────────────────────────────────
        var dateOnlyConverter = new ValueConverter<DateOnly, string>(
            v => v.ToString("yyyy-MM-dd"),
            v => DateOnly.Parse(v));

        var dateOnlyNullableConverter = new ValueConverter<DateOnly?, string?>(
            v => v.HasValue ? v.Value.ToString("yyyy-MM-dd") : null,
            v => v == null ? null : DateOnly.Parse(v));

        // ── FILIAL ────────────────────────────────────────────────────────────
        mb.Entity<Filial>(e =>
        {
            e.ToTable("FILIAL");
            e.HasKey(x => x.Codigo);
            e.Property(x => x.Codigo).HasColumnName("CD_FILIAL");
            e.Property(x => x.Nome).HasColumnName("NO_FILIAL").HasMaxLength(100);
            e.Property(x => x.CodigoBuDeposito).HasColumnName("CD_BU_DEPOSITO").HasMaxLength(20);
            e.Property(x => x.TipoImpressaoNf).HasColumnName("CD_TIPO_IMPRESSAO_NF");
        });

        // ── PASSO ─────────────────────────────────────────────────────────────
        mb.Entity<Passo>(e =>
        {
            e.ToTable("PASSO");
            e.HasKey(x => x.Codigo);
            e.Property(x => x.Codigo).HasColumnName("CD_PASSO");
            e.Property(x => x.Nome).HasColumnName("NO_PASSO").HasMaxLength(100);
        });

        // ── VEICULO ───────────────────────────────────────────────────────────
        mb.Entity<Veiculo>(e =>
        {
            e.ToTable("VEICULO");
            e.HasKey(x => x.Placa);
            e.Property(x => x.Placa).HasColumnName("CD_PLACA_VEICULO").HasMaxLength(10);
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.UfVeiculo).HasColumnName("CD_UF_VEICULO").HasMaxLength(2);
            e.Property(x => x.CodigoRntc).HasColumnName("CD_RNTC_VEICULO").HasMaxLength(30);
        });

        // ── BUSINESS_UNIT ─────────────────────────────────────────────────────
        mb.Entity<BusinessUnit>(e =>
        {
            e.ToTable("BUSINESS_UNIT");
            e.HasKey(x => x.Codigo);
            e.Property(x => x.Codigo).HasColumnName("CD_BUSINESS_UNIT").HasMaxLength(20);
            e.Property(x => x.Nome).HasColumnName("NO_BUSINESS_UNIT").HasMaxLength(100);
            e.Property(x => x.CodigoCompanhia).HasColumnName("CD_COMPANHIA").HasMaxLength(20);
            e.Property(x => x.CodigoAddressNumber).HasColumnName("CD_ADDRESS_NUMBER");
        });

        // ── TIPO_OPERACAO ─────────────────────────────────────────────────────
        mb.Entity<TipoOperacao>(e =>
        {
            e.ToTable("TIPO_OPERACAO");
            e.HasKey(x => x.Codigo);
            e.Property(x => x.Codigo).HasColumnName("CD_TIPO_OPERACAO");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.Nome).HasColumnName("NO_TIPO_OPERACAO").HasMaxLength(100);
            e.Property(x => x.ExpedicaoRecepcao)
             .HasColumnName("CD_EXPEDICAO_RECEPCAO")
             .HasMaxLength(1)
             .HasConversion(new ValueConverter<TipoExpedicaoRecepcao?, string?>(
                 v => v == null ? null : v == TipoExpedicaoRecepcao.Expedicao ? "E" : "R",
                 v => v == "E" ? TipoExpedicaoRecepcao.Expedicao
                    : v == "R" ? TipoExpedicaoRecepcao.Recepcao
                    : null));
            e.Property(x => x.Interfaces).HasColumnName("CD_INTERFACES").HasMaxLength(50);
        });

        // ── TIPO_OPERACAO_PASSO ───────────────────────────────────────────────
        mb.Entity<TipoOperacaoPasso>(e =>
        {
            e.ToTable("TIPO_OPERACAO_PASSO");
            e.HasKey(x => new { x.CodigoTipoOperacao, x.CodigoPasso });
            e.Property(x => x.CodigoTipoOperacao).HasColumnName("CD_TIPO_OPERACAO");
            e.Property(x => x.CodigoPasso).HasColumnName("CD_PASSO");
            e.Property(x => x.Sequencia).HasColumnName("NR_ORDEM");
        });

        // ── TIPO_OPERACAO_PASSO_CFG ───────────────────────────────────────────
        mb.Entity<TipoOperacaoPassoCfg>(e =>
        {
            e.ToTable("TIPO_OPERACAO_PASSO_CFG");
            e.HasNoKey();
            e.Property(x => x.CodigoTipoOperacao).HasColumnName("CD_TIPO_OPERACAO");
        });

        // ── PRODUTO ───────────────────────────────────────────────────────────
        mb.Entity<Produto>(e =>
        {
            e.ToTable("PRODUTO");
            e.HasKey(x => x.Codigo);
            e.Property(x => x.Codigo).HasColumnName("CD_PRODUTO");
            e.Property(x => x.CodigoBusinessUnit).HasColumnName("CD_BUSINESS_UNIT").HasMaxLength(20);
            e.Property(x => x.CodigoProdutoLongo).HasColumnName("CD_PRODUTO_LONGO").HasMaxLength(50);
            e.Property(x => x.Nome).HasColumnName("NO_PRODUTO").HasMaxLength(200);
            e.Property(x => x.CodigoEan).HasColumnName("CD_EAN").HasMaxLength(30);
            e.Property(x => x.UnidadeMedidaComercial).HasColumnName("CD_UMC").HasMaxLength(10);
            e.Property(x => x.UnidadeMedida).HasColumnName("CD_UM").HasMaxLength(10);
            // Campos BOM (Sprint 24)
            e.Property(x => x.TipoProduto).HasColumnName("CD_TIPO_PRODUTO").HasMaxLength(20);
            e.Property(x => x.LeadTimeDias).HasColumnName("NR_LEAD_TIME_DIAS").HasDefaultValue(0);
            e.Property(x => x.CustoPadrao).HasColumnName("VL_CUSTO_PADRAO").HasPrecision(18, 4).HasDefaultValue(0m);
            // Relacionamentos BOM
            e.HasMany(p => p.BomComoFabricado).WithOne(b => b.ProdutoPai).HasForeignKey(b => b.CodigoProdutoPai);
            e.HasMany(p => p.BomComoComponente).WithOne(b => b.ProdutoFilho).HasForeignKey(b => b.CodigoProdutoFilho);
        });

        // ── BOM_ITEM ──────────────────────────────────────────────────────────
        mb.Entity<BomItem>(e =>
        {
            e.ToTable("BOM_ITEM");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID_BOM_ITEM").ValueGeneratedOnAdd();
            e.Property(x => x.CodigoProdutoPai).HasColumnName("CD_PRODUTO_PAI");
            e.Property(x => x.CodigoProdutoFilho).HasColumnName("CD_PRODUTO_FILHO");
            e.Property(x => x.Quantidade).HasColumnName("QT_QUANTIDADE").HasPrecision(18, 6);
            e.Property(x => x.Nivel).HasColumnName("NR_NIVEL");
            e.Property(x => x.PercentualPerda).HasColumnName("PC_PERDA").HasPrecision(5, 2).HasDefaultValue(0m);
            e.Property(x => x.Ativo).HasColumnName("FL_ATIVO").HasDefaultValue(true);
            e.Property(x => x.CriadoEm).HasColumnName("DT_CRIADO_EM");
            e.Property(x => x.AtualizadoEm).HasColumnName("DT_ATUALIZADO_EM");
            e.Ignore(x => x.QuantidadeLiquida);
            e.HasIndex(x => new { x.CodigoProdutoPai, x.CodigoProdutoFilho }).IsUnique();
        });

        // ── ORDEM_PRODUCAO ────────────────────────────────────────────────────
        mb.Entity<OrdemProducao>(e =>
        {
            e.ToTable("ORDEM_PRODUCAO");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID_ORDEM_PRODUCAO").ValueGeneratedOnAdd();
            e.Property(x => x.Numero).HasColumnName("NR_ORDEM").HasMaxLength(30);
            e.Property(x => x.CodigoProduto).HasColumnName("CD_PRODUTO");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.QuantidadePlanejada).HasColumnName("QT_PLANEJADA").HasPrecision(18, 4);
            e.Property(x => x.QuantidadeProduzida).HasColumnName("QT_PRODUZIDA").HasPrecision(18, 4).HasDefaultValue(0m);
            e.Property(x => x.QuantidadeRejeitada).HasColumnName("QT_REJEITADA").HasPrecision(18, 4).HasDefaultValue(0m);
            e.Property(x => x.Status).HasColumnName("CD_STATUS").HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.DataAbertura).HasColumnName("DT_ABERTURA");
            e.Property(x => x.DataPrevistaFim).HasColumnName("DT_PREVISTA_FIM");
            e.Property(x => x.DataFechamento).HasColumnName("DT_FECHAMENTO");
            e.Property(x => x.ObservacaoGeral).HasColumnName("DS_OBSERVACAO").HasMaxLength(500);
            e.Property(x => x.MotivoCancelamento).HasColumnName("DS_MOTIVO_CANCELAMENTO").HasMaxLength(500);
            e.Ignore(x => x.QuantidadeRestante);
            e.Ignore(x => x.PercentualConclusao);
            e.HasIndex(x => x.Numero).IsUnique();
            e.HasMany(o => o.Rastreabilidade).WithOne(r => r.OrdemProducao).HasForeignKey(r => r.OrdemProducaoId);
        });

        // ── RASTREABILIDADE_ITEM ──────────────────────────────────────────────
        mb.Entity<RastreabilidadeItem>(e =>
        {
            e.ToTable("RASTREABILIDADE_ITEM");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID_RASTREABILIDADE").ValueGeneratedOnAdd();
            e.Property(x => x.OrdemProducaoId).HasColumnName("ID_ORDEM_PRODUCAO");
            e.Property(x => x.CodigoProduto).HasColumnName("CD_PRODUTO");
            e.Property(x => x.Lote).HasColumnName("CD_LOTE").HasMaxLength(50);
            e.Property(x => x.Quantidade).HasColumnName("QT_QUANTIDADE").HasPrecision(18, 4);
            e.Property(x => x.TipoMovimento).HasColumnName("CD_TIPO_MOVIMENTO").HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.DataHoraRegistro).HasColumnName("DT_REGISTRO");
            e.Property(x => x.CodigoOperador).HasColumnName("CD_OPERADOR").HasMaxLength(20);
            e.Property(x => x.Observacao).HasColumnName("DS_OBSERVACAO").HasMaxLength(300);
            e.HasIndex(x => x.Lote);
            e.HasIndex(x => x.OrdemProducaoId);
        });

        // ── FICHA ─────────────────────────────────────────────────────────────
        mb.Entity<Ficha>(e =>
        {
            e.ToTable("FICHA");
            e.HasKey(x => x.Codigo);
            e.Property(x => x.Codigo).HasColumnName("CD_FICHA");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.PlacaVeiculo).HasColumnName("CD_PLACA_VEICULO").HasMaxLength(10);
            e.Property(x => x.CodigoBusinessUnit).HasColumnName("CD_BUSINESS_UNIT").HasMaxLength(20);
            e.Property(x => x.CodigoTipoOperacao).HasColumnName("CD_TIPO_OPERACAO");
            e.Property(x => x.CodigoPassoAtual).HasColumnName("CD_PASSO_ATUAL");
            e.Property(x => x.CodigoLotId).HasColumnName("CD_LOTID").HasMaxLength(50);
            e.Property(x => x.DataFicha).HasColumnName("DT_FICHA").HasConversion(dateOnlyNullableConverter);
            e.Property(x => x.DataSaida).HasColumnName("DT_SAIDA").HasConversion(dateOnlyNullableConverter);
            e.Property(x => x.DataInterfaceJde).HasColumnName("DT_INTERFACE_JDE").HasConversion(dateOnlyNullableConverter);
            e.Property(x => x.NomeMotorista).HasColumnName("NO_MOTORISTA").HasMaxLength(100);
            e.Property(x => x.CodigoContratoManifesto).HasColumnName("CD_CONTRATO_MANIFESTO").HasMaxLength(50);
            e.Property(x => x.CodigoTransportadora).HasColumnName("CD_ADDRESS_NUMBER_TRA");
            e.Property(x => x.CodigoProdutoDepto).HasColumnName("CD_PRODUTO_DEPTO");
            e.Property(x => x.CodigoSolicitacaoTransp).HasColumnName("CD_SOLICITACAO_TRANSP").HasMaxLength(50);
            e.Property(x => x.CodigoTipoDoctoJde).HasColumnName("CD_TIPO_DOCTO_JDE").HasMaxLength(10);
            e.Ignore(x => x.EstaFinalizada);
            e.Ignore(x => x.IntegradaComJde);
        });

        // ── NOTA_FISCAL ───────────────────────────────────────────────────────
        mb.Entity<NotaFiscal>(e =>
        {
            e.ToTable("NOTA_FISCAL");
            e.HasKey(x => new { x.Numero, x.SerieNotaFiscal });
            e.Property(x => x.Numero).HasColumnName("CD_NOTA_FISCAL");
            e.Property(x => x.SerieNotaFiscal).HasColumnName("CD_SER_NOTA_FISCAL").HasMaxLength(10);
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.CodigoFicha).HasColumnName("CD_FICHA");
            e.Property(x => x.CodigoBusinessUnit).HasColumnName("CD_BUSINESS_UNIT").HasMaxLength(20);
            e.Property(x => x.NomeCliente).HasColumnName("NO_CLIENTE").HasMaxLength(200);
            e.Property(x => x.ChaveNfe).HasColumnName("CD_CHAVE_NFE").HasMaxLength(50);
            e.Property(x => x.Protocolo).HasColumnName("CD_PROTOCOLO").HasMaxLength(50);
            e.Property(x => x.DataEmissao).HasColumnName("DT_EMISSAO_NF").HasConversion(dateOnlyNullableConverter);
            e.Property(x => x.CnpjCliente).HasColumnName("CD_CNPJ_CLIENTE").HasMaxLength(20);
            e.Property(x => x.ValorTransmissao).HasColumnName("VL_TRANSMISSAO").HasPrecision(18, 4);
            e.Ignore(x => x.Ambiente);
            e.Ignore(x => x.ModeloNf);
            e.Ignore(x => x.NomeFilial);
            e.Ignore(x => x.Impressoes);
            e.Ignore(x => x.Cancelado);
            e.Ignore(x => x.EstaCancelada);
            e.Ignore(x => x.EhAmbienteProducao);
            e.Ignore(x => x.FoiTransmitida);
        });

        // ── FICHA_BALANCA ─────────────────────────────────────────────────────
        mb.Entity<FichaBalanca>(e =>
        {
            e.ToTable("FICHA_BALANCA");
            e.HasNoKey();
            e.Property(x => x.CodigoFicha).HasColumnName("CD_FICHA");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.CodigoPesagem).HasColumnName("CD_PESAGEM");
            e.Property(x => x.OrigemDestino).HasColumnName("IC_ORIGEM_DESTINO").HasMaxLength(1);
            e.Property(x => x.Peso1Pesagem).HasColumnName("VL_PESO_1_PESAGEM").HasPrecision(18, 4);
            e.Property(x => x.Peso2Pesagem).HasColumnName("VL_PESO_2_PESAGEM").HasPrecision(18, 4);
            e.Ignore(x => x.PesoLiquido);
        });

        // ── FICHA_LOAD_DETALHE ────────────────────────────────────────────────
        mb.Entity<FichaLoadDetalhe>(e =>
        {
            e.ToTable("FICHA_LOAD_DETALHE");
            e.HasNoKey();
            e.Property(x => x.CodigoFicha).HasColumnName("CD_FICHA");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.CodigoBusinessUnit).HasColumnName("CD_BUSINESS_UNIT").HasMaxLength(20);
            e.Property(x => x.CodigoProduto).HasColumnName("CD_PRODUTO");
            e.Property(x => x.Quantidade).HasColumnName("QT_PRODUTO").HasPrecision(18, 4);
            e.Property(x => x.UnidadeMedida).HasColumnName("CD_UM").HasMaxLength(10);
            e.Property(x => x.CodigoAddressNumber).HasColumnName("CD_ADDRESS_NUMBER");
            e.Property(x => x.CodigoTipoDoctoJde).HasColumnName("CD_TIPO_DOCTO_JDE").HasMaxLength(10);
        });

        // ── PEDIDO ────────────────────────────────────────────────────────────
        mb.Entity<Pedido>(e =>
        {
            e.ToTable("PEDIDO");
            e.HasKey(x => x.Codigo);
            e.Property(x => x.Codigo).HasColumnName("CD_PEDIDO");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.CodigoBusinessUnit).HasColumnName("CD_BUSINESS_UNIT").HasMaxLength(20);
            e.Property(x => x.CodigoAddressNumber).HasColumnName("CD_ADDRESS_NUMBER");
            e.Property(x => x.DataPedido).HasColumnName("DT_PEDIDO").HasConversion(dateOnlyNullableConverter);
            e.Ignore(x => x.Itens);
            e.Ignore(x => x.QuantidadeTotalProdutos);
            e.Property(x => x.CodigoTipoDoctoJde).HasColumnName("CD_TIPO_DOCTO_JDE").HasMaxLength(10);
            e.Property(x => x.SincronizadoEm).HasColumnName("SincronizadoEm");
        });

        // ── PEDIDO_ITEM ───────────────────────────────────────────────────────
        mb.Entity<PedidoItem>(e =>
        {
            e.ToTable("PEDIDO_ITEM");
            e.HasNoKey();
            e.Property(x => x.CodigoPedido).HasColumnName("CD_PEDIDO");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.CodigoProduto).HasColumnName("CD_PRODUTO");
            e.Property(x => x.Quantidade).HasColumnName("QT_PRODUTO").HasPrecision(18, 4);
            e.Property(x => x.UnidadeMedida).HasColumnName("CD_UM").HasMaxLength(10);
        });

        // ── LOG_SISTEMA ───────────────────────────────────────────────────────
        mb.Entity<LogSistema>(e =>
        {
            e.ToTable("LOG_SISTEMA");
            e.HasNoKey();
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.DescricaoErro).HasColumnName("DS_MENSAGEM").HasMaxLength(500);
            e.Property(x => x.DataLog).HasColumnName("DT_LOG");
            e.Property(x => x.Pendente).HasColumnName("IC_PENDENTE");
        });

        // ── DEPARTAMENTO ──────────────────────────────────────────────────────
        mb.Entity<Departamento>(e =>
        {
            e.ToTable("DEPARTAMENTO");
            e.HasKey(x => x.Codigo);
            e.Property(x => x.Codigo).HasColumnName("CD_DEPARTAMENTO");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.DescricaoProduto).HasColumnName("NO_DEPARTAMENTO").HasMaxLength(100);
        });

        // ── EQUIPAMENTO ───────────────────────────────────────────────────────
        mb.Entity<Equipamento>(e =>
        {
            e.ToTable("EQUIPAMENTO");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID_EQUIPAMENTO").ValueGeneratedOnAdd();
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.Nome).HasColumnName("NO_EQUIPAMENTO").HasMaxLength(100);
            e.Property(x => x.Descricao).HasColumnName("DS_EQUIPAMENTO").HasMaxLength(300);
            e.Property(x => x.CodigoArea).HasColumnName("CD_AREA").HasMaxLength(50);
            e.Property(x => x.Ativo).HasColumnName("FL_ATIVO").HasDefaultValue(true);
            e.Property(x => x.CriadoEm).HasColumnName("DT_CRIADO_EM");
            e.HasMany(eq => eq.RegistrosOee).WithOne(r => r.Equipamento).HasForeignKey(r => r.EquipamentoId);
        });

        // ── REGISTRO_OEE ──────────────────────────────────────────────────────
        mb.Entity<RegistroOee>(e =>
        {
            e.ToTable("REGISTRO_OEE");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID_REGISTRO_OEE").ValueGeneratedOnAdd();
            e.Property(x => x.EquipamentoId).HasColumnName("ID_EQUIPAMENTO");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.DataHoraInicio).HasColumnName("DT_HORA_INICIO");
            e.Property(x => x.DataHoraFim).HasColumnName("DT_HORA_FIM");
            e.Property(x => x.TempoPlanejadoMinutos).HasColumnName("NR_TEMPO_PLANEJADO_MIN");
            e.Property(x => x.TempoRealProducaoMinutos).HasColumnName("NR_TEMPO_REAL_MIN");
            e.Property(x => x.QuantidadeProduzida).HasColumnName("QT_PRODUZIDA").HasPrecision(18, 4);
            e.Property(x => x.QuantidadeAprovada).HasColumnName("QT_APROVADA").HasPrecision(18, 4);
            e.Property(x => x.TempoCicloIdealSegundos).HasColumnName("NR_CICLO_IDEAL_SEG").HasPrecision(10, 3);
            e.Property(x => x.CodigoOperador).HasColumnName("CD_OPERADOR").HasMaxLength(20);
            e.Property(x => x.Observacao).HasColumnName("DS_OBSERVACAO").HasMaxLength(300);
            e.Property(x => x.CriadoEm).HasColumnName("DT_CRIADO_EM");
            // Computed props: ignoradas pelo EF (calculadas na entidade)
            e.Ignore(x => x.Disponibilidade);
            e.Ignore(x => x.Performance);
            e.Ignore(x => x.Qualidade);
            e.Ignore(x => x.Oee);
            e.HasIndex(x => new { x.EquipamentoId, x.DataHoraInicio });
            e.HasIndex(x => x.CodigoFilial);
        });

        // ── PLANO_MANUTENCAO ──────────────────────────────────────────────────
        mb.Entity<PlanoManutencao>(e =>
        {
            e.ToTable("PLANO_MANUTENCAO");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID_PLANO_MANUTENCAO").ValueGeneratedOnAdd();
            e.Property(x => x.EquipamentoId).HasColumnName("ID_EQUIPAMENTO");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.TipoManutencao).HasColumnName("CD_TIPO_MANUTENCAO").HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.Descricao).HasColumnName("DS_DESCRICAO").HasMaxLength(200);
            e.Property(x => x.Instrucoes).HasColumnName("DS_INSTRUCOES").HasMaxLength(2000);
            e.Property(x => x.FrequenciaDias).HasColumnName("NR_FREQUENCIA_DIAS");
            e.Property(x => x.DuracaoEstimadaHoras).HasColumnName("NR_DURACAO_EST_HORAS").HasPrecision(8, 2);
            e.Property(x => x.ProximaExecucao).HasColumnName("DT_PROXIMA_EXECUCAO");
            e.Property(x => x.UltimaExecucao).HasColumnName("DT_ULTIMA_EXECUCAO");
            e.Property(x => x.Ativo).HasColumnName("FL_ATIVO").HasDefaultValue(true);
            e.Property(x => x.CriadoEm).HasColumnName("DT_CRIADO_EM");
            e.HasMany(p => p.OrdensManutencao).WithOne(o => o.PlanoManutencao).HasForeignKey(o => o.PlanoManutencaoId);
            e.HasIndex(x => new { x.CodigoFilial, x.ProximaExecucao });
        });

        // ── ORDEM_MANUTENCAO ──────────────────────────────────────────────────
        mb.Entity<OrdemManutencao>(e =>
        {
            e.ToTable("ORDEM_MANUTENCAO");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID_ORDEM_MANUTENCAO").ValueGeneratedOnAdd();
            e.Property(x => x.PlanoManutencaoId).HasColumnName("ID_PLANO_MANUTENCAO");
            e.Property(x => x.EquipamentoId).HasColumnName("ID_EQUIPAMENTO");
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.TipoManutencao).HasColumnName("CD_TIPO_MANUTENCAO").HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.Descricao).HasColumnName("DS_DESCRICAO").HasMaxLength(200);
            e.Property(x => x.Status).HasColumnName("CD_STATUS").HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.DataPrevista).HasColumnName("DT_PREVISTA");
            e.Property(x => x.DataInicio).HasColumnName("DT_INICIO");
            e.Property(x => x.DataConclusao).HasColumnName("DT_CONCLUSAO");
            e.Property(x => x.DuracaoRealHoras).HasColumnName("NR_DURACAO_REAL_HORAS").HasPrecision(8, 2);
            e.Property(x => x.CodigoTecnico).HasColumnName("CD_TECNICO").HasMaxLength(20);
            e.Property(x => x.Observacao).HasColumnName("DS_OBSERVACAO").HasMaxLength(500);
            e.Property(x => x.MotivoCancelamento).HasColumnName("DS_MOTIVO_CANCELAMENTO").HasMaxLength(500);
            e.Property(x => x.CriadoEm).HasColumnName("DT_CRIADO_EM");
            e.Ignore(x => x.AtrasoEmDias);
            e.HasIndex(x => new { x.CodigoFilial, x.Status });
            e.HasIndex(x => x.EquipamentoId);
        });

        // ── NOTA_FISCAL_DETALHE ───────────────────────────────────────────────
        mb.Entity<NotaFiscalDetalhe>(e =>
        {
            e.ToTable("NOTA_FISCAL_DETALHE");
            e.HasNoKey();
            e.Property(x => x.NumeroNotaFiscal).HasColumnName("CD_NOTA_FISCAL");
            e.Property(x => x.SerieNotaFiscal).HasColumnName("CD_SER_NOTA_FISCAL").HasMaxLength(10);
            e.Property(x => x.CodigoFilial).HasColumnName("CD_FILIAL");
            e.Property(x => x.CodigoProduto).HasColumnName("CD_PRODUTO");
            e.Property(x => x.Quantidade).HasColumnName("QT_PRODUTO").HasPrecision(18, 4);
            e.Property(x => x.PrecoUnitario).HasColumnName("VL_PRECO_UNITARIO").HasPrecision(18, 4);
            e.Property(x => x.ValorTotal).HasColumnName("VL_TOTAL").HasPrecision(18, 4);
            e.Property(x => x.AliquotaIcms).HasColumnName("VL_ALIQUOTA_ICMS").HasPrecision(18, 4);
            e.Property(x => x.BaseReducaoIcms).HasColumnName("VL_BASE_REDUCAO_ICMS").HasPrecision(18, 4);
            e.Property(x => x.AliquotaIpi).HasColumnName("VL_ALIQUOTA_IPI").HasPrecision(18, 4);
            e.Property(x => x.ValorIpi).HasColumnName("VL_IPI").HasPrecision(18, 4);
            e.Property(x => x.ValorIcmsSt).HasColumnName("VL_ICMS_ST").HasPrecision(18, 4);
            e.Property(x => x.ValorPis).HasColumnName("VL_PIS").HasPrecision(18, 4);
            e.Property(x => x.ValorCofins).HasColumnName("VL_COFINS").HasPrecision(18, 4);
            e.Ignore(x => x.ValorTotalComIpi);
        });
    }
}
