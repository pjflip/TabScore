// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Data.Odbc;
using System.Text;
using Resources;

namespace TabScore.Models
{
    public class Result
    {
        public int BoardNumber { get; set; }
        public int NumberNorth { get; set; }
        public int NumberEast { get; set; }
        public int NumberSouth { get; set; } = 0;
        public int NumberWest { get; set; } = 0;
        public string DeclarerNSEW { get; set; }
        public int ContractLevel { get; set; }
        public string ContractSuit { get; set; }
        public string ContractX { get; set; }
        public string Remarks { get; set; }
        public LeadValidationOptions LeadValidation { get; set; }
        public int Score { get; private set; }
        public double MatchpointsNS { get; set; }
        public double MatchpointsEW { get; set; }

        // Default constructor for lists
        public Result() { }

        // Database read constructor
        public Result(TableStatus tableStatus, int boardNumber)
        {
            NumberNorth = tableStatus.RoundData.NumberNorth;
            NumberSouth = tableStatus.RoundData.NumberSouth;
            NumberEast = tableStatus.RoundData.NumberEast;
            NumberWest = tableStatus.RoundData.NumberWest;
            BoardNumber = boardNumber;
            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                connection.Open();
                string SQLString = $"SELECT [NS/EW], Contract, Result, LeadCard, Remarks FROM ReceivedData WHERE Section={tableStatus.SectionID} AND [Table]={tableStatus.TableNumber} AND Round={tableStatus.RoundNumber} AND Board={BoardNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Remarks = reader.GetString(4);
                            string tempContract = reader.GetString(1);
                            if ((Remarks == "" || Remarks == "Wrong direction") && tempContract.Length > 2)
                            {
                                Contract = tempContract;   // Sets ContractLevel, etc
                                if (ContractLevel == 0)  // Passed out
                                {
                                    DeclarerNSEW = "";
                                    TricksTakenNumber = -1;
                                    LeadCardDB = "";
                                }
                                else
                                {
                                    DeclarerNSEW = reader.GetString(0);
                                    TricksTakenSymbol = reader.GetString(2);
                                    LeadCardDB = reader.GetString(3);
                                }
                            }
                            else
                            {
                                ContractLevel = -1;
                                ContractSuit = "";
                                ContractX = "";
                                DeclarerNSEW = "";
                                TricksTakenNumber = -1;
                                LeadCardDB = "";
                            }
                        }
                        else  // No result in database
                        {
                            ContractLevel = -999;
                            ContractSuit = "";
                            ContractX = "";
                            DeclarerNSEW = "";
                            TricksTakenNumber = -1;
                            LeadCardDB = "";
                            Remarks = "";
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

        // CONTRACT
        public string Contract
        {
            get
            {
                if (ContractLevel < 0)  // No result or board not played
                {
                    return "";
                }
                else if (ContractLevel == 0)
                {
                    return "PASS";
                }
                else
                {
                    string contract = $"{ContractLevel} {ContractSuit}";
                    if (ContractX != "")
                    {
                        contract = $"{contract} {ContractX}";
                    }
                    return contract;
                }
            }
            set
            {
                if (value.Length <= 2)  // Board not played or corrupt data
                {
                    ContractLevel = -999;
                }
                else if (value == "PASS")
                {
                    ContractLevel = 0;
                    ContractSuit = "";
                    ContractX = "";
                }
                else  // Contract (hopefully) contains a valid contract
                {
                    string[] temp = value.Split(' ');
                    ContractLevel = Convert.ToInt32(temp[0]);
                    ContractSuit = temp[1];
                    if (temp.Length > 2) ContractX = temp[2];
                    else ContractX = "";
                }
            }
        }
        public string ContractDisplay
        {
            get
            {
                if (ContractLevel == -999)  // No result entry yet
                {
                    return "";
                }
                else if (ContractLevel == -1)  // Board not played or arbitral result of some kind
                {
                    if (Remarks == "Not played")
                    {
                        return $"<span style=\"color:red\">{Strings.NotPlayed}</span>";
                    }
                    else if (Remarks == "Arbitral score")
                    {
                        return $"<span style=\"color:red\">{Strings.ArbitralScore}</span>";
                    }
                    else
                    {
                        return $"<span style=\"color:red\">{Remarks}</span>";
                    }
                }
                else if (ContractLevel == 0)
                {
                    return $"<span style=\"color:darkgreen\">{Strings.AllPass}</span>";
                }

                //Normal contract and result
                StringBuilder s = new StringBuilder("");
                s.Append(ContractLevel);
                switch (ContractSuit)
                {
                    case "NT":
                        s.Append(Strings.NT);
                        break;
                    case "S":
                        s.Append("<span style=\"color:black\">&spades;</span>");
                        break;
                    case "H":
                        s.Append("<span style=\"color:red\">&hearts;</span>");
                        break;
                    case "D":
                        s.Append("<span style=\"color:lightsalmon\">&diams;</span>");
                        break;
                    case "C":
                        s.Append("<span style=\"color:lightslategrey\">&clubs;</span>");
                        break;
                }
                s.Append(ContractX);
                s.Append($"{TricksTakenSymbol} {Strings.by} ");
                s.Append(DeclarerNSEWDisplay);
                return s.ToString();
            }
        }
        public string ContractTravellerDisplay
        {
            get
            {
                if (ContractLevel < 0) return "";
                if (ContractLevel == 0) return $"<span style=\"color:darkgreen\">{Strings.Pass}</span>";
                StringBuilder s = new StringBuilder(ContractLevel.ToString());
                switch (ContractSuit)
                {
                    case "S":
                        s.Append("<span style=\"color:black\">&spades;</span>");
                        break;
                    case "H":
                        s.Append("<span style=\"color:red\">&hearts;</span>");
                        break;
                    case "D":
                        s.Append("<span style=\"color:lightsalmon\">&diams;</span>");
                        break;
                    case "C":
                        s.Append("<span style=\"color:lightslategrey\">&clubs;</span>");
                        break;
                    case "NT":
                        s.Append(Strings.NT);
                        break;
                }
                s.Append(ContractX);
                return s.ToString();
            }
        }

        // DECLARER
        public string DeclarerNSEWDisplay
        {
            get
            {
                switch (DeclarerNSEW)
                {
                    case "N":
                        return Strings.N;
                    case "S":
                        return Strings.S;
                    case "E":
                        return Strings.E;
                    case "W":
                        return Strings.W;
                    default:
                        return "";
                }
            }
        }

        // TRICKS TAKEN
        private int tricksTakenNumber;
        public int TricksTakenNumber
        {
            get
            {
                return tricksTakenNumber;
            }
            set
            {
                tricksTakenNumber = value;
            }
        }
        public string TricksTakenSymbol
        {
            get
            {
                if (tricksTakenNumber == -1)
                {
                    return "";
                }
                else
                {
                    int tricksTakenLevel = tricksTakenNumber - ContractLevel - 6;
                    if (tricksTakenLevel == 0)
                    {
                        return "=";
                    }
                    else
                    {
                        return tricksTakenLevel.ToString("+#;-#;0");
                    }
                }
            }
            set
            {
                if (value == "")
                {
                    tricksTakenNumber = -1;
                }
                else if (value == "=")
                {
                    tricksTakenNumber = ContractLevel + 6;
                }
                else
                {
                    tricksTakenNumber = ContractLevel + Convert.ToInt32(value) + 6;
                }
            }
        }

        // LEAD CARD
        private string leadCard;   // Internal representation of lead card uses 'T' for '10'; database uses '10'.
        public string LeadCard
        {
            get
            {
                return leadCard;
            }
            set
            {
                leadCard = value;
            }
        }
        public string LeadCardDB
        {
            get
            {
                if (leadCard == null || leadCard == "" || leadCard == "SKIP")
                {
                    return "";
                }
                else if (leadCard.Substring(1, 1) == "T")
                {
                    return leadCard.Substring(0, 1) + "10";
                }
                else
                {
                    return leadCard;
                }
            }
            set
            {
                if (value.Length > 2 && value.Substring(1, 2) == "10")
                {
                    leadCard = value.Substring(0, 1) + "T";
                }
                else
                {
                    leadCard = value;
                }
            }
        }
        public string LeadCardDisplay
        {
            get
            {
                string s = leadCard;
                if (s == null || s == "" || s == "SKIP") return "";
                s = s.Replace("S", "<span style=\"color:black\">&spades;</span>");
                s = s.Replace("H", "<span style=\"color:red\">&hearts;</span>");
                s = s.Replace("D", "<span style=\"color:lightsalmon\">&diams;</span>");
                s = s.Replace("C", "<span style=\"color:lightslategrey\">&clubs;</span>");
                s = s.Replace("T", Strings.TenShorthand);
                return s;
            }
        }

        public void CalculateScore()
        {
            Score = 0;
            if (ContractLevel <= 0) return;

            bool vul;
            if (DeclarerNSEW == "N" || DeclarerNSEW == "S")
            {
                vul = Utilities.NSVulnerability[(Convert.ToInt32(BoardNumber) - 1) % 16];
            }
            else
            {
                vul = Utilities.EWVulnerability[(Convert.ToInt32(BoardNumber) - 1) % 16];
            }
            int diff = TricksTakenNumber - ContractLevel - 6;
            if (diff < 0)      // Contract not made
            {
                if (ContractX == "")
                {
                    if (vul)
                    {
                        Score = 100 * diff;
                    }
                    else
                    {
                        Score = 50 * diff;
                    }
                }
                else if (ContractX == "x")
                {
                    if (vul)
                    {
                        Score = 300 * diff + 100;
                    }
                    else
                    {
                        Score = 300 * diff + 400;
                        if (diff == -1) Score -= 200;
                        if (diff == -2) Score -= 100;
                    }
                }
                else  // x = "xx"
                {
                    if (vul)
                    {
                        Score = 600 * diff + 200;
                    }
                    else
                    {
                        Score = 600 * diff + 800;
                        if (diff == -1) Score -= 400;
                        if (diff == -2) Score -= 200;
                    }
                }
            }
            else      // Contract made
            {
                // Basic score, game/part-score bonuses and making x/xx contract bonuses
                if (ContractSuit == "C" || ContractSuit == "D")
                {
                    if (ContractX == "")
                    {
                        Score = 20 * (TricksTakenNumber - 6);
                        if (ContractLevel <= 4)
                        {
                            Score += 50;
                        }
                        else
                        {
                            if (vul) Score += 500;
                            else Score += 300;
                        }
                    }
                    else if (ContractX == "x")
                    {
                        Score = 40 * ContractLevel + 50;
                        if (vul) Score += 200 * diff;
                        else Score += 100 * diff;
                        if (ContractLevel <= 2)
                        {
                            Score += 50;
                        }
                        else
                        {
                            if (vul) Score += 500;
                            else Score += 300;
                        }
                    }
                    else    // x = "xx"
                    {
                        Score = 80 * ContractLevel + 100;
                        if (vul) Score += 400 * diff;
                        else Score += 200 * diff;
                        if (ContractLevel == 1)
                        {
                            Score += 50;
                        }
                        else
                        {
                            if (vul) Score += 500;
                            else Score += 300;
                        }
                    }
                }
                else   // Major suits and NT
                {
                    if (ContractX == "")
                    {
                        Score = 30 * (TricksTakenNumber - 6);
                        if (ContractSuit == "NT")
                        {
                            Score += 10;
                            if (ContractLevel <= 2)
                            {
                                Score += 50;
                            }
                            else
                            {
                                if (vul) Score += 500;
                                else Score += 300;
                            }
                        }
                        else      // Major suit
                        {
                            if (ContractLevel <= 3)
                            {
                                Score += 50;
                            }
                            else
                            {
                                if (vul) Score += 500;
                                else Score += 300;
                            }
                        }
                    }
                    else if (ContractX == "x")
                    {
                        Score = 60 * ContractLevel + 50;
                        if (ContractSuit == "NT") Score += 20;
                        if (vul) Score += 200 * diff;
                        else Score += 100 * diff;
                        if (ContractLevel <= 1)
                        {
                            Score += 50;
                        }
                        else
                        {
                            if (vul) Score += 500;
                            else Score += 300;
                        }
                    }
                    else    // x = "xx"
                    {
                        Score = 120 * ContractLevel + 100;
                        if (ContractSuit == "NT") Score += 40;
                        if (vul) Score += 400 * diff + 500;
                        else Score += 200 * diff + 300;
                    }
                }
                // Slam bonuses
                if (ContractLevel == 6)
                {
                    if (vul) Score += 750;
                    else Score += 500;
                }
                else if (ContractLevel == 7)
                {
                    if (vul) Score += 1500;
                    else Score += 1000;
                }
            }
            if (DeclarerNSEW == "E" || DeclarerNSEW == "W") Score = -Score;
        }

        public void UpdateDB(TableStatus tableStatus)
        {
            int declarer;
            if (ContractLevel <= 0)
            {
                declarer = 0;
            }
            else
            {
                if (DeclarerNSEW == "N" || DeclarerNSEW == "S" || DeclarerNSEW == "NS")
                {
                    declarer = NumberNorth;
                }
                else
                {
                    declarer = NumberEast;
                }
            }

            using (OdbcConnection connection = new OdbcConnection(AppData.DBConnectionString))
            {
                // Delete any previous result
                connection.Open();
                string SQLString = $"DELETE FROM ReceivedData WHERE Section={tableStatus.SectionID} AND [Table]={tableStatus.TableNumber} AND Round={tableStatus.RoundNumber} AND Board={BoardNumber}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                catch { }

                // Add new result
                string remarks = "";
                if (ContractLevel == -1)
                {
                    remarks = "Not played";
                }
                if (AppData.IsIndividual)
                {
                    SQLString = $"INSERT INTO ReceivedData (Section, [Table], Round, Board, PairNS, PairEW, South, West, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({tableStatus.SectionID}, {tableStatus.TableNumber}, {tableStatus.RoundNumber}, {BoardNumber}, {NumberNorth}, {NumberEast}, {NumberSouth}, {NumberWest}, {declarer}, '{DeclarerNSEW}', '{Contract}', '{TricksTakenSymbol}', '{LeadCardDB}', '{remarks}', #{DateTime.Now:yyyy-MM-dd}#, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, False, False, False, False, False, False)";
                }
                else
                {
                    SQLString = $"INSERT INTO ReceivedData (Section, [Table], Round, Board, PairNS, PairEW, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({tableStatus.SectionID}, {tableStatus.TableNumber}, {tableStatus.RoundNumber}, {BoardNumber}, {NumberNorth}, {NumberEast}, {declarer}, '{DeclarerNSEW}', '{Contract}', '{TricksTakenSymbol}', '{LeadCardDB}', '{remarks}', #{DateTime.Now:yyyy-MM-dd}#, #{DateTime.Now:yyyy-MM-dd hh:mm:ss}#, False, False, False, False, False, False)";
                }
                cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd.ExecuteNonQuery();
                    });
                }
                catch { }
                cmd.Dispose();
            }
        }
    }
}
