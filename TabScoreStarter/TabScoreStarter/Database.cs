// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;

namespace TabScoreStarter
{
    static class Database
    {
        public static string ConnectionString(string pathToDB)
        {
            // Return an Microsoft Access connection string for the database
            OdbcConnectionStringBuilder cs = new OdbcConnectionStringBuilder();
            cs.Driver = "Microsoft Access Driver (*.mdb)";
            cs.Add("Dbq", pathToDB);
            cs.Add("Uid", "Admin");
            cs.Add("Pwd", "");
            return cs.ToString();
        }

        public static void SetAccessControl(string pathToDB)
        {
            // Try to give IIS access permissions to the database file.  This is not necessary if all Users already have access (such as
            // a folder directly off the root (C:\...), but does no harm.  It may fail (silently) if the current User doesn't own the file.
            // But it might be necessary if the database file is in a User-owned folder (C:\Users\[current user]\...). 
            try
            {
                FileInfo fileInfo = new FileInfo(pathToDB);
                FileSecurity fileSecurity = fileInfo.GetAccessControl();
                fileSecurity.AddAccessRule(new FileSystemAccessRule("IIS_IUSRS", FileSystemRights.FullControl, AccessControlType.Allow));
                fileInfo.SetAccessControl(fileSecurity);
            }
            catch {}
            return;
        }

        public static bool Initialize(string connectionString)
        {
            // The initialization checks a number of features in the database to ensure that TabScore will work correctly
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch
                {
                    MessageBox.Show("Database is not accessible; check file and folder permissions", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Check if this is an individual event, when RoundData will have a filled 'South' field
                // and set global value accordingly
                AppData.IsIndividual = true;
                string SQLString = "SELECT TOP 1 South FROM RoundData";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    object queryResult = cmd.ExecuteScalar();
                    if (queryResult == DBNull.Value || queryResult == null || Convert.ToString(queryResult) == "") AppData.IsIndividual = false;
                }
                catch (OdbcException)
                {
                    AppData.IsIndividual = false;
                }

                // SESSION TABLE
                int sessionID = 1;
                // Check if table 'Session' exists
                bool sessionExists = true;
                SQLString = "SELECT TOP 1 ID FROM Session";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    sessionID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count != 1 || e.Errors[0].SQLState != "42S02")
                    {
                        // Error other than 'Session' table doesn't exist
                        throw e;
                    }
                    sessionExists = false;
                }

                if (!sessionExists)
                {
                    // No Session table, so create one and populate it
                    SQLString = "CREATE TABLE Session (ID SHORT, [Name] VARCHAR(40), [Date] DATETIME, [Time] DATETIME, [GUID] VARCHAR(32), [Status] SHORT, ShowInApp YESNO, PairsMoveAcrossField YESNO, EWReturnHome YESNO)";
                    cmd = new OdbcCommand(SQLString, connection);
                    cmd.ExecuteNonQuery();
                    string guidString = Guid.NewGuid().ToString("N").ToUpper();
                    SQLString = $"INSERT INTO Session VALUES (1, 'Bridge game', #{DateTime.Now:yy-MM-dd}#, #{DateTime.Now:hh:mm:ss}#, '{guidString}', 0, False, False, False)";
                    cmd = new OdbcCommand(SQLString, connection);
                    cmd.ExecuteNonQuery();
                }

                // SECTION TABLE
                // Add field 'Session' to table 'Section' if it doesn't already exist
                SQLString = "ALTER TABLE Section ADD Session SHORT";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    sessionExists = false;
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS21")
                    {
                        throw e;
                    }
                }

                if (!sessionExists)
                {
                    // Set field 'Session' in table 'Section' to match session ID 
                    SQLString = $"UPDATE Section SET Session={sessionID}";
                    cmd = new OdbcCommand(SQLString, connection);
                    cmd.ExecuteNonQuery();
                }

                // Add field 'EWMoveBeforePlay' to table 'Section' if it doesn't already exist
                SQLString = "ALTER TABLE Section ADD EWMoveBeforePlay SHORT";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    SQLString = $"UPDATE Section SET EWMoveBeforePlay=0";
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

                // Add field 'ScoringType' to table 'Section' if it doesn't already exist
                SQLString = "ALTER TABLE Section ADD ScoringType SHORT";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    SQLString = $"UPDATE Section SET ScoringType=1";
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

                // Add field 'Winners' to table 'Section' if it doesn't already exist
                SQLString = "ALTER TABLE Section ADD Winners SHORT";
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

                // Read sections
                List<Section> sectionsList = new List<Section>();
                int sectionID;
                SQLString = "SELECT ID, Letter, [Tables], Winners FROM Section";
                cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sectionID = reader.GetInt32(0);
                    string sectionLetter = reader.GetString(1);
                    int numTables = reader.GetInt32(2);
                    int winners = 0;
                    if (!reader.IsDBNull(3))
                    {
                        object tempWinners = reader.GetValue(3);
                        if (tempWinners != null) winners = Convert.ToInt32(tempWinners);
                    }
                    sectionsList.Add(new Section() { ID = sectionID, Letter = sectionLetter, Tables = numTables, Winners = winners });
                }
                reader.Close();

                // Check that a section exists
                if (sectionsList.Count == 0)
                {
                    MessageBox.Show("Database contains no Sections", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                foreach (Section section in sectionsList)
                {
                    // Check section letters, and number of tables per section.  These are TabScore constraints
                    section.Letter = section.Letter.Trim();  // Remove any spurious characters
                    if (section.ID < 1 || section.ID > 4 || (section.Letter != "A" && section.Letter != "B" && section.Letter != "C" && section.Letter != "D"))
                    {
                        MessageBox.Show("Database contains incorrect Sections.  Maximum 4 Sections labelled A, B, C, D", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (section.Tables > 30)
                    {
                        MessageBox.Show("Database contains > 30 Tables in a Section", "TabScoreStarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Ensure that section letter is just the trimmed version
                    SQLString = $"UPDATE Section SET Letter='{section.Letter}' WHERE ID={section.ID}";
                    cmd = new OdbcCommand(SQLString, connection);
                    cmd.ExecuteNonQuery();

                    if (section.Winners == 0)
                    {
                        // Set Winners field based on data from RoundData table.  If the maximum pair number > number of tables + 1, we can assume a one-winner movement.
                        // The +1 is to take account of a rover in a two-winner movement. 

                        SQLString = $"SELECT NSpair, EWpair FROM RoundData WHERE Section={section.ID}";
                        int maxPairNumber = 0;
                        cmd = new OdbcCommand(SQLString, connection);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int newNSpair = reader.GetInt32(0);
                            int newEWpair = reader.GetInt32(1);
                            if (newNSpair > maxPairNumber) maxPairNumber = newNSpair;
                            if (newEWpair > maxPairNumber) maxPairNumber = newEWpair;
                        }
                        reader.Close();
                        if (maxPairNumber == 0)  // No round data for this section! 
                        {
                            section.Winners = 0;
                        }
                        else if (maxPairNumber > section.Tables + 1)
                        {
                            section.Winners = 1;
                        }
                        else
                        {
                            section.Winners = 2;
                        }
                        SQLString = $"UPDATE Section SET Winners={section.Winners} WHERE ID={section.ID}";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                }

                // TABLES TABLE
                // Add field 'Group' to table 'Tables' if it doesn't already exist and set all tables to Group 1
                SQLString = "ALTER TABLE [Tables] ADD [Group] SHORT";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    SQLString = $"UPDATE [Tables] SET [Group]=1";
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

                // RECEIVEDDATA TABLE
                // If this is an individual event, add extra fields South and West to ReceivedData if they don't exist
                if (AppData.IsIndividual)
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

                // Add field 'ExternalUpdate' to table 'ReceivedData' if it doesn't already exist
                SQLString = "ALTER TABLE ReceivedData ADD ExternalUpdate YESNO";
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

                // Add field 'SuspiciousContract' to table 'ReceivedData' if it doesn't already exist
                SQLString = "ALTER TABLE ReceivedData ADD SuspiciousContract SHORT";
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

                // PLAYERNUMBERS TABLE
                // Add field 'Name' to table 'PlayerNumbers' if it doesn't already exist
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

                // Add field 'Processed' to table 'PlayerNumbers' if it doesn't already exist
                SQLString = "ALTER TABLE PlayerNumbers ADD Processed YESNO";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    SQLString = $"UPDATE PlayerNumbers SET Processed=False";
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

                // Add field 'Updated' to table 'PlayerNumbers' if it doesn't already exist
                SQLString = "ALTER TABLE PlayerNumbers ADD Updated YESNO";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    SQLString = $"UPDATE PlayerNumbers SET Updated=False";
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

                // Add field 'TimeLog' to table 'PlayerNumbers' if it doesn't already exist
                SQLString = "ALTER TABLE PlayerNumbers ADD TimeLog DATETIME";
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

                // Add field 'Round' to table 'PlayerNumbers' if it doesn't already exist
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
                SQLString = "UPDATE PlayerNumbers SET [Round]=0 WHERE [Round] IS NULL";
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();

                // Add a new field 'TabScorePairNo' to table 'PlayerNumbers' if it doesn't exist and populate it if possible
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
                    if (AppData.IsIndividual)
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
                    SQLString = $"UPDATE PlayerNumbers SET TabScorePairNo={pairNo} WHERE Section={section} AND [Table]={table} AND Direction='{direction}'";
                    cmd2 = new OdbcCommand(SQLString, connection);
                    cmd2.ExecuteNonQuery();
                }
                cmd2.Dispose();

                // PLAYERNAMES TABLE
                SQLString = "CREATE TABLE PlayerNames (ID LONG, [Name] VARCHAR(40), strID VARCHAR(8))";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count != 1 || e.Errors[0].SQLState != "42S01")  // Error other than PlayerNames table already exists
                    {
                        throw e;
                    }
                }

                // HANDRECORD TABLE
                SQLString = "CREATE TABLE HandRecord (Section SHORT, Board SHORT, NorthSpades VARCHAR(13), NorthHearts VARCHAR(13), NorthDiamonds VARCHAR(13), NorthClubs VARCHAR(13), EastSpades VARCHAR(13), EastHearts VARCHAR(13), EastDiamonds VARCHAR(13), EastClubs VARCHAR(13), SouthSpades VARCHAR(13), SouthHearts VARCHAR(13), SouthDiamonds VARCHAR(13), SouthClubs VARCHAR(13), WestSpades VARCHAR(13), WestHearts VARCHAR(13), WestDiamonds VARCHAR(13), WestClubs VARCHAR(13))";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S01")  // Error other than HandRecord table already exists
                    {
                        throw e;
                    }
                }

                // HANDEVALUATION TABLE
                SQLString = "CREATE TABLE HandEvaluation (Section SHORT, Board SHORT, NorthSpades SHORT, NorthHearts SHORT, NorthDiamonds SHORT, NorthClubs SHORT, NorthNoTrump SHORT, EastSpades SHORT, EastHearts SHORT, EastDiamonds SHORT, EastClubs SHORT, EastNoTrump SHORT, SouthSpades SHORT, SouthHearts SHORT, SouthDiamonds SHORT, SouthClubs SHORT, SouthNotrump SHORT, WestSpades SHORT, WestHearts SHORT, WestDiamonds SHORT, WestClubs SHORT, WestNoTrump SHORT, NorthHcp SHORT, EastHcp SHORT, SouthHcp SHORT, WestHcp SHORT)";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S01")  // Error other than HandEvaluation table already exists
                    {
                        throw e;
                    }
                }

                // SETTINGS TABLE
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
                    SQLString = "UPDATE Settings SET BM2NameSource=0";
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

                SQLString = "ALTER TABLE Settings ADD Section SHORT";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    SQLString = "UPDATE Settings SET Section=0";
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

                SQLString = "ALTER TABLE Settings ADD TabletsMove YESNO";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    if (Properties.Settings.Default.TabletsMove)
                    {
                        SQLString = "UPDATE Settings SET TabletsMove=YES";
                    }
                    else
                    {
                        SQLString = "UPDATE Settings SET TabletsMove=NO";
                    }
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

                SQLString = "ALTER TABLE Settings ADD HandRecordReversePerspective YESNO";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    if (Properties.Settings.Default.HandRecordReversePerspective)
                    {
                        SQLString = "UPDATE Settings SET HandRecordReversePerspective=YES";
                    }
                    else
                    {
                        SQLString = "UPDATE Settings SET HandRecordReversePerspective=NO";
                    }
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

                SQLString = "ALTER TABLE Settings DROP ShowTimer";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS22")
                    {
                        throw e;
                    }
                }
                SQLString = "ALTER TABLE Settings ADD ShowTimer YESNO";
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();
                if (Properties.Settings.Default.ShowTimer)
                {
                    SQLString = "UPDATE Settings SET ShowTimer=YES";
                }
                else
                {
                    SQLString = "UPDATE Settings SET ShowTimer=NO";
                }
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();

                SQLString = "ALTER TABLE Settings DROP SecondsPerBoard";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS22")
                    {
                        throw e;
                    }
                }
                SQLString = "ALTER TABLE Settings ADD SecondsPerBoard SHORT";
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();
                SQLString = "UPDATE Settings SET SecondsPerBoard=" + Properties.Settings.Default.SecondsPerBoard.ToString();
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();

                SQLString = "ALTER TABLE Settings DROP AdditionalSecondsPerRound";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS22")
                    {
                        throw e;
                    }
                }
                SQLString = "ALTER TABLE Settings ADD AdditionalSecondsPerRound SHORT";
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();
                SQLString = "UPDATE Settings SET AdditionalSecondsPerRound=" + Properties.Settings.Default.AdditionalSecondsPerRound.ToString();
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();

                // Remove unwanted old settings if they're there
                SQLString = "ALTER TABLE Settings DROP MinutesPerBoard";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS22")
                    {
                        throw e;
                    }
                }

                SQLString = "ALTER TABLE Settings DROP AdditionalMinutesPerRound";
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count != 1 || e.Errors[0].SQLState != "HYS22")
                    {
                        throw e;
                    }
                }
            }
            return true;
        }
    }
}