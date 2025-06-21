using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperPokemonAPI.Dtos;
using SuperPokemonAPI.Interfaces;
using SuperPokemonAPI.Models;
using SuperPokemonAPI.Repository;

namespace SuperPokemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {

            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]

        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound(); // 404 

            var categories = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categories);

        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonsByCategoryId(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pokemons.Count == 0)
            {
                return NotFound();
            }

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
            {
                return BadRequest(ModelState);
            }

            //Aynı Kategori var mı yok mu bunu kontrol et
            var category = _categoryRepository.GetCategories()
                 .Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
                 .FirstOrDefault();

            if (category != null)

            {
                ModelState.AddModelError("Name", "Category already exists");
                return StatusCode(422, ModelState);
            }
           
            //Dto daki dataAnnotations lara bakılır
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            //Mapleme işlemi yapılıyor 
            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("Name", "Something went wrong while saving the category");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created a category");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
        {
            if(updatedCategory == null)
            {
                return BadRequest(ModelState);
            }

            if(categoryId != updatedCategory.Id)
            {
                return BadRequest(ModelState);
            }

            //Kategori var mı yok mu kontrollü
            if(!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            //DataAnnotations kontrolü
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("Name", "Something went wrong while updating the category");
                return StatusCode(500, ModelState);
            }

            return NoContent(); 

        }
    }
 }