using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("NOTA_FISCAL_DETALHE", Schema = "rds")]
[Index("CdProduto", Name = "IX_NFD_PRODUTO")]
public partial class NotaFiscalDetalhe
{
    [Column("CD_NOTA_FISCAL")]
    public int CdNotaFiscal { get; set; }

    [Column("CD_SER_NOTA_FISCAL")]
    [StringLength(10)]
    [Unicode(false)]
    public string CdSerNotaFiscal { get; set; } = null!;

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_PRODUTO")]
    public int? CdProduto { get; set; }

    [Column("QT_PRODUTO", TypeName = "decimal(18, 4)")]
    public decimal? QtProduto { get; set; }

    [Column("CD_UMC")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdUmc { get; set; }

    [Column("CD_UNIDADE_MEDIDA")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdUnidadeMedida { get; set; }

    [Column("VL_PRECO_UNITARIO", TypeName = "decimal(18, 4)")]
    public decimal? VlPrecoUnitario { get; set; }

    [Column("VL_TOTAL", TypeName = "decimal(18, 4)")]
    public decimal? VlTotal { get; set; }

    [Column("VL_ALIQUOTA_ICMS", TypeName = "decimal(18, 4)")]
    public decimal? VlAliquotaIcms { get; set; }

    [Column("VL_BASE_REDUCAO_ICMS", TypeName = "decimal(18, 4)")]
    public decimal? VlBaseReducaoIcms { get; set; }

    [Column("VL_ALIQUOTA_IPI", TypeName = "decimal(18, 4)")]
    public decimal? VlAliquotaIpi { get; set; }

    [Column("VL_IPI", TypeName = "decimal(18, 4)")]
    public decimal? VlIpi { get; set; }

    [Column("CD_ICMS_CST")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdIcmsCst { get; set; }

    [Column("CD_ORIGEM_MERCADORIA")]
    [StringLength(5)]
    [Unicode(false)]
    public string? CdOrigemMercadoria { get; set; }

    [Column("CD_MODALIDAD_CALCULO_ICMS")]
    [StringLength(5)]
    [Unicode(false)]
    public string? CdModalidadCalculoIcms { get; set; }

    [Column("VL_ICMS_ST", TypeName = "decimal(18, 4)")]
    public decimal? VlIcmsSt { get; set; }

    [Column("VL_PIS", TypeName = "decimal(18, 4)")]
    public decimal? VlPis { get; set; }

    [Column("VL_CONFIS", TypeName = "decimal(18, 4)")]
    public decimal? VlConfis { get; set; }

    [Column("CD_CFO")]
    public int? CdCfo { get; set; }

    [Column("CD_SUFIXO_CFO")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdSufixoCfo { get; set; }

    [Column("CD_EAN")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CdEan { get; set; }

    [Column("CD_CTF")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CdCtf { get; set; }

    [Column("CD_CLF")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CdClf { get; set; }

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;

    [ForeignKey("CdProduto")]
    public virtual Produto? CdProdutoNavigation { get; set; }

    [ForeignKey("CdNotaFiscal, CdSerNotaFiscal")]
    public virtual NotaFiscal NotaFiscal { get; set; } = null!;
}
