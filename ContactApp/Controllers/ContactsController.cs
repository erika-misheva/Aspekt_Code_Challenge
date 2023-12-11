using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers;

using Domain.Common;
using Data.Interfaces;
using Domain.DTOs.Contact;
using Domain.DTOs.Shared;
using Domain.Models;



[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Country> _countryRepository;
    private readonly IRepository<Company> _companyRepository;

    public ContactsController(
        IContactRepository contactRepository,
        IMapper mapper,
        IRepository<Country> countryRepository,
        IRepository<Company> companyRepository)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
        _countryRepository = countryRepository;
        _companyRepository = companyRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<EntityDto>>> GetContactsAsync()
    {
        var contacts = await _contactRepository.GetAllAsync();
        return _mapper.Map<List<EntityDto>>(contacts);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<EntityDto>> GetContactAsync(int id)
    {
        var contact = await _contactRepository.GetByIdAsync(id);

        if (contact is null)
        {
            return NotFound(ResponseDetail.NotFound(entity: nameof(Contact), id));
        }

        return _mapper.Map<EntityDto>(contact);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateContactDto createContact)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Contact newContact = _mapper.Map<Contact>(createContact);
        bool companyExists = await _countryRepository.EntityExistsAsync(newContact.CountryId);
        bool countryExists = await _companyRepository.EntityExistsAsync(newContact.CompanyId);

        if (!companyExists || !countryExists)
        {
            object detailsObject = ResponseDetail.NotFound(entity1: nameof(Company), entity2: nameof(Country));

            return NotFound(detailsObject);
        }

        await _contactRepository.AddAsync(newContact);
        try
        {
            await _contactRepository.SavedAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return StatusCode(StatusCodes.Status201Created, ResponseDetail.Created(newContact.Id));
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, UpdateEntityDto updateContact)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (! await _contactRepository.EntityExistsAsync(id))
        {
            return NotFound(ResponseDetail.NotFound(entity: nameof(Contact), id));
        }
        Contact contact = await _contactRepository.GetByIdAsync(id);
      
        contact.Name = updateContact.Name;

        _contactRepository.Update(contact);
           try
        {
            await _contactRepository.SavedAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        var updatedContact = _mapper.Map<EntityDto>(contact);

        return Ok(updatedContact);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        if (!await _contactRepository.EntityExistsAsync(id))
        {
            return NotFound(ResponseDetail.NotFound(entity: nameof(Contact), id));
        }

        await _contactRepository.DeleteAsync(id);
           try
        {
            await _contactRepository.SavedAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return NoContent();
    }

    [HttpGet("filter")]

    public async Task<ActionResult<List<ContactDto>>> FilterContactsAsync (int? countryId = null, int? companyId = null)
    {
        List<Contact> contacts = (List<Contact>)await _contactRepository.FilterContactsAsync(countryId, companyId);
        return _mapper.Map<List<ContactDto>>(contacts);
    }

    [HttpGet("fullInformation")]

    public async Task<ActionResult<List<ContactDto>>> GetContactsWithCompanyAndCountryAsync()
    {
        List<Contact> contacts = await _contactRepository.GetContactsWithCompanyAndCountryAsync();
        return _mapper.Map<List<ContactDto>>(contacts);
    }
}
