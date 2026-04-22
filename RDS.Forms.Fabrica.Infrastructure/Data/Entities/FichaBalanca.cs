using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("FICHA_BALANCA", Schema = "rds")]
public partial class FichaBalanca
{
    [Column("CD_FICHA")]
    public int CdFicha { get; set; }

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_PESAGEM")]
    public int? CdPesagem { get; set; }

    [Column("IC_ORIGEM_DESTINO")]
    [StringLength(1)]
    [Unicode(false)]
    public string? IcOrigemDestino { get; set; }

    [Column("VL_PESO_1_PESAGEM", TypeName = "decimal(18, 4)")]
    public decimal? VlPeso1Pesagem { get; set; }

    [Column("VL_PESO_2_PESAGEM", TypeName = "decimal(18, 4)")]
    public decimal? VlPeso2Pesagem { get; set; }

    [ForeignKey("CdFicha")]
    public virtual Ficha CdFichaNavigation { get; set; } = null!;

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
