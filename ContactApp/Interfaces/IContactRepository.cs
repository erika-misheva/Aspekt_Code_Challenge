using ContactApp.Domain.Models;

namespace ContactApp.Interfaces
{
    public interface IContactRepository
    {
        Contact GetContact(int id);
        ICollection<Contact> GetContacts();

        List<Contact> GetContactsWithCompanyAndCountry();

        ICollection<Contact> FilterContacts(int countryId, int companyId);
        bool ContactExists(int id);

        void CreateContact(Contact contact);

        void UpdateContact(Contact contact);

        void DeleteContact(int id);

        bool Saved();
    }
}
