using System;
using System.Diagnostics;
using CCT.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CCT.Persistence
{
    /// <summary>
    /// localDb context
    /// example for package manager console: add-migration InitialMigration -Context ApplicationDbContext
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var configuration = builder.Build();
                Debug.Write(configuration.ToString());

                string connectionString = configuration["ConnectionStrings:DefaultConnection"];
                optionsBuilder.UseSqlServer(connectionString);               
            }
        }
    }
}
