using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.DataClassification;
using MySqlConnector;
using SDRDDBEncryption.Core.Contracts.Services;
using SDRDDBEncryption.Core.Models;

namespace SDRDDBEncryption.Core.Services
{
    public class DatabaseService : IDatabaseService
    {
        private string ConnectionString;

        private string CreateInfoTable = @"CREATE TABLE if not exists `applicationinfo` (`id` int(4) NOT NULL AUTO_INCREMENT, `tablename` text NOT NULL, `columnname` text NOT NULL,`IsEncrypted` text NOT NULL, PRIMARY KEY (`id`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 DEFAULT COLLATE=latin1_swedish_ci;";
        private string DeleteTableInfo = "Delete from `applicationinfo` where tablename=";


        private string Key = "D6E2D35C80AA466C9C23A0120874B4B2";

        private Dictionary<string, List<string>> loadColoumns = null;
        public DatabaseService()
        {
        }

        public DataTable FillData(string Qry)
        {
            DataTable dt = new DataTable();
            MySqlConnector.MySqlConnection connection = new MySqlConnector.MySqlConnection(ConnectionString);
            try
            {
                MySqlCommand CMD = new MySqlCommand(ConnectionString, connection);
                CMD.Connection.Open();
                CMD.CommandText = Qry;
                CMD.CommandType = CommandType.Text;
                MySqlDataAdapter DA = new MySqlDataAdapter(CMD);
                DA.Fill(dt);
            }
            catch (Exception ex) { }
            finally { connection.Close(); }
            return dt;
        }

        public void SetConnectionString(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        public List<string> GetAllTables()
        {
            string exclude = "information_schema,mysql,performance_schema";
            List<string> tables = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                DataTable tableSchema = connection.GetSchema("Tables");

                foreach (DataRow row in tableSchema.Rows)
                {
                    if (!exclude.Contains(row[1].ToString().ToLower()) && row[3].ToString().ToUpper().Equals("BASE TABLE"))
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        bool isFound = false;
                        foreach (var t in tables)
                        {
                            if (t == tableName || tableName.Contains("pma_"))
                            {
                                isFound = true;
                                break;
                            }

                        }
                        if (!isFound)
                            tables.Add(tableName);
                    }
                }
            }

            return tables;
        }



        public List<string> GetAllColumns(string table)
        {
            List<string> columns = new List<string>();
            if (loadColoumns == null)
            {

                loadColoumns = new Dictionary<string, List<string>>();

                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    DataTable tableSchema = connection.GetSchema("Columns");



                    DataTable KeyColumns = connection.GetSchema("KeyColumnUsage");//, new string[] {"", table,"" });

                    Dictionary<string, List<string>> ids = new Dictionary<string, List<string>>();

                    foreach (DataRow row in KeyColumns.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        string columnsName = row["COLUMN_NAME"].ToString();

                        if (!ids.ContainsKey(tableName))
                        {
                            ids.Add(tableName, new List<string>());
                        }
                        ids[tableName].Add(columnsName);

                    }

                    foreach (DataRow row in tableSchema.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        string columnsName = row["COLUMN_NAME"].ToString();
                        if (ids.ContainsKey(tableName) && !ids[tableName].Contains(columnsName))
                        {
                            if (!loadColoumns.ContainsKey(tableName))
                            {
                                loadColoumns.Add(tableName, new List<string>());
                            }
                            loadColoumns[tableName].Add(columnsName);
                        }
                    }
                }
            }

            foreach (var colName in loadColoumns[table])
            {
                bool isFound = false;
                foreach (var c in columns)
                {
                    if (c == colName)
                    {
                        isFound = true;
                        break;
                    }

                }
                if (!isFound)
                    columns.Add(colName);
            }

            return columns;
        }


        public Dictionary<string, List<string>> GetEncryptedTables()
        {

            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            string sql = "select tablename,columnname from applicationinfo WHERE IsEncrypted IS NULL";

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                var reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        var tablename = reader.GetString(0).ToLower();
                        var columnsName = reader.GetString(1).ToLower();
                        if (!result.ContainsKey(tablename))
                        {
                            result.Add(tablename, new List<string>());
                        }
                        result[tablename].Add(columnsName);
                    }
                }
                finally
                {
                    reader.Close();

                }


            }
            return result;
        }

        public bool EncryptTable(string tableName, List<string> colsToEnrypt)
        {

            var instance = (from t in Assembly.GetExecutingAssembly().GetTypes()
                            where t.GetInterfaces().Contains(typeof(IDatabaseTable))
                            && t.GetConstructor(Type.EmptyTypes) != null
                            && t.FullName.IndexOf(tableName, StringComparison.InvariantCultureIgnoreCase) > 0
                            select Activator.CreateInstance(t) as IDatabaseTable).Take(1).FirstOrDefault();

            MySqlTransaction Trans;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();


                Trans = connection.BeginTransaction();
                //try

                //{
                string DropSQL = instance.DropEncrypted();

                MySqlCommand dropCommand = new MySqlCommand(DropSQL, connection, Trans);
                dropCommand.ExecuteNonQuery();

                string CreateSQL = instance.CreateEncrypted();
                MySqlCommand createCommand = new MySqlCommand(CreateSQL, connection, Trans);
                createCommand.ExecuteNonQuery();

                foreach (var cls in colsToEnrypt)
                {
                    string alterCols = @"ALTER TABLE " + tableName + "_enc " +
                                       @"MODIFY " + cls + " VARBINARY(1500);";
                    MySqlCommand createfCommand = new MySqlCommand(alterCols, connection, Trans);
                    createfCommand.ExecuteNonQuery();

                }

                var dt_AlreadyEncryptedColumns = FillData("SELECT * FROM applicationinfo WHERE tablename='" + tableName + "'");

                if (dt_AlreadyEncryptedColumns.Rows.Count > 0)
                {
                    foreach (DataRow iRow in dt_AlreadyEncryptedColumns.Rows)
                    {
                        string alterCols = @"ALTER TABLE " + tableName + "_enc " +
                                           @"MODIFY " + iRow["columnname"] + " VARBINARY(1500);";
                        MySqlCommand createffCommand = new MySqlCommand(alterCols, connection, Trans);
                        createffCommand.ExecuteNonQuery();

                    }
                }

                String insertSQL = CreateInsertSQL(instance, colsToEnrypt);

                MySqlCommand insertCommand = new MySqlCommand(insertSQL, connection, Trans);
                insertCommand.ExecuteNonQuery();

                MySqlCommand DropOriginal = new MySqlCommand(instance.DropTable(), connection, Trans);
                DropOriginal.ExecuteNonQuery();

                MySqlCommand RenameCommand = new MySqlCommand(instance.RenameTable(), connection, Trans);
                RenameCommand.ExecuteNonQuery();

                MySqlCommand InfoCreateCommand = new MySqlCommand(CreateInfoTable, connection, Trans);
                InfoCreateCommand.ExecuteNonQuery();


                //string sql = DeleteTableInfo + " '" + tableName + "'";
                //MySqlCommand DeleteInfoCommand = new MySqlCommand(sql, connection, Trans);
                //DeleteInfoCommand.ExecuteNonQuery();

                MySqlCommand InsertInfoCommand = new MySqlCommand(connection, Trans);
                foreach (string col in colsToEnrypt)
                {
                    var dt_AlreadyExists = FillData("SELECT * FROM applicationinfo WHERE tablename='" + tableName + "' AND columnname='" + col + "'");
                    if (dt_AlreadyExists.Rows.Count == 0)
                    {
                        InsertInfoCommand.CommandText = "Insert into applicationinfo (`tablename`,`columnname`, `IsEncrypted`) values ('" + tableName + "','" + col + "','YES')";
                        InsertInfoCommand.ExecuteNonQuery();
                    }
                }

                dropCommand.ExecuteNonQuery();

                Trans.Commit();


                //}
                //catch (Exception ex)
                //{
                //    Trans.Rollback();
                //    return false;
                //}
            }
            return true;
        }

        private string CreateInsertSQL(IDatabaseTable table, List<string> colsToEncrypt)
        {
            string resultSQL = "";

            List<String> sb = new List<String>();

            var allreadyEnc = GetEncryptedTables();


            foreach (var col in table.GetColumns())
            {
                //if (colsToEncrypt.Contains(col, StringComparer.InvariantCultureIgnoreCase) && !allreadyEnc[table.GetTableName().ToLower()].Contains(col, StringComparer.InvariantCultureIgnoreCase))
                bool isFound = false;
                foreach (var c in colsToEncrypt)
                {
                    if (c == col)
                    { isFound = true; break; }
                }

                if (isFound)
                {

                    var dt = FillData("SELECT * FROM applicationinfo WHERE tablename='" + table.GetTableName() + "' AND columnname ='" + col + "'");
                    if (dt.Rows.Count == 0)
                        sb.Add("AES_ENCRYPT(`" + col + "`,'" + Key + "') as `" + col + "`");
                    else
                        sb.Add("`" + col + "`");
                }
                else
                {
                    sb.Add("`" + col + "`");
                }
            }

            string cols = String.Join(",", sb.ToArray());

            resultSQL = "insert into " + table.GetEncTableName() + " Select " + cols + " from " + table.GetTableName();


            return resultSQL;
        }


        public List<SearchResults> GetSearchResults(string searchText)
        {
            List<SearchResults> result = new List<SearchResults>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string sqlCommand = "select * from Answersheetreval reval\r\njoin answersheets answer on reval.answersheet = answer.id ";

                Answersheetreval answersheetreval = new Answersheetreval();

                var allreadyEnc = GetEncryptedTables();

                List<String> sb = new List<String>();

                foreach (var col in answersheetreval.SearchColumns)
                {
                    if (allreadyEnc[answersheetreval.GetTableName().ToLower()].Contains(col, StringComparer.InvariantCultureIgnoreCase))
                    {
                        sb.Add("AES_DECRYPT(`" + col + "`,'" + Key + "') like '" + searchText + "'");
                    }
                    else
                    {
                        sb.Add("`" + col + "` like '" + searchText + "'");
                    }
                }


                Answersheets answersheets = new Answersheets();

                foreach (var col in answersheets.SearchColumns)
                {
                    if (allreadyEnc[answersheets.GetTableName().ToLower()].Contains(col, StringComparer.InvariantCultureIgnoreCase))
                    {
                        sb.Add("AES_DECRYPT(`" + col + "`,'" + Key + "') like '" + searchText + "'");
                    }
                    else
                    {
                        sb.Add("`" + col + "` like '" + searchText + "'");
                    }
                }

                string whereString = string.Join(" or ", sb.ToArray());

                String sqlQuery = sqlCommand + " where " + whereString;

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);

                var reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        SearchResults searchResults = new SearchResults();

                        searchResults.notes = reader["notes"].ToString();
                        searchResults.id = reader.GetInt32("id");

                        searchResults.grade = reader.GetString("grade");
                        searchResults.notes = reader.GetString("notes");
                        searchResults.ConvertedXODLink = reader.GetString("ConvertedXODLink");
                        searchResults.AnnotationsLink = reader.GetString("AnnotationsLink");
                        searchResults.AnnotedPDFLink = reader.GetString("AnnotedPDFLink");

                    }
                }
                finally
                {
                    reader.Close();

                }

            }

            return result;

        }

    }
}
