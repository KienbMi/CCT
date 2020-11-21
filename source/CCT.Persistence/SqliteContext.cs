using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CCT.Persistence
{
    /// <summary>
    /// Sqlite DB context
    /// example for package manager console: add-migration InitialMigration -Context SqliteContext -OutputDir Migrations/SqliteMigrations
    /// </summary>
    public class SqliteContext : ApplicationDbContext
    {
        private string _environmentInfo = string.Empty;
        public SqliteContext()
        {

        }
        public SqliteContext(string environmentInfo)
        {
            _environmentInfo = environmentInfo;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var configuration = builder.Build();
                Debug.Write(configuration.ToString());

                if (_environmentInfo == "RPI2")
                {
                    string connectionString = configuration["ConnectionStrings:SqliteConnection"];
                    optionsBuilder.UseSqlite(connectionString);
                }
                else
                {
                    string connectionString = configuration["ConnectionStrings:SqliteConnection_RPI4"];
                    optionsBuilder.UseSqlite(connectionString);
                }
            }
        }
    }
}
