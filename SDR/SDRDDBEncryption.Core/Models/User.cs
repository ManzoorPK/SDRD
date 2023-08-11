using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SDRDDBEncryption.Core.Models
{
    internal class User : BaseTable
    {
        public override string CreateEncrypted() => "CREATE TABLE `user_enc` (`id` int NOT NULL AUTO_INCREMENT, `name` varchar(40) NOT NULL, `username` varchar(60) NOT NULL, `password` varchar(40) NOT NULL, `dob` date NOT NULL, `image` text NOT NULL, `role` int(4) NOT NULL, `type` varchar(10) NOT NULL, `code` varchar(50) NOT NULL, `gender` varchar(20) NOT NULL, `email` varchar(100) NOT NULL, `mobile` varchar(30) NOT NULL, `phone` varchar(30) NOT NULL, `fax` varchar(30) NOT NULL, `address` text NOT NULL, `state` varchar(50) NOT NULL, `country` varchar(50) NOT NULL, `language` varchar(30) NOT NULL, `question` int(4) DEFAULT 0 NOT NULL, `answer` text, `theme` text NOT NULL, `createdate` datetime NOT NULL, `modifieddate` timestamp DEFAULT current_timestamp(), `updatedby` int(4) NOT NULL, `status` int(1) DEFAULT 1 NOT NULL, `delete` int(1) DEFAULT 0 NOT NULL, PRIMARY KEY (`id`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 DEFAULT COLLATE=latin1_swedish_ci;";
        public override List<string> GetColumns() => new List<string>() { "id",
"name",
"username",
"password",
"dob",
"image",
"role",
"type",
"code",
"gender",
"email",
"mobile",
"phone",
"fax",
"address",
"state",
"country",
"language",
"question",
"answer",
"theme",
"createdate",
"modifieddate",
"updatedby",
"status",
"delete"};
        public override string GetTableName() => "user";
    }
}
