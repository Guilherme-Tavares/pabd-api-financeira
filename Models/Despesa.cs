using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFinanceiro.Models
{
    [Table("despesas"), PrimaryKey(nameof(Id))]
    public class Despesa
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("descricao")]
        public required string Descricao { get; set; }
        [Column("valor")]
        public required decimal Valor { get; set; }
        [Column("data_vencimento")]
        public required DateOnly DataVencimento { get; set; }
        [Column("situacao")]
        public required string Situacao { get; set; }
        [Column("data_pagamento")]
        public DateTime? DataPagamento { get; set; }

        [Column("categoria_id")]
        public int? CategoriaId { get; set; }

        public virtual Categoria? Categoria { get; set; }

        public ICollection<Tag>? Tags { get; set; } = new List<Tag>();
    }
}
