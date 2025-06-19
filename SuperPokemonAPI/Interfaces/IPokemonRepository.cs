using SuperPokemonAPI.Models;

namespace SuperPokemonAPI.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        decimal GetPokemonRating(int pokeId);
        bool PokemonExists(int pokeId);
        bool CreatePokemon(int OwnerId , int categoryId , Pokemon pokemon);
        bool Save();


    }
}
