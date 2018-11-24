using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;

namespace TabScoreStarter
{
    class HandsList
    {
        public List<Hand> Hands = new List<Hand>();

        public void ReadFromPBNFile(string pathToFile)
        {
            bool newBoard = false;
            string line = null;
            char[] quoteDelimiter = { '"' };

            Hands.Clear();

            StreamReader file = new StreamReader(pathToFile);
            if (!file.EndOfStream)
            {
                line = file.ReadLine();
                newBoard = line.Length > 7 && line.Substring(0, 7) == "[Board ";
            }
            while (!file.EndOfStream)
            {
                if (newBoard)
                {
                    newBoard = false;
                    Hand hand = new Hand()
                    {
                        SectionID = 1,         // Default SectionID=1 if hands apply to more than one section
                        NorthSpades = "###"
                    };
                    hand.Board = Convert.ToInt32(line.Split(quoteDelimiter)[1]);
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Length > 6 && line.Substring(0, 6) == "[Deal ")
                        {
                            hand.PBN = line.Split(quoteDelimiter)[1];
                        }
                        else if (line.Length > 7 && line.Substring(0, 7) == "[Board ")
                        {
                            newBoard = true;
                            if (hand.NorthSpades != "###") Hands.Add(hand);
                            break;
                        }
                    }
                    if (file.EndOfStream)
                    {
                        if (hand.NorthSpades != "###") Hands.Add(hand);
                    }
                }
                else if (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    newBoard = line.Length > 7 && line.Substring(0, 7) == "[Board ";
                }
            }
            file.Close();
        }

        public void ReadFromDB(string DB)
        {
            Hands.Clear();
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString = $"SELECT Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, EastSpades, EastHearts, EastDiamonds, EastClubs, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, WestSpades, WestHearts, WestDiamonds, WestClubs FROM HandRecord";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                try
                {
                    OdbcDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Hand hr = new Hand();
                        hr.SectionID = reader.GetInt32(0);
                        hr.Board = reader.GetInt32(1);
                        hr.NorthSpades = reader.GetString(2);
                        hr.NorthHearts = reader.GetString(3);
                        hr.NorthDiamonds = reader.GetString(4);
                        hr.NorthClubs = reader.GetString(5);
                        hr.EastSpades = reader.GetString(6);
                        hr.EastHearts = reader.GetString(7);
                        hr.EastDiamonds = reader.GetString(8);
                        hr.EastClubs = reader.GetString(9);
                        hr.SouthSpades = reader.GetString(10);
                        hr.SouthHearts = reader.GetString(11);
                        hr.SouthDiamonds = reader.GetString(12);
                        hr.SouthClubs = reader.GetString(13);
                        hr.WestSpades = reader.GetString(14);
                        hr.WestHearts = reader.GetString(15);
                        hr.WestDiamonds = reader.GetString(16);
                        hr.WestClubs = reader.GetString(17);
                        Hands.Add(hr);
                    }
                    reader.Close();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count == 1 && e.Errors[0].SQLState != "42S02")  // HandRecord table does not exist
                    {
                        cmd.Dispose();
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        public void WriteToDB(string DB)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = "DELETE FROM HandRecord";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();

                foreach (Hand hc in Hands)
                {
                    if (hc.NorthSpades != "###")
                    {
                        SQLString = $"INSERT INTO HandRecord (Section, Board, NorthSpades, NorthHearts, NorthDiamonds, NorthClubs, EastSpades, EastHearts, EastDiamonds, EastClubs, SouthSpades, SouthHearts, SouthDiamonds, SouthClubs, WestSpades, WestHearts, WestDiamonds, WestClubs) VALUES ({hc.SectionID}, {hc.Board}, '{hc.NorthSpades}', '{hc.NorthHearts}', '{hc.NorthDiamonds}', '{hc.NorthClubs}', '{hc.EastSpades}', '{hc.EastHearts}', '{hc.EastDiamonds}', '{hc.EastClubs}', '{hc.SouthSpades}', '{hc.SouthHearts}', '{hc.SouthDiamonds}', '{hc.SouthClubs}', '{hc.WestSpades}', '{hc.WestHearts}', '{hc.WestDiamonds}', '{hc.WestClubs}')";
                        cmd = new OdbcCommand(SQLString, connection);
                        cmd.ExecuteNonQuery();
                    }
                }
                cmd.Dispose();
            }
        }
    }
}
