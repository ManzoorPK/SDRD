using System;
using System.Collections.Generic;
using System.Text;

namespace SDRDDBEncryption.Core.Models
{
    internal class Answersheetreval : BaseTable
    {

        private string dropEncrypted = @"DROP Table if exists answersheetreval_enc";

        private string createEncrypted = @"CREATE TABLE `answersheetreval_enc` (`id` bigint NOT NULL AUTO_INCREMENT, `answersheet` bigint NOT NULL COMMENT 'Primary key of answer sheet table.', `grade` varchar(100) NOT NULL, `notes` text NOT NULL, `ConvertedXODLink` text NOT NULL, `AnnotationsLink` text NOT NULL, `AnnotedPDFLink` text NOT NULL, `createdate` datetime NOT NULL, `modifieddate` timestamp DEFAULT current_timestamp(), `updatedby` int(4) NOT NULL, `status` int(1) DEFAULT 1 NOT NULL, `delete` int(1) DEFAULT 0 NOT NULL, PRIMARY KEY (`id`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 DEFAULT COLLATE=latin1_swedish_ci;";

        private List<string> columns = new List<string>() { "id", "answersheet", "grade", "notes", "ConvertedXODLink", "AnnotationsLink", "AnnotedPDFLink", "createdate", "modifieddate", "updatedby", "status", "delete" };

        public List<string> SearchColumns = new List<string>() { "notes" };


        public string DropEncrypted()
        { return dropEncrypted; }       

        public override  string CreateEncrypted()
        { return createEncrypted; } 

        public override List<string> GetColumns()
        { return columns; } 

        public override string GetTableName()
        {
            return "Answersheetreval";
        }

        public string GetEncTableName()
        {
            return GetTableName() + "_enc";

        }
    }
}
