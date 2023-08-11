using System;
using System.Collections.Generic;
using System.Text;

namespace SDRDDBEncryption.Core.Models
{
    internal class Subject : BaseTable
    {
        public override string CreateEncrypted() => "CREATE TABLE `subject_enc` (`id` int(4) NOT NULL AUTO_INCREMENT, `major` int(4) NOT NULL, `name` varchar(1500) COLLATE utf8mb3_unicode_ci NOT NULL, `code` varchar(20) NOT NULL, `description` text NOT NULL, `createdate` datetime NOT NULL, `modifieddate` timestamp DEFAULT current_timestamp(), `updatedby` int(4) NOT NULL, `status` int(1) DEFAULT 1 NOT NULL, `delete` int(1) DEFAULT 0 NOT NULL, PRIMARY KEY (`id`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 DEFAULT COLLATE=latin1_swedish_ci;";
        public override List<string> GetColumns() => new List<string>() { "id",
                                                                            "major",
                                                                            "name",
                                                                            "code",
                                                                            "description",
                                                                            "modifieddate",
                                                                            "createdate",
                                                                            "updatedby",
                                                                            "status",
                                                                            "delete"};
        public override string GetTableName() => "subject";
    }
}
