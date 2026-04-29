using ApiFinanceiro.Dtos;
using ApiFinanceiro.Dtos.Responses;
using ApiFinanceiro.Models;
using AutoMapper;

namespace ApiFinanceiro.Profiles
{
    public class DespesaProfile : Profile
    {
        public DespesaProfile()
        {
            CreateMap<DespesaDto, Despesa>()
                .ForMember(
                dest => dest.Situacao,
                opt => opt.MapFrom(src => "pendente")
                );
            CreateMap<DespesaUpdateDto, Despesa>()
                .ForMember(
                    dest => dest.DataPagamento,
                    opt => opt.MapFrom(
                        src => src.DataPagamento.ToDateTime(TimeOnly.FromDateTime(DateTime.Now))
                    )
                );

            CreateMap<Categoria, CategoriaResponseDto>();
            CreateMap<Despesa, DespesaResponseDto>();
            CreateMap<Tag, TagResponseDto>();
        }
    }
}
