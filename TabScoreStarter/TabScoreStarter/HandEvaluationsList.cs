// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Collections.Generic;
using System.Data.Odbc;

namespace TabScoreStarter
{
    class HandEvaluationsList : List<HandEvaluation>
    {
        public void WriteToDB(string connectionString)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();
                string SQLString = "DELETE FROM HandEvaluation";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();

                foreach (HandEvaluation hev in this)
                {
                    if (hev.NorthSpades != -1)
                    {
                        SQLString = $"INSERT INTO HandEvaluation (Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, NorthNotrump, EastSpades, EastHearts, EastDiamonds, EastClubs, EastNotrump, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, SouthNotrump, WestSpades, WestHearts, WestDiamonds, WestClubs, WestNotrump, NorthHcp, EastHcp, SouthHcp, WestHcp) VALUES ({hev.SectionID}, {hev.BoardNumber}, '{hev.NorthSpades}', '{hev.NorthHearts}', '{hev.NorthDiamonds}', '{hev.NorthClubs}', '{hev.NorthNotrump}', '{hev.EastSpades}', '{hev.EastHearts}', '{hev.EastDiamonds}', '{hev.EastClubs}', '{hev.EastNotrump}', '{hev.SouthSpades}', '{hev.SouthHearts}', '{hev.SouthDiamonds}', '{hev.SouthClubs}', '{hev.SouthNotrump}', '{hev.WestSpades}', '{hev.WestHearts}', '{hev.WestDiamonds}', '{hev.WestClubs}', '{hev.WestNotrump}', '{hev.NorthHcp}', '{hev.EastHcp}', '{hev.SouthHcp}', '{hev.WestHcp}')";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                }
                cmd.Dispose();
            }
        }
    }
}
