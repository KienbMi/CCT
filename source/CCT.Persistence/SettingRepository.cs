using CCT.Core;
using CCT.Core.Contracts;
using CCT.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Persistence
{
    public class SettingRepository : ISettingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        //Parameternames
        const string P01_StorageDuration = "StorageDuration";
        const string P02_Password = "Password";
        const string P03_WelcomeText = "WelcomeText";
        const string P04_NfcReaderType = "NfcReaderType";

        public SettingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetPasswordAsync()
        {
            string defaultValue = "cct";

            var password = await _dbContext.Settings
                .SingleOrDefaultAsync(s => s.Name == P02_Password);
            
            if (password == null)
            {
                return defaultValue;
            }
            else
            {
                return password.Value;
            }
        }

        public async Task<int> GetStorageDurationAsync()
        {
            int defaultValue = 30;

            var storageDuration = await _dbContext.Settings
                .SingleOrDefaultAsync(s => s.Name == P01_StorageDuration);

            if (storageDuration == null)
            {
                return defaultValue;
            }
            else if(storageDuration.Type == 1 && int.TryParse(storageDuration.Value, out int value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public async Task<string> GetWelcomeTextAsync()
        {
            string defaultValue = "Herzlich Willkommen!";

            var welcomeText = await _dbContext.Settings
                .SingleOrDefaultAsync(s => s.Name == P03_WelcomeText);

            if (welcomeText == null)
            {
                return defaultValue;
            }
            else
            {
                return welcomeText.Value;
            }
        }

        public async Task<NfcReaderType> GetNfcReaderTypeAsync()
        {
            NfcReaderType defaultValue = NfcReaderType.uFr;

            var nfcReaderType = await _dbContext.Settings
                .SingleOrDefaultAsync(s => s.Name == P04_NfcReaderType);

            if (nfcReaderType == null)
            {
                return defaultValue;
            }
            else
            {
                try
                {
                    return (NfcReaderType)Enum.Parse(typeof(NfcReaderType), nfcReaderType.Value);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }

        public async Task SetPasswordAsync(string password)
        {
            var passwordInDb = await _dbContext.Settings
                .SingleOrDefaultAsync(s => s.Name == P02_Password);

            if (passwordInDb == null)
            {
                Setting newSetting = new Setting
                {
                    Name = P02_Password,
                    Type = 0,
                    Value = password
                };
                
                _dbContext.Add(newSetting);
            }
            else
            {
                passwordInDb.Value = password;
            }
        }

        public async Task SetStorageDurationAsync(int days)
        {
            var storageDurationInDb = await _dbContext.Settings
                .SingleOrDefaultAsync(s => s.Name == P01_StorageDuration);

            if (storageDurationInDb == null)
            {
                Setting newSetting = new Setting
                {
                    Name = P01_StorageDuration,
                    Type = 1,
                    Value = days.ToString()
                };

                _dbContext.Add(newSetting);
            }
            else
            {
                storageDurationInDb.Value = days.ToString();
            }
        }

        public async Task SetWelcomeTextAsync(string welcomeText)
        {
            var welcomeTextInDb = await _dbContext.Settings
                .SingleOrDefaultAsync(s => s.Name == P03_WelcomeText);

            if (welcomeTextInDb == null)
            {
                Setting newSetting = new Setting
                {
                    Name = P03_WelcomeText,
                    Type = 0,
                    Value = welcomeText
                };

                _dbContext.Add(newSetting);
            }
            else
            {
                welcomeTextInDb.Value = welcomeText;
            }
        }

        public async Task SetNfcReaderTypeAsync(NfcReaderType nfcReaderType)
        {
            var nfcReaderTypeInDb = await _dbContext.Settings
                .SingleOrDefaultAsync(s => s.Name == P04_NfcReaderType);

            if (nfcReaderTypeInDb == null)
            {
                Setting newSetting = new Setting
                {
                    Name = P04_NfcReaderType,
                    Type = 2,
                    Value = nfcReaderType.ToString()
                };

                _dbContext.Add(newSetting);
            }
            else
            {
                nfcReaderTypeInDb.Value = nfcReaderType.ToString();
            }
        }
    }
}
