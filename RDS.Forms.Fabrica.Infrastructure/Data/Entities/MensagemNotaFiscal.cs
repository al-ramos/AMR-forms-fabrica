using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("MENSAGEM_NOTA_FISCAL", Schema = "rds")]
public partial class MensagemNotaFiscal
{
    [Key]
    [Column("CD_MENSAGEM")]
    public int CdMensagem { get; set; }

    [Column("DS_MENSAGEM")]
    [Unicode(false)]
    public string? DsMensagem { get; set; }
}
