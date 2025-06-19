namespace SuperPokemonAPI.Models
{
    public class PokemonOwner
    {
        public int PokemonId { get; set; }
        public int OwnerId { get; set; }
        public Pokemon Pokemon { get; set; } // 1-1 relationship with Pokemon
        public Owner Owner { get; set; } // 1-1 relationship with Owner
    }
}
