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
        public ActionResult<List<EntityDto>> GetContacts()
        {
            var contacts = _contactRepository.GetAll();
            return _mapper.Map<List<EntityDto>>(contacts);
        }

        [HttpGet("{id}")]

        public ActionResult<EntityDto> GetContact(int id)
        {
            var contact = _contactRepository.GetById(id);

            if (contact is null)
            {
                return NotFound();
            }

            return _mapper.Map<EntityDto>(contact);
        }

        [HttpPost]
        public ActionResult Create(CreateContactDto contactDto)
        {
            Contact newContact = _mapper.Map<Contact>(contactDto);

            _contactRepository.Add(newContact);
            _contactRepository.Saved();

            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpPut("{id}")]
        public ActionResult Update(int id, UpdateEntityDto updateContact)
        {

            if (!_contactRepository.EntityExists(id))
            {
                return NotFound();
            }
            Contact contact = _contactRepository.GetById(id);
            contact.Name = updateContact.Name;

            _contactRepository.Saved();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!_contactRepository.EntityExists(id))
            {
                return NotFound();
            }

            _contactRepository.Delete(id);
            _contactRepository.Saved();

            return Ok();
        }

        [HttpGet("filter")]

        public ActionResult<List<ContactDto>> FilterContacts (int? countryId = null, int? companyId = null)
        {
            List<Contact> contacts = _contactRepository.FilterContacts(countryId, companyId).ToList();
            return _mapper.Map<List<ContactDto>>(contacts);
        }

        [HttpGet("fullInformation/contacts")]

        public ActionResult<List<ContactDto>> GetContactsWithCompanyAndCountry()
        {
            List<Contact> contacts = _contactRepository.GetContactsWithCompanyAndCountry().ToList();
            return _mapper.Map<List<ContactDto>>(contacts);
        }
    }
}
