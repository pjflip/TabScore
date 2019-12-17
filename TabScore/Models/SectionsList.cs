using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class SectionsList : List<Section>
    {
        public SectionsList(string DB)
        {
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
                            Section s = new Section
                            {
                                ID = reader.GetInt32(0),
                                Letter = reader.GetString(1),
                                NumTables = reader.GetInt32(2),
                                MissingPair = reader.GetInt32(3)
                            };
                            Add(s);
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }
            }
        }
    }
}