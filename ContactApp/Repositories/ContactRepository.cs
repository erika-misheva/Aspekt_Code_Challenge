using ContactApp.Data;
using ContactApp.Domain.Models;
using ContactApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace ContactApp.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private DataContext _context;

        public ContactRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public bool ContactExists(int id)
        {
            return _context.Contacts.Any(country => country.Id == id);
        }

        public void CreateContact(Contact contact)
        {
            _context.Add(contact);
        
        }

        public void DeleteContact(int id)
        {
            Contact contact = _context.Contacts.FirstOrDefault(co => co.Id == id);
            _context.Remove(contact);
      
        }

        public ICollection<Contact> FilterContacts(int countryId, int companyId)
        {
            return _context.Contacts.Where(co => co.CountryId == countryId && co.CompanyId == companyId)
                .Include(c => c.Company)
                .Include(c => c.Country)
                .ToList();
        }

        public Contact GetContact(int id)
        {
            return _context.Contacts.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Contact> GetContacts()
        {
            return _context.Contacts.ToList();
        }

        public List<Contact> GetContactsWithCompanyAndCountry()
        {
            return _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.Country)
                .ToList();
        }

        public bool Saved()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public void UpdateContact(Contact contact)
        {
            _context.Update(contact);
     
        }
    }
}
