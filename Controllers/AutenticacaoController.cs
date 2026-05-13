using ApiFinanceiro.Models;
using ApiFinanceiro.Dtos;
using ApiFinanceiro.DataContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceiro.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IConfiguration _config;

        private readonly AppDbContext _context;

        private readonly PasswordHasher<Usuario> _hasher = new();

        private Usuario _usuario = new Usuario
        {
            Nome = "Guilherme",
            Email = "guilherme@ifro.edu.br",
            Senha = "123456"
        };

        public AutenticacaoController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.Equals(user.Email));

            if (usuario == null)
            {
                return BadRequest("E-mail e/ou senha incorretos");
            }

            var verificacao = _hasher.VerifyHashedPassword(usuario, usuario.Senha, user.Password);

            if (verificacao == PasswordVerificationResult.Failed)
            {
                return BadRequest("Senha incorreta");
            }

            var token = GerarJwtToken(usuario);

            return Ok(new { token });

            //if (!user.Email.Equals(_usuario.Email) || !user.Password.Equals(_usuario.Senha))
            //{
            //    return BadRequest("E-mail e/ou senha incorretos");
            //}

            //var token = GerarJwtToken(_usuario);

            //return Ok(new { token });
        }

        private string GerarJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim("Nome", usuario.Nome),
                new Claim("Email", usuario.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
