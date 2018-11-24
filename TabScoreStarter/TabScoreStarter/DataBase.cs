using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Windows.Forms;

namespace TabScoreStarter
{
    public static class DataBase
    {
        public static bool ConnectionOK(string DB)
        {
            using (OdbcConnection connnection = new OdbcConnection(DB))
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
                            MessageBox.Show("Database countains incorrect Sections.  Maximum 4 Sections labelled A, B, C, D", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        if (numTables > 30)
                        {
                            reader.Close();
                            MessageBox.Show("Database countains > 30 Tables in a Section", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public static bool InitializeHandRecords(string DB)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
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
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S01")  // HandRecord table already exists
                    {
                        SQLString = "SELECT 1 FROM HandRecord";
                        cmd = new OdbcCommand(SQLString, connection);
                        object queryResult = cmd.ExecuteScalar();
                        cmd.Dispose();
                        if (queryResult == null)
                        {
                            return true;    // But it contains no records
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        throw e;
                    }
                }
                cmd.Dispose();
                return true;
            }
        }

        public static bool InitializeHandEvaluations(string DB)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = "CREATE TABLE HandEvaluation (Section SHORT, Board SHORT, NorthSpades SHORT, NorthHearts SHORT, NorthDiamonds SHORT, NorthClubs SHORT, NorthNoTrump SHORT, EastSpades SHORT, EastHearts SHORT, EastDiamonds SHORT, EastClubs SHORT, EastNoTrump SHORT, SouthSpades SHORT, SouthHearts SHORT, SouthDiamonds SHORT, SouthClubs SHORT, SouthNotrump SHORT, WestSpades SHORT, WestHearts SHORT, WestDiamonds SHORT, WestClubs SHORT, WestNoTrump SHORT, NorthHcp SHORT, EastHcp SHORT, SouthHcp SHORT, WestHcp SHORT)";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState == "42S01")  // HandEvaluation table already exists
                    {
                        SQLString = "SELECT 1 FROM HandEvaluation";
                        cmd = new OdbcCommand(SQLString, connection);
                        object queryResult = cmd.ExecuteScalar();
                        cmd.Dispose();
                        if (queryResult == null)
                        {
                            return true;    // But it contains no records
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        throw e;
                    }
                }
                cmd.Dispose();
                return true;
            }
        }
    }
}
