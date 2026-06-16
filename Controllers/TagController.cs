using ApiFinanceiro.DataContexts;
using ApiFinanceiro.Dtos;
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
        private readonly DespesaService _service;

        public TagController(AppDbContext context, DespesaService service)
        {
            _context = context;
            _service = service;
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

        [HttpDelete]
        public async Task<IActionResult> Remove(int id, DespesaTagsDto tag)
        {
            try
            {
                await _service.RemoveTags(id, tag);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem($"Ocorreram erros ao processar a solicitação. {ex.GetHashCode}");
            }
        }
    }
}
