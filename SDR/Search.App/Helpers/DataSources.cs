using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.App.Helpers
{
    public class DataSources
    {
        public DataTable GetSubjects()
        {
            var dt = new DatabaseHelper().FillData("SELECT * FROM applicationinfo WHERE tablename ='subject' AND columnname = 'name'");

            string Qry = @"SELECT Id,
                           CAST(AES_DECRYPT(name,'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR)
                           AS Subject
                           FROM subject";

            if (dt.Rows.Count == 0)
                Qry = @"SELECT Id,
                        name
                        AS Subject
                        FROM subject";

            return new DatabaseHelper().FillData(Qry);
        }

        public DataTable GetProfessors()
        {
            var dt = new DatabaseHelper().FillData("SELECT * FROM applicationinfo WHERE tablename ='user' AND columnname = 'name'");

            string Qry = @"SELECT Id,
                           CAST(AES_DECRYPT(name,'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR)
                           AS name
                           FROM user WHERE role = 1";

            if (dt.Rows.Count == 0)
                Qry = @"SELECT Id,
                        name
                        FROM user WHERE role = 1";

            return new DatabaseHelper().FillData(Qry);
        }
        public DataTable GetSemesters()
        {
            var dt = new DatabaseHelper().FillData("SELECT * FROM applicationinfo WHERE tablename ='examination' AND columnname = 'semester'");

            string Qry = @"SELECT DISTINCT
                           CAST(AES_DECRYPT(semester,'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR)
                           AS name
                           FROM examination";

            if (dt.Rows.Count == 0)
                Qry = @"SELECT
                        DISTINCT semester as name
                        FROM examination";

            return new DatabaseHelper().FillData(Qry);
        }

        public DataTable GetYears()
        {
            var dt = new DatabaseHelper().FillData("SELECT * FROM applicationinfo WHERE tablename ='examination' AND columnname = 'year'");

            string Qry = @"SELECT DISTINCT
                           CAST(AES_DECRYPT(year,'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR)
                           AS name
                           FROM examination";

            if (dt.Rows.Count == 0)
                Qry = @"SELECT
                        DISTINCT year as name
                        FROM examination";

            return new DatabaseHelper().FillData(Qry);
        }

        public DataTable GetStatus()
        {
            var dt = new DatabaseHelper().FillData("SELECT * FROM applicationinfo WHERE tablename ='examination' AND columnname = 'status'");

            string Qry = @"SELECT DISTINCT
                           CAST(AES_DECRYPT(status,'D6E2D35C80AA466C9C23A0120874B4B2') AS CHAR)
                           AS name
                           FROM examination";

            if (dt.Rows.Count == 0)
                Qry = @"SELECT
                        DISTINCT status as name
                        FROM examination";

            return new DatabaseHelper().FillData(Qry);
        }
    }
}
