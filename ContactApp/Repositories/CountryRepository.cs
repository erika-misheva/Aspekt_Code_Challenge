using ContactApp.Data;
using ContactApp.Domain.Models;
using ContactApp.Interfaces;

namespace ContactApp.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private DataContext _context;

        public CountryRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public bool CountryExists(int id)
        {
            return _context.Countries.Any(country => country.Id == id);
        }

        public void CreateCountry(Country country)
        {
            _context.Add(country);
           
        }

        public void DeleteCountry(int id)
        {
            Country country = _context.Countries.FirstOrDefault(country => country.Id == id);
            _context.Remove(country);

        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();

        }

        public Country GetCountry(int id)
        {
            return _context.Countries.FirstOrDefault(c => c.Id == id);
        }

        public bool Saved()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
         }

        public void UpdateCountry(Country country)
        {
            _context.Update(country);
        }

       
    }
}
