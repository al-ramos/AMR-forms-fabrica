using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("SMARTCARD", Schema = "rds")]
public partial class Smartcard
{
    [Column("CD_FICHA")]
    public int CdFicha { get; set; }

    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [ForeignKey("CdFicha")]
    public virtual Ficha CdFichaNavigation { get; set; } = null!;

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
