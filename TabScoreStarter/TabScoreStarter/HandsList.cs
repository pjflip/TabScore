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
                        hr.NorthSpades = reader.GetValue(2).ToString();
                        hr.NorthHearts = reader.GetValue(3).ToString();
                        hr.NorthDiamonds = reader.GetValue(4).ToString();
                        hr.NorthClubs = reader.GetValue(5).ToString();
                        hr.EastSpades = reader.GetValue(6).ToString();
                        hr.EastHearts = reader.GetValue(7).ToString();
                        hr.EastDiamonds = reader.GetValue(8).ToString();
                        hr.EastClubs = reader.GetValue(9).ToString();
                        hr.SouthSpades = reader.GetValue(10).ToString();
                        hr.SouthHearts = reader.GetValue(11).ToString();
                        hr.SouthDiamonds = reader.GetValue(12).ToString();
                        hr.SouthClubs = reader.GetValue(13).ToString();
                        hr.WestSpades = reader.GetValue(14).ToString();
                        hr.WestHearts = reader.GetValue(15).ToString();
                        hr.WestDiamonds = reader.GetValue(16).ToString();
                        hr.WestClubs = reader.GetValue(17).ToString();
                        Hands.Add(hr);
                    }
                    reader.Close();
                }
                catch (OdbcException e)
                {
                    if (e.Errors.Count > 1 || e.Errors[0].SQLState != "42S02")  // Error other than HandRecord table does not exist
                    {
                        throw e;
                    }
                }
                cmd.Dispose();
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
