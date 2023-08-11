using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace SDRDDBEncryption.Core.Models
{
    internal class Examination : BaseTable
    {
        public override string CreateEncrypted() => "CREATE TABLE `examination_enc` (`id` bigint NOT NULL AUTO_INCREMENT, `number` varchar(50) NOT NULL, `date` date NOT NULL, `year` varchar(50) DEFAULT ' ' NOT NULL, `semester` varchar(50) NOT NULL, `professor` int(5) NOT NULL, `professorcode` varchar(50) NOT NULL, `professorname` varchar(50) NOT NULL, `subject` int(5) NOT NULL, `totalstudent` int(5) NOT NULL, `questionsheet` varchar(100) NOT NULL, `remark` text NOT NULL, `status` varchar(30) NOT NULL, `completion` int(4) DEFAULT 0 NOT NULL, `submited` int(1) DEFAULT 0 NOT NULL, `recheck` bigint DEFAULT 0 NOT NULL, `latedays` int DEFAULT 0 NOT NULL, `createdate` datetime NOT NULL, `modifieddate` timestamp DEFAULT current_timestamp(), `updatedby` int(4) NOT NULL, `delete` int(1) DEFAULT 0 NOT NULL, PRIMARY KEY (`id`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 DEFAULT COLLATE=latin1_swedish_ci;";
        public override List<string> GetColumns() => new List<string>() {"id",
"number",
"date",
"year",
"semester",
"professor",
"professorcode",
"professorname",
"subject",
"totalstudent",
"questionsheet",
"remark",
"status",
"completion",
"submited",
"recheck",
"latedays",
"createdate",
"modifieddate",
"updatedby",
"delete"};
        public override string GetTableName() => "examination";
    }
}
