using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using static Azure.Core.HttpHeader;

namespace SDRDDBEncryption.Core.Models
{
    internal class Answersheets : BaseTable
    {

        public List<string> SearchColumns = new List<string> { "studentcode", "studentname"};

        public override string CreateEncrypted() => @"CREATE TABLE `answersheets_enc` (`id` int NOT NULL AUTO_INCREMENT, `examination` int NOT NULL, `student` int NOT NULL, `studentcode` varchar(100) NOT NULL, `studentname` varchar(100) NOT NULL, `grade` varchar(100) NOT NULL, `notes` text COLLATE utf8mb3_swedish_ci NOT NULL, `OriginalPDFName` varchar(175) NOT NULL, `FileSize` varchar(10) NOT NULL, `SysGeneratedName` varchar(75) NOT NULL, `UploadedPDFLink` varchar(250) NOT NULL, `UploadDateTime` datetime NOT NULL, `ConvertedXODLink` varchar(250) NOT NULL, `AnnotationsLink` varchar(250), `LastModification` timestamp DEFAULT current_timestamp() NULL, `AnnotedPDFLink` varchar(250), `status` int(1) DEFAULT 0 NOT NULL, `reval` int(1) DEFAULT 0 NOT NULL, `submited` int(1) DEFAULT 0 NOT NULL, `delete` int(1) DEFAULT 0 NOT NULL, `updatedby` int DEFAULT 1 NOT NULL, PRIMARY KEY (`id`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 DEFAULT COLLATE=latin1_swedish_ci;";
        public override List<string> GetColumns() => new List<string> { "id", "examination", "student", "studentcode", "studentname", "grade", "notes", "OriginalPDFName", "FileSize", "SysGeneratedName", "UploadedPDFLink", "UploadDateTime", "ConvertedXODLink", "AnnotationsLink", "LastModification", "AnnotedPDFLink", "status", "reval", "submited", "delete", "updatedby" };
            
        public override string GetTableName() => "answersheets";
    }
}
