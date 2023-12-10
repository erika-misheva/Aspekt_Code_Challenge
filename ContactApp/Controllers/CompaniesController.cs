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
    public async Task<ActionResult<List<EntityDto>>> GetCompanies()
    {
        IEnumerable<Company> companies = await _companyRepository.GetAllAsync();
        return _mapper.Map<List<EntityDto>>(companies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EntityDto>> GetCompany(int id)
    { 
        if (! await _companyRepository.EntityExistsAsync(id))
        {
            return NotFound();
        }

        var company = await _companyRepository.GetByIdAsync(id);

        return _mapper.Map<EntityDto>(company);
    }

    [HttpPost]
    public async Task<ActionResult> Create(UpdateEntityDto companyDto)
    {
        Company newCompany = _mapper.Map<Company>(companyDto);

        await _companyRepository.AddAsync(newCompany);
        await _companyRepository.SavedAsync();

        return StatusCode(StatusCodes.Status201Created, ResponseDetail.Created(newCompany.Id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateEntityDto updateCompany)
    {
        if (!await _companyRepository.EntityExistsAsync(id))
        {
            return NotFound();
        }

        Company company = (await _companyRepository.GetByIdAsync(id))!;
        company.Name = updateCompany.Name;

        _companyRepository.Update(company);
        await _companyRepository.SavedAsync();

        var updatedCompany = _mapper.Map<EntityDto>(company);

        return Ok(updatedCompany);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _companyRepository.EntityExistsAsync(id))
        {
            return NotFound();
        }

        await _companyRepository.DeleteAsync(id);
        await _companyRepository.SavedAsync();

        return Ok();
    }
}
