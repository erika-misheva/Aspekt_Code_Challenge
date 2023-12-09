namespace ContactApp.Domain.Models
{
    public class Company : BaseModel
    {
        public ICollection<Contact> Contacts { get; set; }
    }
}
