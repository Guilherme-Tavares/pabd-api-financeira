using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFinanceiro.Dtos
{
    public class UsuarioDto
    {
        public string Nome { get; set; }

        public string Email { get; set; }
        
        public string Senha { get; set; }
    }
}
