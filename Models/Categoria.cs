using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiFinanceiro.Models
{
    [Table("categorias"), PrimaryKey(nameof(Id))]
    public class Categoria
    {
        public int Id { get; set; }

        public required string Descricao { get; set; }

        public ICollection<Despesa>? Despesas { get; set; }
    }
}
