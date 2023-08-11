using System;
using System.Collections.Generic;
using System.Text;

namespace SDRDDBEncryption.Core.Models
{
    internal interface IDatabaseTable
    {

        
        public string DropEncrypted();

        public string CreateEncrypted();

        public List<string> GetColumns();

        public string GetTableName();

        public string GetEncTableName();

        public string DropTable();

        public string RenameTable();

    }
}
