using System;
using System.Collections.Generic;
using System.Text;

namespace SDRDDBEncryption.Core.Models
{
    internal abstract class BaseTable : IDatabaseTable
    {
        public abstract string CreateEncrypted();
        public string DropEncrypted()
        {
            return "Drop Table if exists `" + GetEncTableName()+ "`";
        }
        public string DropTable()
        {
            return "Drop Table if exists `" + GetTableName()+ "`";

        }
        public abstract List<string> GetColumns();
        public string GetEncTableName()
        {
            return GetTableName() + "_enc";
        }
        public abstract string GetTableName();
        public string RenameTable()
        {
            return "Rename Table `" + GetEncTableName() + "` To `" + GetTableName() + "`";

        }
    }
}
