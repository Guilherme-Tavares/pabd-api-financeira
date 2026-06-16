namespace ApiFinanceiro.Helpers.Paginated
{
    public interface IPaginatedFilter
    {
        string? Search { get; set; }

        int Page { get; set; }

        int Limit { get; set; }
    }
}
