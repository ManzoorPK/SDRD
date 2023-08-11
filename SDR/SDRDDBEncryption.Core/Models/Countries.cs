using System;
using System.Collections.Generic;
using System.Text;

namespace SDRDDBEncryption.Core.Models
{
    internal class Countries : BaseTable
    {
        public override string CreateEncrypted() => "CREATE TABLE `countries_enc` (`id` int NOT NULL AUTO_INCREMENT, `country_code` varchar(2) NOT NULL, `name` varchar(100) NOT NULL, `delete` int(1) DEFAULT 0 NOT NULL, PRIMARY KEY (`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 DEFAULT COLLATE=utf8mb3_general_ci;";
        public override List<string> GetColumns() => new List<string>() {"id","country_code","name","delete"};
        public override string GetTableName() => "Countries";
    }
}
