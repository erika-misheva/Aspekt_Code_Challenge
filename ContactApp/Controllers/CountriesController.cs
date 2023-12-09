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

        public ActionResult<List<EntityDto>> GetCountries()
        {
            var countries = _countryRepository.GetAll();
            return _mapper.Map<List<EntityDto>>(countries);
        }

        [HttpGet("{id}")]

        public ActionResult<EntityDto> GetCountry(int id)
        {
            var country = _countryRepository.GetById(id);

            if (country is null)
            {
                return NotFound();
            }

            return _mapper.Map<EntityDto>(country);
        }

        [HttpPost]
        public ActionResult Create(UpdateEntityDto countryDto)
        {
            Country newCountry = _mapper.Map<Country>(countryDto);

            _countryRepository.Add(newCountry);
            _countryRepository.Saved();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, UpdateEntityDto updateCountry)
        {

            if (!_countryRepository.EntityExists(id))
            {
                return NotFound();
            }
            Country country = _countryRepository.GetById(id);
            country.Name = updateCountry.Name;

            _countryRepository.Saved();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!_countryRepository.EntityExists(id))
            {
                return NotFound();
            }

            _countryRepository.Delete(id);
            _countryRepository.Saved();

            return Ok();
        }
    }
}
