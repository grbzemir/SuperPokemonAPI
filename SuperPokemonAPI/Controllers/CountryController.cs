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
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository , IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countries);

        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]

        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound(); // 404 

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);

        }

        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry ([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
            {
                return BadRequest(ModelState);
            }

            //Aynı Kategori var mı yok mu bunu kontrol et
            var country = _countryRepository.GetCountries()
                 .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                 .FirstOrDefault();

            if (country != null)

            {
                ModelState.AddModelError("Name", "Country already exists");
                return StatusCode(422, ModelState);
            }

            //Dto daki dataAnnotations lara bakılır
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Mapleme işlemi yapılıyor 
            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("Name", "Something went wrong while saving the Country");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created a Country");
        }
    }
}
