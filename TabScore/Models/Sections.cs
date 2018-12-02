using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public static class Sections
    {
        public static List<SectionClass> GetSections(string DB)
        {
            List<SectionClass> sList = new List<SectionClass>();

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                SectionClass s = new SectionClass();

                string SQLString = "SELECT ID, Letter, Tables, MissingPair, Winners FROM Section";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    OdbcDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        s.ID = reader.GetInt32(0);
                        s.Letter = reader.GetString(1);
                        s.Tables = reader.GetInt32(2);
                        s.MissingPair = reader.GetInt32(3);
                        s.Winners = reader.GetInt32(4);
                        sList.Add(s);
                    }
                    reader.Close();
                }
                catch  
                {
                    // Assume error is no 'Winners'
                    SQLString = "SELECT ID, Letter, Tables, MissingPair FROM Section";
                    cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        s.ID = reader.GetInt32(0);
                        s.Letter = reader.GetString(1);
                        s.Tables = reader.GetInt32(2);
                        s.MissingPair = reader.GetInt32(3);
                        SQLString = "SELECT EWPair FROM RoundData WHERE Section=" + s.ID + " AND Round=1 AND NSPair=1";
                        cmd = new OdbcCommand(SQLString, connection);
                        object queryResult = cmd.ExecuteScalar();
                        if (queryResult == null || queryResult.ToString() != "1")
                        {
                            s.Winners = 1;   // NSPair!=EWPair so assuming one winner movement
                        }
                        else
                        {
                            s.Winners = 2;   // NSPair==EWPair so assuming Mitchell type movement with 2 winners
                        }
                        sList.Add(s);
                    }
                    reader.Close();
                }
                cmd.Dispose();
            }
            return sList;
        }
    }
}