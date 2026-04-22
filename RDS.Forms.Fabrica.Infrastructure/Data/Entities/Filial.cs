using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RDS.Forms.Fabrica.Infrastructure.Data.Entities;

[Table("FILIAL", Schema = "rds")]
public partial class Filial
{
    [Key]
    [Column("CD_FILIAL")]
    public int CdFilial { get; set; }

    [Column("NO_FILIAL")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoFilial { get; set; }

    [Column("CD_BU_DEPOSITO")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CdBuDeposito { get; set; }

    [Column("CD_TIPO_IMPRESSAO_NF")]
    public int? CdTipoImpressaoNf { get; set; }

    [InverseProperty("CdFilialNavigation")]
    public virtual ICollection<ControleSerieNf> ControleSerieNfs { get; set; } = new List<ControleSerieNf>();

    [InverseProperty("CdFilialNavigation")]
    public virtual ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();

    [InverseProperty("CdFilialNavigation")]
    public virtual ICollection<Estacao> Estacaos { get; set; } = new List<Estacao>();

    [InverseProperty("CdFilialNavigation")]
    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();

    [InverseProperty("CdFilialNavigation")]
    public virtual ICollection<NotaFiscal> NotaFiscals { get; set; } = new List<NotaFiscal>();

    [InverseProperty("CdFilialNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [InverseProperty("CdFilialNavigation")]
    public virtual ICollection<TipoOperacao> TipoOperacaos { get; set; } = new List<TipoOperacao>();

    [InverseProperty("CdFilialNavigation")]
    public virtual ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
}
