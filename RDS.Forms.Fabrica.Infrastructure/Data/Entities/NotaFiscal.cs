using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[PrimaryKey("CdNotaFiscal", "CdSerNotaFiscal")]
[Table("NOTA_FISCAL", Schema = "rds")]
[Index("DtEmissaoNf", Name = "IX_NF_DT_EMISSAO")]
[Index("CdFicha", Name = "IX_NF_FICHA")]
[Index("CdFilial", Name = "IX_NF_FILIAL")]
public partial class NotaFiscal
{
    [Key]
    [Column("CD_NOTA_FISCAL")]
    public int CdNotaFiscal { get; set; }

    [Key]
    [Column("CD_SER_NOTA_FISCAL")]
    [StringLength(10)]
    [Unicode(false)]
    public string CdSerNotaFiscal { get; set; } = null!;

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_FICHA")]
    public int? CdFicha { get; set; }

    [Column("CD_BUSINESS_UNIT")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CdBusinessUnit { get; set; }

    [Column("NO_FILIAL")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoFilial { get; set; }

    [Column("NO_CLIENTE")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NoCliente { get; set; }

    [Column("IC_IMPRESSOES")]
    public int? IcImpressoes { get; set; }

    [Column("IC_CANCELADO")]
    public int? IcCancelado { get; set; }

    [Column("CD_CHAVE_NFE")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CdChaveNfe { get; set; }

    [Column("CD_PROTOCOLO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CdProtocolo { get; set; }

    [Column("CD_AMBIENTE")]
    [StringLength(5)]
    [Unicode(false)]
    public string? CdAmbiente { get; set; }

    [Column("CD_MODELO_NF")]
    [StringLength(10)]
    [Unicode(false)]
    public string? CdModeloNf { get; set; }

    [Column("CD_DANFE")]
    [StringLength(200)]
    [Unicode(false)]
    public string? CdDanfe { get; set; }

    [Column("DT_EMISSAO_NF")]
    public DateOnly? DtEmissaoNf { get; set; }

    [Column("CD_CNPJ_CLIENTE")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CdCnpjCliente { get; set; }

    [Column("CD_UF2_FILIAL")]
    [StringLength(2)]
    [Unicode(false)]
    public string? CdUf2Filial { get; set; }

    [Column("CAN_INU_XJUST")]
    [StringLength(500)]
    [Unicode(false)]
    public string? CanInuXjust { get; set; }

    [Column("INU_ANO")]
    [StringLength(4)]
    [Unicode(false)]
    public string? InuAno { get; set; }

    [Column("VL_TRANSMISSAO", TypeName = "decimal(18, 4)")]
    public decimal? VlTransmissao { get; set; }

    [Column("CD_TIPO_IMPRESSAO_NF")]
    public int? CdTipoImpressaoNf { get; set; }

    [ForeignKey("CdBusinessUnit")]
    [InverseProperty("NotaFiscals")]
    public virtual BusinessUnit? CdBusinessUnitNavigation { get; set; }

    [ForeignKey("CdFicha")]
    [InverseProperty("NotaFiscals")]
    public virtual Ficha? CdFichaNavigation { get; set; }

    [ForeignKey("CdFilial")]
    [InverseProperty("NotaFiscals")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
