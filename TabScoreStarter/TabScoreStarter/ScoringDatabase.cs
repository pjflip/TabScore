using System;
using System.Data.Odbc;
using System.Windows.Forms;

namespace TabScoreStarter
{
    public static class ScoringDatabase
    {
        public static string SetDBConnectionString(string pathToDB)
        {
            OdbcConnectionStringBuilder cs = new OdbcConnectionStringBuilder();
            cs.Driver = "Microsoft Access Driver (*.mdb)";
            cs.Add("Dbq", pathToDB);
            cs.Add("Uid", "Admin");
            cs.Add("Pwd", "");
            return cs.ToString();
        }

        public static bool InitializeDB(string DB)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                try
                {
                    connection.Open();

                    // Check sections
                    int sectionID = 0;
                    string SQLString = "SELECT ID, Letter, [Tables], Winners FROM Section";
                    OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                    OdbcDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        sectionID = reader.GetInt32(0);
                        string sectionLetter = reader.GetString(1);
                        int numTables = reader.GetInt32(2);
                        if (sectionID < 1 || sectionID > 4 || (sectionLetter != "A" && sectionLetter != "B" && sectionLetter != "C" && sectionLetter != "D"))
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
                    if (sectionID == 0)
                    {
                        MessageBox.Show("Database contains no Sections", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Add column 'Name' to table 'PlayerNumbers' if it doesn't already exist
                    SQLString = "ALTER TABLE PlayerNumbers ADD [Name] VARCHAR(30)";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }

                    // Add column 'Round' to table 'PlayerNumbers' if it doesn't already exist
                    SQLString = "ALTER TABLE PlayerNumbers ADD [Round] SHORT";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    
                    // Ensure that all Round values are set to 0 to start with
                    SQLString = "UPDATE PlayerNumbers SET [Round]=0";
                    cmd = new OdbcCommand(SQLString, connection);
                    cmd.ExecuteNonQuery();

                    // Check if this is an individual event, when RoundData will have a 'South' column
                    bool individualEvent = true;
                    SQLString = $"SELECT TOP 1 South FROM RoundData";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteScalar();
                    }
                    catch (OdbcException)
                    {
                        individualEvent = false;
                    }

                    // If this is an individual event, add extra columns South and West to ReceivedData if they dont exist
                    if (individualEvent)
                    {
                        SQLString = "ALTER TABLE ReceivedData ADD South SHORT";
                        cmd = new OdbcCommand(SQLString, connection);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (OdbcException e)
                        {
                            if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                            {
                                throw e;
                            }
                        }
                        SQLString = "ALTER TABLE ReceivedData ADD West SHORT";
                        cmd = new OdbcCommand(SQLString, connection);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (OdbcException e)
                        {
                            if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                            {
                                throw e;
                            }
                        }
                    }

                    // Add a new column 'TabScorePairNo' to table 'PlayerNumbers' if it doesn't exist and populate it if possible
                    SQLString = "ALTER TABLE PlayerNumbers ADD TabScorePairNo SHORT";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "SELECT Section, [Table], Direction FROM PlayerNumbers";
                    cmd = new OdbcCommand(SQLString, connection);
                    reader = cmd.ExecuteReader();
                    OdbcCommand cmd2 = new OdbcCommand();
                    while (reader.Read())
                    {
                        int section = reader.GetInt32(0);
                        int table = reader.GetInt32(1);
                        string direction = reader.GetString(2);
                        if (individualEvent)
                        {
                            switch (direction)
                            {
                                case "N":
                                    SQLString = $"SELECT NSPair FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                                case "S":
                                    SQLString = $"SELECT South FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                                case "E":
                                    SQLString = $"SELECT EWPair FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                                case "W":
                                    SQLString = $"SELECT West FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                            }
                        }
                        else
                        {
                            switch (direction)
                            {
                                case "N":
                                case "S":
                                    SQLString = $"SELECT NSPair FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                                case "E":
                                case "W":
                                    SQLString = $"SELECT EWPair FROM RoundData WHERE Section={section} AND [Table]={table} AND ROUND=1";
                                    break;
                            }
                        }
                        cmd2 = new OdbcCommand(SQLString, connection);
                        object queryResult = cmd2.ExecuteScalar();
                        string pairNo = queryResult.ToString();
                        SQLString = $"UPDATE PlayerNumbers SET TabScorePairNo={pairNo} WHERE Section={section.ToString()} AND [Table]={table.ToString()} AND Direction='{direction}'";
                        cmd2 = new OdbcCommand(SQLString, connection);
                        cmd2.ExecuteNonQuery();
                    }
                    cmd2.Dispose();

                    // Add various columns to table 'Settings' if they don't already exist and set defaults
                    SQLString = "ALTER TABLE Settings ADD ShowResults YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET ShowResults=YES";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD ShowPercentage YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET ShowPercentage=YES";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD LeadCard YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET LeadCard=YES";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD BM2ValidateLeadCard YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET BM2ValidateLeadCard=YES";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD BM2NumberEntryEachRound YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET BM2NumberEntryEachRound=NO";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD BM2ViewHandRecord YESNO";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET BM2ViewHandRecord=YES";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD BM2Ranking SHORT";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET BM2Ranking=1";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }
                    SQLString = "ALTER TABLE Settings ADD BM2NameSource SHORT";
                    cmd = new OdbcCommand(SQLString, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        SQLString = "UPDATE Settings SET ShowResults=0";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                        {
                            throw e;
                        }
                    }

                    // Check if any previous results in database
                    object Result;
                    SQLString = "SELECT * FROM ReceivedData";
                    cmd = new OdbcCommand(SQLString, connection);
                    Result = cmd.ExecuteScalar();
                    if (Result != null)
                    {
                        MessageBox.Show("Database contains previous results", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
