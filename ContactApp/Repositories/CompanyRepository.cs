using ContactApp.Data;
using ContactApp.Domain.Models;
using ContactApp.Interfaces;
using System.Diagnostics.Metrics;

namespace ContactApp.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private DataContext _context;

        public CompanyRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public bool CompanyExists(int id)
        {
            return _context.Companies.Any(country => country.Id == id);
        }

        public void CreateCompany(Company company)
        {
            _context.Add(company);
        }

        public void DeleteCompany(int id)
        {
            Company company = _context.Companies.FirstOrDefault(country => country.Id == id);
            _context.Remove(company);
        }

        public ICollection<Company> GetCompanies()
        {
            return _context.Companies.ToList();
        }

        public Company GetCompany(int id)
        {
            return _context.Companies.FirstOrDefault(c => c.Id == id);
        }

        public bool Saved()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public void UpdateCompany(Company company)
        {
            _context.Update(company);
        }
    }
}
