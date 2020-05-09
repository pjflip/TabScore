// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class SectionsList : List<Section>
    {
        public SectionsList()
        {
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
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