using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Core.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonRepository PersonRepository { get; }

        Task<int> SaveChangesAsync();

        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
        bool Exists();
    }
}
