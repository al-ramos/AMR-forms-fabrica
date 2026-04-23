using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RDS.Forms.Fabrica.Infrastructure.Data.Entities;

namespace RDS.Forms.Fabrica.Infrastructure.Data;

public partial class RdsFormasFabricaDbContext : DbContext
{
    public RdsFormasFabricaDbContext(DbContextOptions<RdsFormasFabricaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AddressBook> AddressBooks { get; set; }
    public virtual DbSet<AddressBookExtended> AddressBookExtendeds { get; set; }
    public virtual DbSet<BusinessUnit> BusinessUnits { get; set; }
    public virtual DbSet<ControleSerieNf> ControleSerieNfs { get; set; }
    public virtual DbSet<Departamento> Departamentos { get; set; }
    public virtual DbSet<DepartamentoProduto> DepartamentoProdutos { get; set; }
    public virtual DbSet<Estacao> Estacaos { get; set; }
    public virtual DbSet<EstacaoPasso> EstacaoPassos { get; set; }
    public virtual DbSet<Ficha> Fichas { get; set; }
    public virtual DbSet<FichaBalanca> FichaBalancas { get; set; }
    public virtual DbSet<FichaLoadDetalhe> FichaLoadDetalhes { get; set; }
    public virtual DbSet<FichaNotaFiscalDetalhe> FichaNotaFiscalDetalhes { get; set; }
    public virtual DbSet<FichaQualidadeNf> FichaQualidadeNfs { get; set; }
    public virtual DbSet<Filial> Filials { get; set; }
    public virtual DbSet<FilialBusinessUnit> FilialBusinessUnits { get; set; }
    public virtual DbSet<InterfaceParametro> InterfaceParametros { get; set; }
    public virtual DbSet<LogSistema> LogSistemas { get; set; }
    public virtual DbSet<MensagemNotaFiscal> MensagemNotaFiscals { get; set; }
    public virtual DbSet<ModalidadeFrete> ModalidadeFretes { get; set; }
    public virtual DbSet<NaturezaOperacao> NaturezaOperacaos { get; set; }
    public virtual DbSet<NotaFiscal> NotaFiscals { get; set; }
    public virtual DbSet<NotaFiscalDetalhe> NotaFiscalDetalhes { get; set; }
    public virtual DbSet<Passo> Passos { get; set; }
    public virtual DbSet<Pedido> Pedidos { get; set; }
    public virtual DbSet<PedidoItem> PedidoItems { get; set; }
    public virtual DbSet<Produto> Produtos { get; set; }
    public virtual DbSet<ProdutoDetalhe> ProdutoDetalhes { get; set; }
    public virtual DbSet<RegimeTributario> RegimeTributarios { get; set; }
    public virtual DbSet<Smartcard> Smartcards { get; set; }
    public virtual DbSet<TipoOperacao> TipoOperacaos { get; set; }
    public virtual DbSet<TipoOperacaoPasso> TipoOperacaoPassos { get; set; }
    public virtual DbSet<TipoOperacaoPassoCfg> TipoOperacaoPassoCfgs { get; set; }
    public virtual DbSet<Veiculo> Veiculos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ─── Mapeamento de tabelas (UPPER_SNAKE_CASE) ─────────────────────
        modelBuilder.Entity<AddressBook>().ToTable("ADDRESS_BOOK", "rds");
        modelBuilder.Entity<AddressBookExtended>().ToTable("ADDRESS_BOOK_EXTENDED", "rds");
        modelBuilder.Entity<BusinessUnit>().ToTable("BUSINESS_UNIT", "rds");
        modelBuilder.Entity<ControleSerieNf>().ToTable("CONTROLE_SERIE_NF", "rds");
        modelBuilder.Entity<Departamento>().ToTable("DEPARTAMENTO", "rds");
        modelBuilder.Entity<DepartamentoProduto>().ToTable("DEPARTAMENTO_PRODUTO", "rds");
        modelBuilder.Entity<Estacao>().ToTable("ESTACAO", "rds");
        modelBuilder.Entity<EstacaoPasso>().ToTable("ESTACAO_PASSO", "rds");
        modelBuilder.Entity<Ficha>().ToTable("FICHA", "rds");
        modelBuilder.Entity<FichaBalanca>().ToTable("FICHA_BALANCA", "rds");
        modelBuilder.Entity<FichaLoadDetalhe>().ToTable("FICHA_LOAD_DETALHE", "rds");
        modelBuilder.Entity<FichaNotaFiscalDetalhe>().ToTable("FICHA_NOTA_FISCAL_DETALHE", "rds");
        modelBuilder.Entity<FichaQualidadeNf>().ToTable("FICHA_QUALIDADE_NF", "rds");
        modelBuilder.Entity<Filial>().ToTable("FILIAL", "rds");
        modelBuilder.Entity<FilialBusinessUnit>().ToTable("FILIAL_BUSINESS_UNIT", "rds");
        modelBuilder.Entity<InterfaceParametro>().ToTable("INTERFACE_PARAMETRO", "rds");
        modelBuilder.Entity<LogSistema>().ToTable("LOG_SISTEMA", "rds");
        modelBuilder.Entity<MensagemNotaFiscal>().ToTable("MENSAGEM_NOTA_FISCAL", "rds");
        modelBuilder.Entity<ModalidadeFrete>().ToTable("MODALIDADE_FRETE", "rds");
        modelBuilder.Entity<NaturezaOperacao>().ToTable("NATUREZA_OPERACAO", "rds");
        modelBuilder.Entity<NotaFiscal>().ToTable("NOTA_FISCAL", "rds");
        modelBuilder.Entity<NotaFiscalDetalhe>().ToTable("NOTA_FISCAL_DETALHE", "rds");
        modelBuilder.Entity<Passo>().ToTable("PASSO", "rds");
        modelBuilder.Entity<Pedido>().ToTable("PEDIDO", "rds");
        modelBuilder.Entity<PedidoItem>().ToTable("PEDIDO_ITEM", "rds");
        modelBuilder.Entity<Produto>().ToTable("PRODUTO", "rds");
        modelBuilder.Entity<ProdutoDetalhe>().ToTable("PRODUTO_DETALHE", "rds");
        modelBuilder.Entity<RegimeTributario>().ToTable("REGIME_TRIBUTARIO", "rds");
        modelBuilder.Entity<Smartcard>().ToTable("SMARTCARD", "rds");
        modelBuilder.Entity<TipoOperacao>().ToTable("TIPO_OPERACAO", "rds");
        modelBuilder.Entity<TipoOperacaoPasso>().ToTable("TIPO_OPERACAO_PASSO", "rds");
        modelBuilder.Entity<TipoOperacaoPassoCfg>().ToTable("TIPO_OPERACAO_PASSO_CFG", "rds");
        modelBuilder.Entity<Veiculo>().ToTable("VEICULO", "rds");

        // ─── Configurações de entidades ───────────────────────────────────

        modelBuilder.Entity<AddressBook>(entity =>
        {
            entity.Property(e => e.CdAddressNumber).ValueGeneratedNever();
            entity.Property(e => e.CdUf).IsFixedLength();
        });

        modelBuilder.Entity<AddressBookExtended>(entity =>
        {
            entity.HasOne(d => d.CdAddressNumberNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ADDRESS_BOOK_EXTENDED_ADDRESS_BOOK");
        });

        modelBuilder.Entity<BusinessUnit>(entity =>
        {
            entity.HasOne(d => d.CdAddressNumberNavigation).WithMany(p => p.BusinessUnits)
                .HasConstraintName("FK_BUSINESS_UNIT_ADDRESS_BOOK");
        });

        modelBuilder.Entity<ControleSerieNf>(entity =>
        {
            entity.HasOne(d => d.CdFilialNavigation).WithMany(p => p.ControleSerieNfs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CONTROLE_SERIE_NF_FILIAL");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.Property(e => e.CdDepartamento).ValueGeneratedNever();
            entity.HasOne(d => d.CdFilialNavigation).WithMany(p => p.Departamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DEPARTAMENTO_FILIAL");
        });

        modelBuilder.Entity<DepartamentoProduto>(entity =>
        {
            entity.HasOne(d => d.CdDepartamentoNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DP_DEPARTAMENTO");
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DP_FILIAL");
            entity.HasOne(d => d.CdProdutoNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DP_PRODUTO");
        });

        modelBuilder.Entity<Estacao>(entity =>
        {
            entity.Property(e => e.CdEstacao).ValueGeneratedNever();
            entity.HasOne(d => d.CdFilialNavigation).WithMany(p => p.Estacaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ESTACAO_FILIAL");
        });

        modelBuilder.Entity<EstacaoPasso>(entity =>
        {
            entity.HasOne(d => d.CdEstacaoNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ESTACAO_PASSO_ESTACAO");
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ESTACAO_PASSO_FILIAL");
            entity.HasOne(d => d.CdPassoNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ESTACAO_PASSO_PASSO");
        });

        modelBuilder.Entity<Ficha>(entity =>
        {
            entity.HasOne(d => d.CdAddressNumberTraNavigation).WithMany(p => p.Fichas)
                .HasConstraintName("FK_FICHA_ADDRESS_BOOK_TRA");
            entity.HasOne(d => d.CdBusinessUnitNavigation).WithMany(p => p.Fichas)
                .HasConstraintName("FK_FICHA_BUSINESS_UNIT");
            entity.HasOne(d => d.CdFilialNavigation).WithMany(p => p.Fichas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FICHA_FILIAL");
            entity.HasOne(d => d.CdPassoAtualNavigation).WithMany(p => p.Fichas)
                .HasConstraintName("FK_FICHA_PASSO");
            entity.HasOne(d => d.CdPlacaVeiculoNavigation).WithMany(p => p.Fichas)
                .HasConstraintName("FK_FICHA_VEICULO");
            entity.HasOne(d => d.CdTipoOperacaoNavigation).WithMany(p => p.Fichas)
                .HasConstraintName("FK_FICHA_TIPO_OPERACAO");
        });

        modelBuilder.Entity<FichaBalanca>(entity =>
        {
            entity.Property(e => e.IcOrigemDestino).IsFixedLength();
            entity.HasOne(d => d.CdFichaNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FICHA_BALANCA_FICHA");
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FICHA_BALANCA_FILIAL");
        });

        modelBuilder.Entity<FichaLoadDetalhe>(entity =>
        {
            entity.HasOne(d => d.CdAddressNumberNavigation).WithMany()
                .HasConstraintName("FK_FLD_ADDRESS_BOOK");
            entity.HasOne(d => d.CdBusinessUnitNavigation).WithMany()
                .HasConstraintName("FK_FLD_BUSINESS_UNIT");
            entity.HasOne(d => d.CdFichaNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLD_FICHA");
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLD_FILIAL");
            entity.HasOne(d => d.CdProdutoNavigation).WithMany()
                .HasConstraintName("FK_FLD_PRODUTO");
        });

        modelBuilder.Entity<FichaNotaFiscalDetalhe>(entity =>
        {
            entity.HasOne(d => d.CdAddressNumberNavigation).WithMany()
                .HasConstraintName("FK_FNFD_ADDRESS_BOOK");
            entity.HasOne(d => d.CdBusinessUnitNavigation).WithMany()
                .HasConstraintName("FK_FNFD_BUSINESS_UNIT");
            entity.HasOne(d => d.CdFichaNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FNFD_FICHA");
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FNFD_FILIAL");
            entity.HasOne(d => d.CdProdutoNavigation).WithMany()
                .HasConstraintName("FK_FNFD_PRODUTO");
            entity.HasOne(d => d.NotaFiscal).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FNFD_NOTA_FISCAL");
        });

        modelBuilder.Entity<FichaQualidadeNf>(entity =>
        {
            entity.HasOne(d => d.CdAddressNumberNavigation).WithMany()
                .HasConstraintName("FK_FQNF_ADDRESS_BOOK");
            entity.HasOne(d => d.CdBusinessUnitNavigation).WithMany()
                .HasConstraintName("FK_FQNF_BUSINESS_UNIT");
            entity.HasOne(d => d.CdFichaNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FQNF_FICHA");
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FQNF_FILIAL");
            entity.HasOne(d => d.CdProdutoNavigation).WithMany()
                .HasConstraintName("FK_FQNF_PRODUTO");
            entity.HasOne(d => d.NotaFiscal).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FQNF_NOTA_FISCAL");
        });

        modelBuilder.Entity<Filial>(entity =>
        {
            entity.Property(e => e.CdFilial).ValueGeneratedNever();
        });

        modelBuilder.Entity<FilialBusinessUnit>(entity =>
        {
            entity.HasOne(d => d.CdBusinessUnitNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FBU_BUSINESS_UNIT");
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FBU_FILIAL");
        });

        modelBuilder.Entity<InterfaceParametro>(entity =>
        {
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_INTERFACE_PARAMETRO_FILIAL");
        });

        modelBuilder.Entity<LogSistema>(entity =>
        {
            entity.HasOne(d => d.CdFichaNavigation).WithMany()
                .HasConstraintName("FK_LOG_SISTEMA_FICHA");
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LOG_SISTEMA_FILIAL");
        });

        modelBuilder.Entity<MensagemNotaFiscal>(entity =>
        {
            entity.Property(e => e.CdMensagem).ValueGeneratedNever();
        });

        modelBuilder.Entity<ModalidadeFrete>(entity =>
        {
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MODALIDADE_FRETE_FILIAL");
        });

        modelBuilder.Entity<NotaFiscal>(entity =>
        {
            entity.HasOne(d => d.CdBusinessUnitNavigation).WithMany(p => p.NotaFiscals)
                .HasConstraintName("FK_NF_BUSINESS_UNIT");
            entity.HasOne(d => d.CdFichaNavigation).WithMany(p => p.NotaFiscals)
                .HasConstraintName("FK_NF_FICHA");
            entity.HasOne(d => d.CdFilialNavigation).WithMany(p => p.NotaFiscals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NF_FILIAL");
        });

        modelBuilder.Entity<NotaFiscalDetalhe>(entity =>
        {
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NFD_FILIAL");
            entity.HasOne(d => d.CdProdutoNavigation).WithMany()
                .HasConstraintName("FK_NFD_PRODUTO");
            entity.HasOne(d => d.NotaFiscal).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NFD_NOTA_FISCAL");
        });

        modelBuilder.Entity<Passo>(entity =>
        {
            entity.Property(e => e.CdPasso).ValueGeneratedNever();
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.Property(e => e.CdPedido).ValueGeneratedNever();
            entity.HasOne(d => d.CdAddressNumberNavigation).WithMany(p => p.Pedidos)
                .HasConstraintName("FK_PEDIDO_ADDRESS_BOOK");
            entity.HasOne(d => d.CdBusinessUnitNavigation).WithMany(p => p.Pedidos)
                .HasConstraintName("FK_PEDIDO_BUSINESS_UNIT");
            entity.HasOne(d => d.CdFilialNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PEDIDO_FILIAL");
        });

        modelBuilder.Entity<PedidoItem>(entity =>
        {
            entity.HasOne(d => d.CdAddressNumberNavigation).WithMany()
                .HasConstraintName("FK_PI_ADDRESS_BOOK");
            entity.HasOne(d => d.CdBusinessUnitNavigation).WithMany()
                .HasConstraintName("FK_PI_BUSINESS_UNIT");
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PI_FILIAL");
            entity.HasOne(d => d.CdPedidoNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PI_PEDIDO");
            entity.HasOne(d => d.CdProdutoNavigation).WithMany()
                .HasConstraintName("FK_PI_PRODUTO");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.Property(e => e.CdProduto).ValueGeneratedNever();
            entity.HasOne(d => d.CdBusinessUnitNavigation).WithMany(p => p.Produtos)
                .HasConstraintName("FK_PRODUTO_BUSINESS_UNIT");
        });

        modelBuilder.Entity<ProdutoDetalhe>(entity =>
        {
            entity.HasOne(d => d.CdBusinessUnitNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRODUTO_DETALHE_BUSINESS_UNIT");
            entity.HasOne(d => d.CdProdutoNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRODUTO_DETALHE_PRODUTO");
        });

        modelBuilder.Entity<RegimeTributario>(entity =>
        {
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_REGIME_TRIBUTARIO_FILIAL");
        });

        modelBuilder.Entity<Smartcard>(entity =>
        {
            entity.HasOne(d => d.CdFichaNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SMARTCARD_FICHA");
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SMARTCARD_FILIAL");
        });

        modelBuilder.Entity<TipoOperacao>(entity =>
        {
            entity.Property(e => e.CdTipoOperacao).ValueGeneratedNever();
            entity.Property(e => e.CdExpedicaoRecepcao).IsFixedLength();
            entity.HasOne(d => d.CdFilialNavigation).WithMany(p => p.TipoOperacaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TIPO_OPERACAO_FILIAL");
        });

        modelBuilder.Entity<TipoOperacaoPasso>(entity =>
        {
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TOP_FILIAL");
            entity.HasOne(d => d.CdPassoNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TOP_PASSO");
            entity.HasOne(d => d.CdTipoOperacaoNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TOP_TIPO_OPERACAO");
        });

        modelBuilder.Entity<TipoOperacaoPassoCfg>(entity =>
        {
            entity.Property(e => e.CdTipo).IsFixedLength();
            entity.HasOne(d => d.CdFilialNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TOPC_FILIAL");
            entity.HasOne(d => d.CdTipoOperacaoNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TOPC_TIPO_OPERACAO");
        });

        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.HasOne(d => d.CdFilialNavigation).WithMany(p => p.Veiculos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VEICULO_FILIAL");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
