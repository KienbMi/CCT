﻿using CCT.Core.Contracts;
using CCT.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private bool _disposed;

        public UnitOfWork()
        {
            string actEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (actEnvironment != null && actEnvironment.StartsWith("RPI"))
            {
                _dbContext = new SqliteContext(actEnvironment);
            }
            else
            {
                _dbContext = new ApplicationDbContext();
            }
            PersonRepository = new PersonRepository(_dbContext);
            SettingRepository = new SettingRepository(_dbContext);
        }

        public UnitOfWork(ApplicationDbContext dbContext)
        {            
            _dbContext = dbContext;
            PersonRepository = new PersonRepository(_dbContext);
            SettingRepository = new SettingRepository(_dbContext);
        }

        public IPersonRepository PersonRepository { get; }
        public ISettingRepository SettingRepository { get; }


        /// <summary>
        /// Repository-übergreifendes Speichern der Änderungen
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// Check if database exists
        /// </summary>
        /// <returns></returns>
        public bool Exists() => _dbContext.Database.CanConnect();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }
        public async Task DeleteDatabaseAsync() => await _dbContext.Database.EnsureDeletedAsync();
        public async Task MigrateDatabaseAsync() => await _dbContext.Database.MigrateAsync();
        public void MigrateDatabase() => _dbContext.Database.Migrate();
    }
}
