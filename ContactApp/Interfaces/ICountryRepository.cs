using ContactApp.Domain.Models;

namespace ContactApp.Interfaces
{
    public interface ICountryRepository
    {
        Country GetCountry(int id);
        ICollection<Country> GetCountries();
        bool CountryExists(int id);

        void CreateCountry(Country country);

        void UpdateCountry(Country country);

        void DeleteCountry(int id);

        bool Saved();
    }
}
