using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers;

using Domain.DTOs.Shared;
using Domain.Models;
using Data.Interfaces;
using Domain.Common;

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

    public async Task<ActionResult<List<EntityDto>>> GetCountriesAsync()
    {
        var countries = await _countryRepository.GetAllAsync();
        return _mapper.Map<List<EntityDto>>(countries);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<EntityDto>> GetCountryAsync(int id)
    {
        var country = await _countryRepository.GetByIdAsync(id);

        if (country is null)
        {
            return NotFound(ResponseDetail.NotFound(entity: nameof(Country), id));
        }

        return _mapper.Map<EntityDto>(country);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(UpdateEntityDto createCountry)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Country newCountry = _mapper.Map<Country>(createCountry);

        await _countryRepository.AddAsync(newCountry);
        try
        {
            await _countryRepository.SavedAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        return StatusCode(StatusCodes.Status201Created, ResponseDetail.Created(newCountry.Id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, UpdateEntityDto updateCountry)
    {
        if (! await _countryRepository.EntityExistsAsync(id))
        {
            return NotFound(ResponseDetail.NotFound(entity: nameof(Country), id));
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Country country = await _countryRepository.GetByIdAsync(id);
        country.Name = updateCountry.Name;

        _countryRepository.Update(country);
        try
        {
            await _countryRepository.SavedAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        var updatedCountry = _mapper.Map<EntityDto>(country);

        return Ok(updatedCountry);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        if (!await _countryRepository.EntityExistsAsync(id))
        {
            return NotFound(ResponseDetail.NotFound(entity: nameof(Country), id));
        }

       await _countryRepository.DeleteAsync(id);
        try
        {
            await _countryRepository.SavedAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return NoContent();
    }
}
