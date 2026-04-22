using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("MODALIDADE_FRETE", Schema = "rds")]
public partial class ModalidadeFrete
{
    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("SIGLA")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Sigla { get; set; }

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
