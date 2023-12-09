using AutoMapper;
using ContactApp.Domain.DTOs.Shared;
using ContactApp.Domain.Models;
using ContactApp.Interfaces;
using ContactApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
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
            var companies = await _companyRepository.GetAllAsync();
            return _mapper.Map<List<EntityDto>>(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EntityDto>> GetCompany(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company is null)
            {
                return NotFound();
            }

            return _mapper.Map<EntityDto>(company);
        }

        [HttpPost]
        public async Task<ActionResult> Create(UpdateEntityDto companyDto)
        {
            Company newCompany = _mapper.Map<Company>(companyDto);

            await _companyRepository.AddAsync(newCompany);
            await _companyRepository.SavedAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateEntityDto updateCompany)
        {
            if (!await _companyRepository.EntityExistsAsync(id))
            {
                return NotFound();
            }

            Company company = await _companyRepository.GetByIdAsync(id);
            company.Name = updateCompany.Name;

            _companyRepository.UpdateAsync(company);
            await _companyRepository.SavedAsync();

            return Ok();
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
}
