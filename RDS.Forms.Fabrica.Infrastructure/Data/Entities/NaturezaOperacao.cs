using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[PrimaryKey("CdCfo", "CdSufixoCfo")]
[Table("NATUREZA_OPERACAO", Schema = "rds")]
public partial class NaturezaOperacao
{
    [Key]
    [Column("CD_CFO")]
    public int CdCfo { get; set; }

    [Key]
    [Column("CD_SUFIXO_CFO")]
    [StringLength(10)]
    [Unicode(false)]
    public string CdSufixoCfo { get; set; } = null!;

    [Column("NO_DESCRICAO_CFOP")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NoDescricaoCfop { get; set; }
}
