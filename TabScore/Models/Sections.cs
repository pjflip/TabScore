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

                string SQLString = "SELECT ID, Letter, Tables, MissingPair FROM Section";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                OdbcDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    s.ID = reader.GetInt32(0);
                    s.Letter = reader.GetString(1);
                    s.Tables = reader.GetInt32(2);
                    s.MissingPair = reader.GetInt32(3);
                    sList.Add(s);
                }
                reader.Close();
                cmd.Dispose();
            }
            return sList;
        }
    }
}