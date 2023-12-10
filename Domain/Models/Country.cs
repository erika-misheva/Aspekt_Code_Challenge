namespace Domain.Models;

public class Country : BaseModel
{
    public List<Contact> Contacts { get; set; }
}
