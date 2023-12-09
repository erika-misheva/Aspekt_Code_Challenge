using ContactApp.Data;
using ContactApp.Domain.Models;
using ContactApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace ContactApp.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(DataContext dataContext) : base(dataContext)
        {
            
        }

        public ICollection<Contact> FilterContacts(int? countryId = null, int? companyId = null)
        {
            var query = _context.Contacts.AsQueryable();

            if (countryId.HasValue)
            {
                query = query.Where(co => co.CountryId == countryId.Value);
            }

            if (companyId.HasValue)
            {
                query = query.Where(co => co.CompanyId == companyId.Value);
            }

            return query
                .Include(c => c.Company)
                .Include(c => c.Country)
                .ToList();
        }

        public List<Contact> GetContactsWithCompanyAndCountry()
        {
            return _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.Country)
                .ToList();
        }

     
    }
}
