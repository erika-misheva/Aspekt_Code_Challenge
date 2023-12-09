using AutoMapper;
using ContactApp.Domain.DTOs.Contact;
using ContactApp.Domain.DTOs.Shared;
using ContactApp.Domain.Models;

namespace ContactApp.Domain.Mappings
{
    public class ContactProfile : Profile
    {
        public ContactProfile() { 
        
        CreateMap<Company, EntityDto>();
        CreateMap<Country, EntityDto>();
        CreateMap<Contact, EntityDto>();

        CreateMap<UpdateEntityDto, Company>();
        CreateMap<UpdateEntityDto, Country>();

        CreateMap<Contact, CreateContactDto>().ReverseMap();
        CreateMap<Contact, ContactDto>()
           .ForMember(contactDto => contactDto.Company, opt => opt.MapFrom(contact => contact.Company))
           .ForMember(contactDto => contactDto.Country, opt => opt.MapFrom(contact => contact.Country));



        }
    }
}
