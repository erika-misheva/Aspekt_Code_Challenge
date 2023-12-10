namespace Domain.DTOs.Contact;

using Shared;

public class ContactDto : EntityDto
{
    public EntityDto Company { get; set; } = new();
    public EntityDto Country { get; set; } = new();
}
