using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFinanceiro.Models
{
    [Table("tags"), PrimaryKey(nameof(Id))]
    public class Tag
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public ICollection<Despesa>? Despesas { get; set; }
    }
}
