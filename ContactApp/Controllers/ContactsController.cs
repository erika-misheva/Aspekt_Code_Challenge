using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers;

using Domain.Common;
using Data.Interfaces;
using Domain.DTOs.Contact;
using Domain.DTOs.Shared;
using Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


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
    public async Task<ActionResult<List<EntityDto>>> GetContacts()
    {
        var contacts = await _contactRepository.GetAllAsync();
        return _mapper.Map<List<EntityDto>>(contacts);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<EntityDto>> GetContact(int id)
    {
        var contact = await _contactRepository.GetByIdAsync(id);

        if (contact is null)
        {
            return NotFound();
        }

        return _mapper.Map<EntityDto>(contact);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateContactDto contactDto)
    {
        Contact newContact = _mapper.Map<Contact>(contactDto);
        bool companyExists = await _countryRepository.EntityExistsAsync(newContact.CountryId);
        bool countryExists = await _companyRepository.EntityExistsAsync(newContact.CompanyId);

        if (!companyExists || !countryExists)
        {
            return NotFound("The Company or Country could not be found");
        }

        await _contactRepository.AddAsync(newContact);
        await _contactRepository.SavedAsync();

        return StatusCode(StatusCodes.Status201Created, ResponseDetail.Created(newContact.Id));
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateEntityDto updateContact)
    {

        if (! await _contactRepository.EntityExistsAsync(id))
        {
            return NotFound();
        }
        Contact contact = await _contactRepository.GetByIdAsync(id);
      
        contact.Name = updateContact.Name;

        _contactRepository.Update(contact);
        await _contactRepository.SavedAsync();

        var updatedContact = _mapper.Map<EntityDto>(contact);

        return Ok(updatedContact);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        if (!await _contactRepository.EntityExistsAsync(id))
        {
            return NotFound();
        }

        await _contactRepository.DeleteAsync(id);
        await _contactRepository.SavedAsync();

        return Ok();
    }

    [HttpGet("filter")]

    public async Task<ActionResult<List<ContactDto>>> FilterContacts (int? countryId = null, int? companyId = null)
    {
        List<Contact> contacts = (List<Contact>)await _contactRepository.FilterContactsAsync(countryId, companyId);
        return _mapper.Map<List<ContactDto>>(contacts);
    }

    [HttpGet("fullInformation")]

    public async Task<ActionResult<List<ContactDto>>> GetContactsWithCompanyAndCountry()
    {
        List<Contact> contacts = await _contactRepository.GetContactsWithCompanyAndCountryAsync();
        return _mapper.Map<List<ContactDto>>(contacts);
    }
}
