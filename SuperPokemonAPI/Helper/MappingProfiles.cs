using AutoMapper;
using SuperPokemonAPI.Dtos;
using SuperPokemonAPI.Models;

namespace SuperPokemonAPI.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Country, CountryDto>();
           
        }
    }
}
