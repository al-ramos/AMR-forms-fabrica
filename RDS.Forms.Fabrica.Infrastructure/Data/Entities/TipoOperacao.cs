using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("TIPO_OPERACAO", Schema = "rds")]
public partial class TipoOperacao
{
    [Key]
    [Column("CD_TIPO_OPERACAO")]
    public int CdTipoOperacao { get; set; }

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("NO_TIPO_OPERACAO")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoTipoOperacao { get; set; }

    [Column("CD_EXPEDICAO_RECEPCAO")]
    [StringLength(1)]
    [Unicode(false)]
    public string? CdExpedicaoRecepcao { get; set; }

    [Column("CD_INTERFACES")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CdInterfaces { get; set; }

    [ForeignKey("CdFilial")]
    [InverseProperty("TipoOperacaos")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;

    [InverseProperty("CdTipoOperacaoNavigation")]
    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();
}
