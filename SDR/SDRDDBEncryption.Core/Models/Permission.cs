using System;
using System.Collections.Generic;
using System.Data;
using System.Security;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SDRDDBEncryption.Core.Models
{
    internal class Permission : BaseTable
    {
        public override string CreateEncrypted() => @"CREATE TABLE `permission_enc` (`id` int (4) NOT NULL AUTO_INCREMENT, `permission` text NOT NULL, `role` int (4) NOT NULL, `createdate` datetime NOT NULL, `modifieddate` timestamp DEFAULT current_timestamp(), `updatedby` int (4) NOT NULL, `delete` int (1) DEFAULT 0 NOT NULL, PRIMARY KEY(`id`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 DEFAULT COLLATE=latin1_swedish_ci;";

        public override List<string> GetColumns() => new List<string> { "id",
"permission",
"role",
"createdate",
"modifieddate",
"updatedby",
"delete"};
        public override string GetTableName() => "permission";
    }
}
