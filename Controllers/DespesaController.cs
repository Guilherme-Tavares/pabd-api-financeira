using ApiFinanceiro.Models;
using ApiFinanceiro.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiFinanceiro.DataContexts;
using Microsoft.EntityFrameworkCore;
using ApiFinanceiro.Services;
using ApiFinanceiro.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace ApiFinanceiro.Controllers
{
    [Route("/despesas")]

    // Annotation
    [ApiController]
    [Authorize]
    public class DespesaController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly DespesaService _service;

        public DespesaController(DespesaService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet()]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var despesas = await _service.FindAll();

                return Ok(despesas);

            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(int id)
        {
            try
            {
                var despesa = await _service.FindById(id);
                return Ok(despesa);

            }

            // Exceção deve ser retornada no controller, e não no service
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] DespesaDto novaDespesa)
        {
            try
            {
                var despesa = await _service.Create(novaDespesa);

                return Created("", despesa);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DespesaUpdateDto despesaDto)
        {
            try
            {
                var despesa = await _service.Update(id, despesaDto);

                return Ok(despesa);

            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost("{id}/tags")]
        public async Task<IActionResult> AddTags(int id, [FromBody] DespesaTagsDto tag)
        {
            try
            {
                var despesa = await _service.AddTags(id, tag);
                return Ok();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);

                return NoContent();

            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        //public static List<Despesa> listaDespesas = new()
        //{
        //    new Despesa {
        //        Descricao = "Internet",
        //        Valor = 150,
        //        Categoria = "Moradia",
        //        DataVencimento = new DateOnly(2026, 03, 15),
        //        Situacao = "pago"
        //    },
        //    new Despesa {
        //        Descricao = "Internet",
        //        Valor = 125,
        //        Categoria = "Moradia",
        //        DataVencimento = new DateOnly(2026, 03, 16),
        //        Situacao = "pendente"
        //     },
        //    new Despesa {
        //        Descricao = "Internet",
        //        Valor = 200,
        //        Categoria = "Moradia",
        //        DataVencimento = new DateOnly(2026, 03, 17),
        //        Situacao = "pendente"
        //     }
        // };

        // Annotation
        //        [HttpGet()]
        //        public ActionResult FindAll()
        //        {
        //            return Ok(listaDespesas);
        //        }

        //        // Annotation
        //        [HttpPost()]
        //        public ActionResult Create([FromBody] DespesaDto novaDespesa)
        //        {
        //            var despesa = new Despesa
        //            {
        //                Descricao = novaDespesa.Descricao,
        //                Valor = novaDespesa.Valor,
        //                Categoria = novaDespesa.Categoria,
        //                DataVencimento = novaDespesa.DataVencimento,
        //                Situacao = "aberto"
        //            };

        //            listaDespesas.Add(despesa);

        //            return Created("", novaDespesa);
        //        }

        //        [HttpGet("{id}")]
        //        public ActionResult FindById(Guid id)
        //        {
        //            // Lambda expression (com arrow function)
        //            // var despesa = listaDespesas.Find(d => d.Id == id);
        //            var despesa = listaDespesas.FirstOrDefault(d => d.Id == id);

        //            if (despesa is null)
        //                return NotFound(new { mensagem = $"Despesa #{id} não encontrada" });

        //            return Ok(despesa);
        //        }

        //        [HttpPut("{id}")]
        //        public ActionResult Update(Guid id, [FromBody] DespesaUpdateDto despesaDto)
        //        {
        //            var despesa = listaDespesas.FirstOrDefault(d => d.Id == id);

        //            if (despesa is null)
        //                return NotFound(new { mensagem = $"Despesa #{id} não encontrada" });

        //            despesa.Descricao = despesaDto.Descricao;
        //            despesa.Valor = despesaDto.Valor;
        //            despesa.DataVencimento = despesaDto.DataVencimento;
        //            despesa.Categoria = despesaDto.Categoria;
        //            despesa.Situacao = despesaDto.Situacao;
        //            despesa.DataPagamento = despesaDto.DataPagamento;

        //            return Ok(despesa);
        //        }

        //        // Pagamento
        //        // [HttpPut("{id}")]
        //        // public ActionResult Pay(Guid id, [FromBody])

        //        [HttpDelete("{id}")]
        //        public ActionResult Remove(Guid id)
        //        {
        //            var despesa = listaDespesas.FirstOrDefault(d => d.Id == id);

        //            if (despesa is null)
        //                return NotFound(new { mensagem = $"Despesa #{id} não encontrada" });

        //            listaDespesas.Remove(despesa);
        //            return NoContent();
        //        }
    }
}

