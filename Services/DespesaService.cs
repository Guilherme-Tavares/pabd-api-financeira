using ApiFinanceiro.DataContexts;
using ApiFinanceiro.Dtos;
using ApiFinanceiro.Dtos.Responses;
using ApiFinanceiro.Exceptions;
using ApiFinanceiro.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiFinanceiro.Services
{
    public class DespesaService
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public DespesaService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<DespesaResponseDto>> FindAll()
        {
            try
            {
                /**
                 * SQL: Seleciona todas as colunas
                */
                var list = await _context.Despesas
                    .Include(d => d.Categoria)
                    .Include(d => d.Tags)
                    .ToListAsync();

                /**
                 * Mapper: filtra a lista de despesas e retorna somente
                 * os atributos que estão em DespesaResponseDto
                */
                return _mapper.Map<ICollection<DespesaResponseDto>>(list);

                /**
                 * Melhor uso, na SQL faz o SELECT somente das colunas adicionadas
                 * no DespesaResponseDto
                */
                //return await _context.Despesas
                //       .Include(d => d.Categoria)
                //       .ProjectTo<DespesaResponseDto>(_mapper.ConfigurationProvider)
                //       .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Despesa> AddTags(int id, DespesaTagsDto tag)
        {
            try
            {
                var despesa = await FindById(id);

                var tags = await _context.Tags.Where(x => tag.Ids.Contains(x.Id)).ToListAsync();

                if (tags.Count == 0)
                {
                    throw new ErrorServiceException("Tags não econtradas",
                        c => c.NotFound(new { message = $"Tags não encontradas" }));
                }

                foreach(Tag _tag in tags)
                {
                    if (despesa.Tags.Any(t => t.Id != _tag.Id))
                    {
                        despesa.Tags.Add(_tag);
                    }    
                }
                await _context.SaveChangesAsync();

                return despesa;       
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<Despesa> FindById(int id)
        {
            var despesa = await _context.Despesas.Include(d => d.Tags).FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                if (despesa is null)
                {
                    throw new ErrorServiceException($"Despesa #{id} não encontrada",
                        c => c.NotFound(new { message = $"Despesa #{id} não encontrada" }));
                }

                return despesa;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Despesa> Create(DespesaDto data)
        {
            try
            {
                var categoriaExiste = await _context.Categorias.AnyAsync(x => x.Id == data.CategoriaId);

                if (!categoriaExiste)
                {
                    throw new ErrorServiceException("$Categoria não encontrada",
                        c => c.NotFound(new { message = $"Categoria #{data.CategoriaId} não encontrada" }));
                }

                var despesa = _mapper.Map<Despesa>(data);

                await _context.Despesas.AddAsync(despesa);
                await _context.SaveChangesAsync();

                return despesa;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task<Despesa> Update(int id, DespesaUpdateDto data)
        {
            try
            {
                var despesa = await FindById(id);

                var categoriaExiste = await _context.Categorias.AnyAsync(x => x.Id == data.CategoriaId);

                if (!categoriaExiste)
                {
                    throw new ErrorServiceException("$Categoria não encontrada",
                        c => c.NotFound(new { message = $"Categoria #{data.CategoriaId} não encontrada" }));
                }

                var dataVencimento = new DateTime(despesa.DataVencimento.Year, despesa.DataVencimento.Month, despesa.DataVencimento.Day);
                var dataPagamento = new DateTime(data.DataPagamento.Year, data.DataPagamento.Month, data.DataPagamento.Day);


                // TODO: adicionar data de emissão
                //if (dataPagamento < dataVencimento)
                //{
                //    throw new ErrorServiceException("Somente é possível realizar o pagamento no dia do vencimento",
                //        c => c.Conflict(new { message = "Somente é possível realizar o pagamento no dia do vencimento" }));
                //}

                _mapper.Map(data, despesa);

                _context.Despesas.Update(despesa);
                await _context.SaveChangesAsync();

                return despesa;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Remove(int id)
        {
            try
            {
                var despesa = await FindById(id);

                _context.Despesas.Remove(despesa);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Despesa> removeTags(int id, DespesaTagsDto tag)
        {
            try
            {
                var despesa = await FindById(id);

                var tags = await _context.Tags.Where(x => tag.Ids.Contains(x.Id)).ToListAsync();

                if (tags.Count == 0)
                {
                    throw new ErrorServiceException("Tags não econtradas",
                        c => c.NotFound(new { message = $"Tags não encontradas" }));
                }

                foreach (Tag _tag in tags)
                {
                    if (despesa.Tags.Any(t => t.Id != _tag.Id))
                    {
                        despesa.Tags.Remove(_tag);
                    }
                }
                await _context.SaveChangesAsync();

                return despesa;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
