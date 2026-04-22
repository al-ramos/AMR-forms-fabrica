using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("FILIAL_BUSINESS_UNIT", Schema = "rds")]
public partial class FilialBusinessUnit
{
    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("CD_BUSINESS_UNIT")]
    [StringLength(20)]
    [Unicode(false)]
    public string CdBusinessUnit { get; set; } = null!;

    [ForeignKey("CdBusinessUnit")]
    public virtual BusinessUnit CdBusinessUnitNavigation { get; set; } = null!;

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
