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

        public ActionResult<List<EntityDto>> GetCompanies()
        {
            var companies = _companyRepository.GetAll();
            return _mapper.Map<List<EntityDto>>(companies);
        }

        [HttpGet("{id}")]

        public ActionResult<EntityDto> GetCompany(int id)
        {
            var company = _companyRepository.GetById(id);

            if (company is null)
            {
                return NotFound();
            }

            return _mapper.Map<EntityDto>(company);
        }

        [HttpPost]
        public ActionResult Create(UpdateEntityDto companyDto)
        {
            Company newCompany = _mapper.Map<Company>(companyDto);

            _companyRepository.Add(newCompany);
            _companyRepository.Saved();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, UpdateEntityDto updateCompany)
        {

            if (!_companyRepository.EntityExists(id))
            {
                return NotFound();
            }
            Company company = _companyRepository.GetById(id);
            company.Name = updateCompany.Name;

            _companyRepository.Saved();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!_companyRepository.EntityExists(id))
            {
                return NotFound();
            }

            _companyRepository.Delete(id);
            _companyRepository.Saved();

            return Ok();
        }
    }
}
