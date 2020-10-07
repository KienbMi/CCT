﻿using CCT.Core.Contracts;
using ClubAdministration.Persistence;
using Microsoft.EntityFrameworkCore;
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
            _dbContext = new ApplicationDbContext();
            PersonRepository = new PersonRepository(_dbContext);
        }

        public IPersonRepository PersonRepository { get; }


        /// <summary>
        /// Repository-übergreifendes Speichern der Änderungen
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

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
    }
}