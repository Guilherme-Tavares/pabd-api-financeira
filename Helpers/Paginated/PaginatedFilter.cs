using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiFinanceiro.Helpers.Paginated
{
    public class PaginatedFilter : IPaginatedFilter
    {
        public string? Search { get; set; } = null;

        [DefaultValue(1)]
        public int Page { get; set; } = 1;

        [DefaultValue(4)]
        [Range(1, 10)]
        public int Limit { get; set; } = 4;
    }
}
