using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Windows.Forms;

namespace TabScoreStarter
{
    public static class DB
    {
        public static bool TestDBConnection(string pathToDB)
        {
            if (pathToDB == "")
            {
                return false;
            }
            else
            {
                OdbcConnectionStringBuilder cs = new OdbcConnectionStringBuilder();
                cs.Driver = "Microsoft Access Driver (*.mdb)";
                cs.Add("Dbq", pathToDB);
                cs.Add("Uid", "Admin");
                using (OdbcConnection connnection = new OdbcConnection(cs.ToString()))
                {
                    int SectionID = 0;
                    string Section;
                    int numTables;
                    try
                    {
                        string SQLString = "SELECT ID, Letter, [Tables] FROM Section";
                        OdbcCommand cmd = new OdbcCommand(SQLString, connnection);
                        connnection.Open();
                        OdbcDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            SectionID = reader.GetInt32(0);
                            Section = reader.GetString(1);
                            numTables = reader.GetInt32(2);
                            if (SectionID < 1 || SectionID > 4 || (Section != "A" && Section != "B" && Section != "C" && Section != "D"))
                            {
                                reader.Close();
                                MessageBox.Show("Database countains > 4 Sections", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            if (numTables > 20)
                            {
                                reader.Close();
                                MessageBox.Show("Database countains > 20 Tables", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        reader.Close();
                        if (SectionID == 0)
                        {
                            MessageBox.Show("Database contains no Sections", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        object Result;
                        cmd = new OdbcCommand("SELECT * FROM ReceivedData", connnection);
                        Result = cmd.ExecuteScalar();
                        if (Result != null)
                        {
                            MessageBox.Show("Database contains previous results", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                return true;
            }
        }

        public static void WriteHandsToDB(string pathToDB, List<HandClass> handList)
        {
            OdbcConnectionStringBuilder cs = new OdbcConnectionStringBuilder();
            cs.Driver = "Microsoft Access Driver (*.mdb)";
            cs.Add("Dbq", pathToDB);
            cs.Add("Uid", "Admin");
            using (OdbcConnection connection = new OdbcConnection(cs.ToString()))
            {
                connection.Open();
                string SQLString = "CREATE TABLE HandRecord (Section SHORT, Board SHORT, NorthSpades VARCHAR(13), NorthHearts VARCHAR(13), NorthDiamonds VARCHAR(13), NorthClubs VARCHAR(13), EastSpades VARCHAR(13), EastHearts VARCHAR(13), EastDiamonds VARCHAR(13), EastClubs VARCHAR(13), SouthSpades VARCHAR(13), SouthHearts VARCHAR(13), SouthDiamonds VARCHAR(13), SouthClubs VARCHAR(13), WestSpades VARCHAR(13), WestHearts VARCHAR(13), WestDiamonds VARCHAR(13), WestClubs VARCHAR(13))";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S01")  // HandRecord table exists
                    {
                        SQLString = "DELETE FROM HandRecord";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw e;
                    }
                }
                foreach (HandClass hc in handList)
                {
                    if (hc.NorthSpades != "###")
                    {
                        SQLString = $"INSERT INTO HandRecord (Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, EastSpades, EastHearts, EastDiamonds, EastClubs, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, WestSpades, WestHearts, WestDiamonds, WestClubs) VALUES (1, {hc.Board}, '{hc.NorthSpades}', '{hc.NorthHearts}', '{hc.NorthDiamonds}', '{hc.NorthClubs}', '{hc.EastSpades}', '{hc.EastHearts}', '{hc.EastDiamonds}', '{hc.EastClubs}', '{hc.SouthSpades}', '{hc.SouthHearts}', '{hc.SouthDiamonds}', '{hc.SouthClubs}', '{hc.WestSpades}', '{hc.WestHearts}', '{hc.WestDiamonds}', '{hc.WestClubs}')";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                }

                SQLString = "CREATE TABLE HandEvaluation (Section SHORT, Board SHORT, NorthSpades SHORT, NorthHearts SHORT, NorthDiamonds SHORT, NorthClubs SHORT, NorthNoTrump SHORT, EastSpades SHORT, EastHearts SHORT, EastDiamonds SHORT, EastClubs SHORT, EastNoTrump SHORT, SouthSpades SHORT, SouthHearts SHORT, SouthDiamonds SHORT, SouthClubs SHORT, SouthNotrump SHORT, WestSpades SHORT, WestHearts SHORT, WestDiamonds SHORT, WestClubs SHORT, WestNoTrump SHORT, NorthHcp SHORT, EastHcp SHORT, SouthHcp SHORT, WestHcp SHORT)";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S01")  // HandEvaluation table exists
                    {
                        SQLString = "DELETE FROM HandEvaluation";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw e;
                    }
                }
                foreach (HandClass hc in handList)
                {
                    if (hc.EvalNorthNT != -1)
                    {
                        SQLString = $"INSERT INTO HandEvaluation (Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, NorthNotrump, EastSpades, EastHearts, EastDiamonds, EastClubs, EastNotrump, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, SouthNotrump, WestSpades, WestHearts, WestDiamonds, WestClubs, WestNotrump, NorthHcp, EastHcp, SouthHcp, WestHcp) VALUES (1, {hc.Board}, '{hc.EvalNorthSpades}', '{hc.EvalNorthHearts}', '{hc.EvalNorthDiamonds}', '{hc.EvalNorthClubs}', '{hc.EvalNorthNT}', '{hc.EvalEastSpades}', '{hc.EvalEastHearts}', '{hc.EvalEastDiamonds}', '{hc.EvalEastClubs}', '{hc.EvalEastNT}', '{hc.EvalSouthSpades}', '{hc.EvalSouthHearts}', '{hc.EvalSouthDiamonds}', '{hc.EvalSouthClubs}', '{hc.EvalSouthNT}', '{hc.EvalWestSpades}', '{hc.EvalWestHearts}', '{hc.EvalWestDiamonds}', '{hc.EvalWestClubs}', '{hc.EvalWestNT}', '{hc.HCPNorth}', '{hc.HCPEast}', '{hc.HCPSouth}', '{hc.HCPWest}')";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                }
                cmd.Dispose();
            }
        }
    }
}