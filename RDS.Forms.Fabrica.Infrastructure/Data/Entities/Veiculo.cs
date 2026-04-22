using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("VEICULO", Schema = "rds")]
public partial class Veiculo
{
    [Key]
    [Column("CD_PLACA_VEICULO")]
    [StringLength(10)]
    [Unicode(false)]
    public string CdPlacaVeiculo { get; set; } = null!;

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_UF_VEICULO")]
    [StringLength(2)]
    [Unicode(false)]
    public string? CdUfVeiculo { get; set; }

    [Column("CD_RNTC_VEICULO")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CdRntcVeiculo { get; set; }

    [ForeignKey("CdFilial")]
    [InverseProperty("Veiculos")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;

    [InverseProperty("CdPlacaVeiculoNavigation")]
    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();
}
