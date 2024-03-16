using AutoMapper;
using PeopleInc.Data.Dtos.Pessoa;
using PeopleInc.Models;

namespace PeopleInc.Profiles
{
    public class PessoaProfile : Profile
    {
        public PessoaProfile()
        {
            CreateMap<CadastrarPessoaDto, Pessoa>();
            CreateMap<AlterarPessoaDto, Pessoa>().ReverseMap();
            CreateMap<Pessoa, ConsultarPessoaDto>();
        }
    }
}
