namespace ApiFinanceiro.Dtos.Responses
{
    public class DespesaResponseDto
    {
        public int Id { get; set; }

        public required string Descricao { get; set; }

        public required decimal Valor { get; set; }

        //public required DateOnly DataVencimento { get; set; }

        public required string Situacao { get; set; }

        //public DateTime? DataPagamento { get; set; }

        public CategoriaResponseDto? Categoria { get; set; }
        public List<TagResponseDto> Tags { get; set; }
    }
}
