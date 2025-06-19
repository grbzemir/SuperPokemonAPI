using SuperPokemonAPI.Models;

namespace SuperPokemonAPI.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();


    }
}
