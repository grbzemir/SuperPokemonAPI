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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public OwnerController(IOwnerRepository ownerRepository , IMapper mapper, ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owners);

        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]

        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound(); // 404 

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);

        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            var owner = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
              
            return Ok(owner);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }

            //Aynı Owner var mı yok mu bunu kontrol et
            var owners = _ownerRepository.GetOwners()
                 .Where(c => c.Name.Trim().ToUpper() == ownerCreate.Name.TrimEnd().ToUpper())
                 .FirstOrDefault();

            if (owners != null)

            {
                ModelState.AddModelError("Name", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            //Dto daki dataAnnotations lara bakılır
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Mapleme işlemi yapılıyor 
            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("Name", "Something went wrong while saving the Owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created a Owner");
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
            {
                return BadRequest(ModelState);
            }

            if (ownerId != updatedOwner.Id)
            {
                return BadRequest(ModelState);
            }

            //Owner var mı yok mu kontrollü
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            //DataAnnotations kontrolü
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(updatedOwner);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("Name", "Something went wrong while updating the owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}