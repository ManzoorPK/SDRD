using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDRDDBEncryption.Contracts.Services
{
    public  interface IEncryptionService
    {
        public List<String> GetTables();

        public List<String> GetColumns(string table);

        public Dictionary<string,List< string>> GetEncryptedTables();

        public bool EncryptTable(string tablename, List<string> columns);
    }
}
