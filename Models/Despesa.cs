using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFinanceiro.Models
{
    [Table("despesas"), PrimaryKey(nameof(Id))]
    public class Despesa
    {
        public int Id { get; set; }

        public required string Descricao { get; set; }

        public required decimal Valor { get; set; }

        public required DateOnly DataVencimento { get; set; }

        public required string Situacao { get; set; }

        public DateTime? DataPagamento { get; set; }

        public int? CategoriaId { get; set; }

        public virtual Categoria? Categoria { get; set; }

        public ICollection<Tag>? Tags { get; set; } = new List<Tag>();
    }
}
