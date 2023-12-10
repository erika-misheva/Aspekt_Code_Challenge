using Microsoft.EntityFrameworkCore;

namespace ContactApp.Data.Interfaces;

using Domain.Models;

public interface IContactDbContext
{
    DbSet<Company> Companies { get; set; }
    DbSet<Contact> Contacts { get; set; }
    DbSet<Country> Countries { get; set; }
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}