using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("FICHA_NOTA_FISCAL_DETALHE", Schema = "rds")]
public partial class FichaNotaFiscalDetalhe
{
    [Column("CD_FICHA")]
    public int CdFicha { get; set; }

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_NOTA_FISCAL")]
    public int CdNotaFiscal { get; set; }

    [Column("CD_SER_NOTA_FISCAL")]
    [StringLength(10)]
    [Unicode(false)]
    public string CdSerNotaFiscal { get; set; } = null!;

    [Column("CD_ADDRESS_NUMBER")]
    public int? CdAddressNumber { get; set; }

    [Column("CD_PRODUTO")]
    public int? CdProduto { get; set; }

    [Column("CD_BUSINESS_UNIT")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CdBusinessUnit { get; set; }

    [Column("NO_CONTRATO_MEXICO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NoContratoMexico { get; set; }

    [ForeignKey("CdAddressNumber")]
    public virtual AddressBook? CdAddressNumberNavigation { get; set; }

    [ForeignKey("CdBusinessUnit")]
    public virtual BusinessUnit? CdBusinessUnitNavigation { get; set; }

    [ForeignKey("CdFicha")]
    public virtual Ficha CdFichaNavigation { get; set; } = null!;

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;

    [ForeignKey("CdProduto")]
    public virtual Produto? CdProdutoNavigation { get; set; }

    [ForeignKey("CdNotaFiscal, CdSerNotaFiscal")]
    public virtual NotaFiscal NotaFiscal { get; set; } = null!;
}
