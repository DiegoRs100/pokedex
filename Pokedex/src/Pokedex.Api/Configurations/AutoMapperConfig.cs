using AutoMapper;
using Pokedex.Api.Models;
using Pokedex.Business.Entities;

namespace Pokedex.Api.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<PokemonModel, Pokemon>().ReverseMap();
            CreateMap<CategoryModel, Category>().ReverseMap();
        }
    }
}