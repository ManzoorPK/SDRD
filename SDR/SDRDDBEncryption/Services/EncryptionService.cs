using Microsoft.Extensions.Options;
using SDRDDBEncryption.Contracts.Services;
using SDRDDBEncryption.Core.Contracts.Services;
using SDRDDBEncryption.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDRDDBEncryption.Services
{

    public class EncryptionService : IEncryptionService
    {
        private readonly AppConfig Config;
        private readonly IDatabaseService DataBaseService;
        public EncryptionService(IOptions<AppConfig> appConfig, IDatabaseService databaseService)
        {
            Config = appConfig.Value;
            DataBaseService = databaseService;
            DataBaseService.SetConnectionString(appConfig.Value.ConnectionString);
        }

        public List<String> GetTables()
        {
            return DataBaseService.GetAllTables();

        }

        public List<String> GetColumns(string table)
        {
            return DataBaseService.GetAllColumns(table);

        }

        public Dictionary<string, List<string>> GetEncryptedTables()
        {
            return DataBaseService.GetEncryptedTables();

        }


        public bool EncryptTable(string tablename, List<string> columns)
        {
            try
            {
                bool result = DataBaseService.EncryptTable(tablename, columns);

                return result;
            }
            catch

            {
                return false;
            }
        }

    }
}
