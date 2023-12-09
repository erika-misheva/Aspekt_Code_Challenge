using ContactApp.Domain.Models;

namespace ContactApp.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
   
        List<Contact> GetContactsWithCompanyAndCountry();

        ICollection<Contact> FilterContacts(int? countryId = null, int? companyId = null);
       
    }
}
