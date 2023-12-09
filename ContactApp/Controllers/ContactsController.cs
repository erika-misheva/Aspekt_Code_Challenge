using AutoMapper;
using ContactApp.Domain.DTOs.Contact;
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
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactsController(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
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

            await _contactRepository.AddAsync(newContact);
            await _contactRepository.SavedAsync();

            return StatusCode(StatusCodes.Status201Created);
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

            await _contactRepository.SavedAsync();

            return Ok();
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

        [HttpGet("fullInformation/contacts")]

        public async Task<ActionResult<List<ContactDto>>> GetContactsWithCompanyAndCountry()
        {
            List<Contact> contacts = await _contactRepository.GetContactsWithCompanyAndCountryAsync();
            return _mapper.Map<List<ContactDto>>(contacts);
        }
    }
}
