using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TurkeyWebsiteProject.Models;

namespace TurkeyWebsiteProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<ImportantPerson> ImportantPeople { get; set; }
        public DbSet<Territory> Territories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
