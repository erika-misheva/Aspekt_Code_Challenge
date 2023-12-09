using ContactApp.Domain.Models;

namespace ContactApp.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {

        Task<List<Contact>> GetContactsWithCompanyAndCountryAsync();

        Task<ICollection<Contact>> FilterContactsAsync(int? countryId = null, int? companyId = null);

    }
}
