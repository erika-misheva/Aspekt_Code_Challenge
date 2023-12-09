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

        public async Task<ICollection<Contact>> FilterContactsAsync(int? countryId = null, int? companyId = null)
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

            return await query
                .Include(c => c.Company)
                .Include(c => c.Country)
                .ToListAsync();
        }

        public async Task<List<Contact>> GetContactsWithCompanyAndCountryAsync()
        {
            return await _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.Country)
                .ToListAsync();
        }


    }
}
