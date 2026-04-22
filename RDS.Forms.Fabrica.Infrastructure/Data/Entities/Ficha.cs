using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("FICHA", Schema = "rds")]
[Index("DtFicha", Name = "IX_FICHA_DT_FICHA")]
[Index("CdFilial", Name = "IX_FICHA_FILIAL")]
[Index("CdPlacaVeiculo", Name = "IX_FICHA_PLACA_VEICULO")]
[Index("CdTipoOperacao", Name = "IX_FICHA_TIPO_OPERACAO")]
public partial class Ficha
{
    [Key]
    [Column("CD_FICHA")]
    public int CdFicha { get; set; }

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_PLACA_VEICULO")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdPlacaVeiculo { get; set; }

    [Column("CD_BUSINESS_UNIT")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CdBusinessUnit { get; set; }

    [Column("CD_TIPO_OPERACAO")]
    public int? CdTipoOperacao { get; set; }

    [Column("CD_PASSO_ATUAL")]
    public int? CdPassoAtual { get; set; }

    [Column("CD_LOTID")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CdLotid { get; set; }

    [Column("DT_FICHA")]
    public DateOnly? DtFicha { get; set; }

    [Column("DT_SAIDA")]
    public DateOnly? DtSaida { get; set; }

    [Column("DT_INTERFACE_JDE")]
    public DateOnly? DtInterfaceJde { get; set; }

    [Column("NO_MOTORISTA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoMotorista { get; set; }

    [Column("CD_CONTRATO_MANIFESTO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CdContratoManifesto { get; set; }

    [Column("CD_ADDRESS_NUMBER_TRA")]
    public int? CdAddressNumberTra { get; set; }

    [Column("CD_PRODUTO_DEPTO")]
    public int? CdProdutoDepto { get; set; }

    [Column("CD_SOLICITACAO_TRANSP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CdSolicitacaoTransp { get; set; }

    [Column("CD_TIPO_DOCTO_JDE")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdTipoDoctoJde { get; set; }

    [ForeignKey("CdAddressNumberTra")]
    [InverseProperty("Fichas")]
    public virtual AddressBook? CdAddressNumberTraNavigation { get; set; }

    [ForeignKey("CdBusinessUnit")]
    [InverseProperty("Fichas")]
    public virtual BusinessUnit? CdBusinessUnitNavigation { get; set; }

    [ForeignKey("CdFilial")]
    [InverseProperty("Fichas")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;

    [ForeignKey("CdPassoAtual")]
    [InverseProperty("Fichas")]
    public virtual Passo? CdPassoAtualNavigation { get; set; }

    [ForeignKey("CdPlacaVeiculo")]
    [InverseProperty("Fichas")]
    public virtual Veiculo? CdPlacaVeiculoNavigation { get; set; }

    [ForeignKey("CdTipoOperacao")]
    [InverseProperty("Fichas")]
    public virtual TipoOperacao? CdTipoOperacaoNavigation { get; set; }

    [InverseProperty("CdFichaNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscals { get; set; } = new List<NotaFiscal>();
}
