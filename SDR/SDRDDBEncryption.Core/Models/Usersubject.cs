using System;
using System.Collections.Generic;
using System.Text;

namespace SDRDDBEncryption.Core.Models
{
    internal class Usersubject : BaseTable
    {
        public override string CreateEncrypted() => "CREATE TABLE `usersubject_enc` (`id` int NOT NULL AUTO_INCREMENT, `professor` int(4) NOT NULL, `subject` int(4) NOT NULL, `createdate` datetime NOT NULL, `modifieddate` timestamp DEFAULT current_timestamp() ON UPDATE current_timestamp(), `updatedby` int(4) NOT NULL, `delete` int(1) DEFAULT 0 NOT NULL, PRIMARY KEY (`id`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 DEFAULT COLLATE=latin1_swedish_ci;";
        public override List<string> GetColumns() => new List<string>()
        { "id",
"professor",
"subject",
"createdate",
"modifieddate",
"updatedby",
"delete"};
        public override string GetTableName() => "usersubject";
    }
}
