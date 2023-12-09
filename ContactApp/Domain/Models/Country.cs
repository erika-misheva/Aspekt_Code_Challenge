namespace ContactApp.Domain.Models
{
    public class Country : BaseModel
    {
        public ICollection<Contact> Contacts { get; set; }
    }
}
