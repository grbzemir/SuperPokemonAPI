namespace SuperPokemonAPI.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gym { get; set; } 
        public Country Country { get; set; } //1-1
        public ICollection<PokemonOwner> PokemonOwners { get; set; } //1-n
    }
}
