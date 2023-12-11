using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers;

using Domain.DTOs.Shared;
using Domain.Models;
using Domain.Common;
using Data.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IRepository<Company> _companyRepository;
    private readonly IMapper _mapper;

    public CompaniesController(IRepository<Company> companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<EntityDto>>> GetCompaniesAsync()
    {
        IEnumerable<Company> companies = await _companyRepository.GetAllAsync();
        return _mapper.Map<List<EntityDto>>(companies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EntityDto>> GetCompanyAsync(int id)
    { 
        if (! await _companyRepository.EntityExistsAsync(id))
        {
            return NotFound(ResponseDetail.NotFound(entity: nameof(Company), id));
        }

        var company = await _companyRepository.GetByIdAsync(id);

        return _mapper.Map<EntityDto>(company);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(UpdateEntityDto createCompany)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Company newCompany = _mapper.Map<Company>(createCompany);

        await _companyRepository.AddAsync(newCompany);
        try
        {
            await _companyRepository.SavedAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return StatusCode(StatusCodes.Status201Created, ResponseDetail.Created(newCompany.Id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, UpdateEntityDto updateCompany)
    {
        if (!await _companyRepository.EntityExistsAsync(id))
        {
            return NotFound(ResponseDetail.NotFound(entity: nameof(Company), id));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Company company = (await _companyRepository.GetByIdAsync(id))!;
        company.Name = updateCompany.Name;

        _companyRepository.Update(company);
        try
        {
            await _companyRepository.SavedAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }


        var updatedCompany = _mapper.Map<EntityDto>(company);

        return Ok(updatedCompany);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        if (!await _companyRepository.EntityExistsAsync(id))
        {
            return NotFound(ResponseDetail.NotFound(entity: nameof(Company), id));
        }

        await _companyRepository.DeleteAsync(id);
        try
        {
            await _companyRepository.SavedAsync();
        }
        catch (Exception ex)
        { 
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }


        return NoContent();
    }
}
