using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceiro.Helpers.Paginated
{
    public class Paginate<T>
    {
        public async static Task<(IQueryable<T>, PaginatedResponse<T>)> Set(IQueryable<T> query, IPaginatedFilter paginate)
        {
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)paginate.Limit);

            var _query = query
                .Skip((paginate.Page - 1) * paginate.Limit)
                .Take(paginate.Limit);

            var response = new PaginatedResponse<T>()
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                Limit = paginate.Limit,
                Page = paginate.Page
            };

            return (_query, response);
        }

        public async static Task<PaginatedResponse<TDto>> Set<TDto>(IQueryable<T> query, IPaginatedFilter paginate, IMapper mapper)
        {
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)paginate.Limit);

            var list = await query
                .Skip((paginate.Page - 1) * paginate.Limit)
                .Take(paginate.Limit)
                .ToListAsync();

            var response = new PaginatedResponse<TDto>()
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                Limit = paginate.Limit,
                Page = paginate.Page,
                //OrderBy = new OrderBy { Column = "descricao", Order = "ASC" },
                Data = mapper.Map<IEnumerable<TDto>>(list)
            };

            return response;
        }
    }
}
