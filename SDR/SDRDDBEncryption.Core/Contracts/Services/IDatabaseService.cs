using SDRDDBEncryption.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDRDDBEncryption.Core.Contracts.Services
{
    public interface IDatabaseService 
    {

        void SetConnectionString(string connectionstring);
        List<string> GetAllTables();

        List<string> GetAllColumns(string table);

        public Dictionary<string, List<string>> GetEncryptedTables();
        bool EncryptTable(string tableName, List<string> ColsToEnrypt);

        List<SearchResults> GetSearchResults(string searchText);
    }
}
