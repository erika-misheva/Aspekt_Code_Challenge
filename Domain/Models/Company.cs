namespace Domain.Models;

public class Company : BaseModel
{
    public List<Contact> Contacts { get; set; }
}
