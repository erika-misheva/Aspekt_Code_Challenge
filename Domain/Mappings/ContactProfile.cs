using AutoMapper;

namespace Domain.Mappings;

using DTOs.Contact;
using DTOs.Shared;
using Models;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<Company, EntityDto>();
        CreateMap<Country, EntityDto>();
        CreateMap<Contact, EntityDto>();

        CreateMap<UpdateEntityDto, Company>();
        CreateMap<UpdateEntityDto, Country>();

        CreateMap<Contact, CreateContactDto>().ReverseMap();
        CreateMap<Contact, ContactDto>();
    }
}
