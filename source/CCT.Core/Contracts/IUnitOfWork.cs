using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Core.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonRepository PersonRepository { get; }
        ISettingRepository SettingRepository { get; }

        Task<int> SaveChangesAsync();
        int SaveChanges();

        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
        void MigrateDatabase();
        bool Exists();
    }
}
