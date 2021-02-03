using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Core.Contracts
{
    public interface ISettingRepository
    {
        Task SetStorageDurationAsync(int days);
        Task<int> GetStorageDurationAsync();
        Task SetPasswordAsync(string password);
        Task<string> GetPasswordAsync();
        Task SetWelcomeTextAsync(string welcomeText);
        Task<string> GetWelcomeTextAsync();
        Task SetNfcReaderTypeAsync(NfcReaderType nfcReader);
        Task<NfcReaderType> GetNfcReaderTypeAsync();
    }
}
