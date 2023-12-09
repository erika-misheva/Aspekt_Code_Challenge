using AutoMapper;
using ContactApp.Domain.DTOs.Shared;
using ContactApp.Domain.Models;
using ContactApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly IMapper _mapper;

        public CountriesController(IRepository<Country> countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<List<EntityDto>>> GetCountries()
        {
            var countries = await _countryRepository.GetAllAsync();
            return _mapper.Map<List<EntityDto>>(countries);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<EntityDto>> GetCountry(int id)
        {
            var country = await _countryRepository.GetByIdAsync(id);

            if (country is null)
            {
                return NotFound();
            }

            return _mapper.Map<EntityDto>(country);
        }

        [HttpPost]
        public async Task<ActionResult> Create(UpdateEntityDto countryDto)
        {
            Country newCountry = _mapper.Map<Country>(countryDto);

            await _countryRepository.AddAsync(newCountry);
            await _countryRepository.SavedAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateEntityDto updateCountry)
        {

            if (! await _countryRepository.EntityExistsAsync(id))
            {
                return NotFound();
            }
            Country country = await _countryRepository.GetByIdAsync(id);
            country.Name = updateCountry.Name;

            await _countryRepository.GetAllAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _countryRepository.EntityExistsAsync(id))
            {
                return NotFound();
            }

           await _countryRepository.DeleteAsync(id);
           await _countryRepository.SavedAsync();

            return Ok();
        }
    }
}
