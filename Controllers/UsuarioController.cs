using ApiFinanceiro.DataContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiFinanceiro.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ApiFinanceiro.Models;

namespace ApiFinanceiro.Controllers
{
    [Route("/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public UsuarioController(
            AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] UsuarioDto dto)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(dto);

                usuario.Senha = _passwordHasher.HashPassword(usuario, dto.Senha);

                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
