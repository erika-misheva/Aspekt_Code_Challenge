using ContactApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasOne(co => co.Company)
                .WithMany(c => c.Contacts)
                .HasForeignKey(co => co.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Contact>()
                .HasOne(co => co.Country)
                .WithMany(c => c.Contacts)
                .HasForeignKey(co => co.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
