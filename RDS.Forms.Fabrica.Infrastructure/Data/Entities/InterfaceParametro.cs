using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Keyless]
[Table("INTERFACE_PARAMETRO", Schema = "rds")]
public partial class InterfaceParametro
{
    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("DS_SENHANEOGRID")]
    [StringLength(25)]
    [Unicode(false)]
    public string? DsSenhaneogrid { get; set; }

    [Column("EXTENSAO_IMPRESSORA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ExtensaoImpressora { get; set; }

    [Column("EXTENSAO_IMPRESSORA_BANDEJA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ExtensaoImpressoraBandeja { get; set; }

    [Column("IDXDOCPRINT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Idxdocprint { get; set; }

    [ForeignKey("CdFilial")]
    public virtual Filial CdFilialNavigation { get; set; } = null!;
}
