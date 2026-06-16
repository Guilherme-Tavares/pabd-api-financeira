using ApiFinanceiro.Helpers.Paginated;

namespace ApiFinanceiro.Controllers.Filters
{
    public class DespesaFilter : PaginatedFilter
    {
        public string? Situacao { get; set; } = null;
    }
}
