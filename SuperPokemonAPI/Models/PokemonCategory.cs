namespace SuperPokemonAPI.Models
{
    public class PokemonCategory
    {
        public int PokemonId { get; set; }
        public int CategoryId { get; set; }
        public Pokemon Pokemon { get; set; } // 1-1 relationship with Pokemon
    }
}
