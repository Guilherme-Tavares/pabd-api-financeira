namespace ApiFinanceiro.Helpers.Paginated
{
    public class PaginatedResponse<T>
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public OrderBy? OrderBy { get; set; }

        public IEnumerable<T> Data { get; set; } = new List<T>();
    }

    public class OrderBy
    {
        public string Column { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
    }
}
