namespace ContactApp.Data.Interfaces;

using Domain.Models;

public interface IContactRepository : IRepository<Contact>
{

    Task<List<Contact>> GetContactsWithCompanyAndCountryAsync();

    Task<ICollection<Contact>> FilterContactsAsync(int? countryId = null, int? companyId = null);

}
