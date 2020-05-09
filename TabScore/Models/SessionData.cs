// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;

namespace TabScore.Models
{
    public class SessionData  // A class for bridge session data
    {
        public int TableNumber { get; set; }
        public int SectionID { get; set; }
        public string SectionLetter { get; set; }
        public string SectionTableString { get; set; }
        public int NumTables { get; set; }
        public int MissingPair { get; set; }

        public int TableLogonStatus()
        {
            object queryResult = null;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT LogOnOff FROM Tables WHERE Section={SectionID} AND [Table]={TableNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd.ExecuteScalar();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return Convert.ToInt32(queryResult);
        }

        public void LogonTable()
        {
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"UPDATE Tables SET LogOnOff=1 WHERE Section={SectionID} AND [Table]={TableNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }
    }
}