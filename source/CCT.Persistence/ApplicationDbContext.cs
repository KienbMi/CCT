using System;
using System.Diagnostics;
using CCT.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CCT.Persistence
{
    public class ApplicationDbContext: DbContext
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

                string actEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                if (actEnvironment == "RPI2")
                {
                    string connectionString = configuration["ConnectionStrings:SqliteConnection"];
                    optionsBuilder.UseSqlite(connectionString);
                }
                else
                {
                    string connectionString = configuration["ConnectionStrings:DefaultConnection"];
                    optionsBuilder.UseSqlServer(connectionString);
                }
            }
        }
    }
}
