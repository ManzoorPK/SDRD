using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace SDRDDBEncryption.Core.Models
{
    internal class Securityquestion : BaseTable
    {
        public override string CreateEncrypted() => "CREATE TABLE `securityquestion_enc` (`id` int(4) NOT NULL AUTO_INCREMENT, `name` text NOT NULL, `code` varchar(20) NOT NULL, `description` text NOT NULL, `createdate` datetime NOT NULL, `modifieddate` timestamp DEFAULT current_timestamp(), `updatedby` int(4) NOT NULL, `status` int(1) DEFAULT 1 NOT NULL, `delete` int(1) DEFAULT 0 NOT NULL, PRIMARY KEY (`id`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 DEFAULT COLLATE=latin1_swedish_ci;";
        public override List<string> GetColumns() => new List<string>()
        {
            "id",
"name",
"code",
"description",
"createdate",
"modifieddate",
"updatedby",
"status",
"delete"
        };
        public override string GetTableName() => "securityquestion";
    }
}
