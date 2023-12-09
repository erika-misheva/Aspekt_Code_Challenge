using ContactApp.Domain.DTOs.Shared;

namespace ContactApp.Domain.DTOs.Contact
{
    public class ContactDto : EntityDto
    {
        public EntityDto Company { get; set; } = new();
        public EntityDto Country { get; set; } = new();
    }
}
