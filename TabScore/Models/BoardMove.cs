using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class BoardMove
    {
        public int NewTableNumber { get; private set; }

        public BoardMove(SessionData sessionData, int newRoundNumber, int lowBoard)
        {
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                // Get a list of all possible tables to which boards could move
                List<int> tableList = new List<int>();
                string SQLString = $"SELECT [Table] FROM RoundData WHERE Section={sessionData.SectionID} AND Round={newRoundNumber} AND LowBoard={lowBoard}";
                connection.Open();
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int t = reader.GetInt32(0);
                            tableList.Add(t);
                        }
                    });
                }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                }

                if (tableList.Count == 0)
                {
                    // No table, so move to relay table
                    NewTableNumber = 0;
                }
                else if (tableList.Count == 1)
                {
                    // Just one table, so use it
                    NewTableNumber = tableList[0];
                }
                else
                {
                    // Find the next table down to which the boards could move
                    for (int t = sessionData.TableNumber; t > 0; t--)
                    {
                        if (tableList.Contains(t))
                        {
                            NewTableNumber = t;
                            return;
                        }
                    }
                    NewTableNumber = 0;
                    foreach (int t in tableList)  // Next table down must be highest table number in the list
                    {
                        if (t > NewTableNumber) NewTableNumber = t;
                    }
                }
            }
        }
    }
}