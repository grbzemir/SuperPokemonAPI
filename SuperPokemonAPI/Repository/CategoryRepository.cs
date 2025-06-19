using SuperPokemonAPI.Data;
using SuperPokemonAPI.Interfaces;
using SuperPokemonAPI.Models;

namespace SuperPokemonAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
        {
            return _context.PokemonCategories
                .Where(e => e.Category.Id == categoryId)
                .Select(c => c.Pokemon)
                .ToList();
        }
    }
}
