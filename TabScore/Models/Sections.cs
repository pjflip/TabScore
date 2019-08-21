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
                connection.Open();
                string SQLString = "SELECT ID, Letter, Tables, MissingPair FROM Section";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            SectionClass s = new SectionClass
                            {
                                ID = reader.GetInt32(0),
                                Letter = reader.GetString(1),
                                Tables = reader.GetInt32(2),
                                MissingPair = reader.GetInt32(3)
                            };
                            sList.Add(s);
                        }
                    });
                }
                catch (OdbcException)
                {
                    return null;
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            }
            return sList;
        }
    }
}