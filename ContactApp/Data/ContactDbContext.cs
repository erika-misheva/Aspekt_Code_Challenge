﻿    using Microsoft.EntityFrameworkCore;

namespace ContactApp.Data;

using Interfaces;
using Domain.Models;


public class ContactDbContext : DbContext, IContactDbContext
{
    public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options)
    {
        Contacts = Set<Contact>();
        Companies = Set<Company>();
        Countries = Set<Country>();
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
