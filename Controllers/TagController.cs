using ApiFinanceiro.DataContexts;
using ApiFinanceiro.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceiro.Controllers
{
    [Route("tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var tags = await _context.Tags.ToListAsync();

                return Ok(tags);

            }
            catch (Exception ex)
            {
                return Problem($"Ocorreram erros ao processar a solicitação. {ex.GetHashCode}");
            }
        }
    }
}
