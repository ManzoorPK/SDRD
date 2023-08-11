using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySqlConnector;
using System.Configuration;

namespace Search.App.Helpers
{
    public class DatabaseHelper
    {
        public string SQLConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
        public DataTable FillData(string Qry)
        {
            DataTable dt = new DataTable();
            MySqlConnector.MySqlConnection connection = new MySqlConnector.MySqlConnection(SQLConnectionString);
            try
            {
                MySqlCommand CMD = new MySqlCommand(SQLConnectionString, connection);
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
    }
}
