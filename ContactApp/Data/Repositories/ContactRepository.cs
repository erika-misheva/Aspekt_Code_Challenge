using Microsoft.EntityFrameworkCore;

namespace ContactApp.Data.Repositories;

using ContactApp.Data.Interfaces;
using Data;
using Domain.Models;

public class ContactRepository : Repository<Contact>, IContactRepository
{
    public ContactRepository(IContactDbContext contactDbContext) : base(contactDbContext)
    {

    }

    public async Task<ICollection<Contact>> FilterContactsAsync(int? countryId = null, int? companyId = null)
    {
        IQueryable<Contact> contactsQuery = _context.Contacts.AsQueryable();

        bool hasCountryAndCompany = countryId != null && companyId != null;
        bool hasCountryOrCompany = countryId != null || companyId != null;

        if (hasCountryAndCompany)
        {
            contactsQuery = contactsQuery.Where(co => co.CountryId == countryId
                                                   && co.CompanyId == companyId);
        }

        if (hasCountryOrCompany)
        {
            contactsQuery = contactsQuery.Where(co => co.CountryId == countryId
                                                   || co.CompanyId == companyId);
        }

        return await contactsQuery
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
