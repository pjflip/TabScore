using System;
using System.Data.Odbc;
using System.Text;

namespace TabScore.Models
{
    public class ResultClass
    {
        public string SectionID { get; set; }
        public string Table { get; set; }
        public string Round { get; set; }
        public string Board { get; set; }
        public string PairNS { get; set; }
        public string PairEW { get; set; }
        public string NSEW { get; set; }
        public string ContractLevel { get; set; }
        public string ContractSuit { get; set; }
        public string ContractX { get; set; }
        public string LeadCard { get; set; }

        public string DisplayLeadCard()
        {
            if (LeadCard == null || LeadCard == "" || LeadCard == "SKIP")
            {
                return "";
            }
            StringBuilder s = new StringBuilder("");
            switch (LeadCard.Substring(0,1))
            {
                case "S":
                    s.Append("<a>&spades;</a>");
                    break;
                case "H":
                    s.Append("<a style=\"color:red\">&hearts;</a>");
                    break;
                case "D":
                    s.Append("<a style=\"color:red\">&diams;</a>");
                    break;
                case "C":
                    s.Append("<a>&clubs;</a>");
                    break;
            }
            if (LeadCard.Substring(1,1) == "1")
            {
                s.Append("T");
            }
            else
            {
                s.Append(LeadCard.Substring(1));
            }
            return s.ToString();
        }

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
                if (tricksTakenNumber == -1)
                {
                    tricksTakenSymbol = "";
                }
                else
                {
                    int tricksTakenLevel = tricksTakenNumber - Convert.ToInt32(ContractLevel) - 6;
                    if (tricksTakenLevel == 0)
                    {
                        tricksTakenSymbol = "=";
                    }
                    else
                    {
                        tricksTakenSymbol = tricksTakenLevel.ToString("+#;-#;0");
                    }
                }
            }
        }

        private string tricksTakenSymbol;
        public string TricksTakenSymbol
        {
            get
            {
                return tricksTakenSymbol;
            }
            set
            {
                tricksTakenSymbol = value;
                if (tricksTakenSymbol == "")
                {
                    tricksTakenNumber = -1;
                }
                else if (tricksTakenSymbol == "=")
                {
                    tricksTakenNumber = Convert.ToInt32(ContractLevel) + 6;
                }
                else
                {
                    tricksTakenNumber = Convert.ToInt32(ContractLevel) + Convert.ToInt32(tricksTakenSymbol) + 6;
                }
            }
        }

        public string DisplayContract(int FormatType = 1)
        {
            if (ContractLevel == null)
            {
                return "";
            }
            if (ContractLevel == "PASS")
            {
                if (FormatType > 1)
                {
                    return "PASSed Out";
                }
                else
                {
                    return "PASS";
                }
            }
            StringBuilder s = new StringBuilder("");
            s.Append(ContractLevel);
            switch (ContractSuit)
            {
                case "NT":
                    s.Append("NT");
                    break;
                case "S":
                    s.Append("<a>&spades;</a>");
                    break;
                case "H":
                    s.Append("<a style=\"color:red\">&hearts;</a>");
                    break;
                case "D":
                    s.Append("<a style=\"color:red\">&diams;</a>");
                    break;
                case "C":
                    s.Append("<a>&clubs;</a>");
                    break;
            }
            if (ContractX != "NONE")
            {
                s.Append(ContractX);
            }
            if (FormatType > 0)
            {
                s.Append($"{ TricksTakenSymbol} by ");
                if (FormatType > 1)
                {
                    switch (NSEW)
                    {
                        case "N":
                            s.Append("North");
                            break;
                        case "S":
                            s.Append("South");
                            break;
                        case "E":
                            s.Append("East");
                            break;
                        case "W":
                            s.Append("West");
                            break;
                    }
                }
                else
                {
                    s.Append(NSEW);
                }
            }
            return s.ToString();
        }

        public int Score()
        {
            if (ContractLevel == "PASS") return 0;
            int score = 0;
            bool vul;
            if (NSEW == "N" || NSEW == "S")
            {
                vul = Vulnerability.NSVul[(Convert.ToInt32(Board) - 1) % 16];
            }
            else
            {
                vul = Vulnerability.EWVul[(Convert.ToInt32(Board) - 1) % 16];
            }
            int level = Convert.ToInt32(ContractLevel);
            int diff = TricksTakenNumber - level - 6;
            if (diff < 0)      // Contract not made
            {
                if (ContractX == "NONE")
                {
                    if (vul)
                    {
                        score = 100 * diff;
                    }
                    else
                    {
                        score = 50 * diff;
                    }
                }
                else if (ContractX == "x")
                {
                    if (vul)
                    {
                        score = 300 * diff + 100;
                    }
                    else
                    {
                        score = 300 * diff + 400;
                        if (diff == -1) score -= 200;
                        if (diff == -2) score -= 100;
                    }
                }
                else  // x = "xx"
                {
                    if (vul)
                    {
                        score = 600 * diff + 200;
                    }
                    else
                    {
                        score = 600 * diff + 800;
                        if (diff == -1) score -= 400;
                        if (diff == -2) score -= 200;
                    }
                }
            }
            else      // Contract made
            {
                // Basic score, game/part-score bonuses and making x/xx contract bonuses
                if (ContractSuit == "C" || ContractSuit == "D")
                {
                    if (ContractX == "NONE")
                    {
                        score = 20 * (TricksTakenNumber - 6);
                        if (level <= 4)
                        {
                            score += 50;
                        }
                        else
                        {
                            if (vul) score += 500;
                            else score += 300;
                        }
                    }
                    else if (ContractX == "x")
                    {
                        score = 40 * level + 50;
                        if (vul) score += 200 * diff;
                        else score += 100 * diff;
                        if (level <= 2)
                        {
                            score += 50;
                        }
                        else
                        {
                            if (vul) score += 500;
                            else score += 300;
                        }
                    }
                    else    // x = "xx"
                    {
                        score = 80 * level + 100;
                        if (vul) score += 400 * diff;
                        else score += 200 * diff;
                        if (level == 1)
                        {
                            score += 50;
                        }
                        else
                        {
                            if (vul) score += 500;
                            else score += 300;
                        }
                    }
                }
                else   // Major suits and NT
                {
                    if (ContractX == "NONE")
                    {
                        score = 30 * (TricksTakenNumber - 6);
                        if (ContractSuit == "NT")
                        {
                            score += 10;
                            if (level <= 2)
                            {
                                score += 50;
                            }
                            else
                            {
                                if (vul) score += 500;
                                else score += 300;
                            }
                        }
                        else      // Major suit
                        {
                            if (level <= 3)
                            {
                                score += 50;
                            }
                            else
                            {
                                if (vul) score += 500;
                                else score += 300;
                            }
                        }
                    }
                    else if (ContractX == "x")
                    {
                        score = 60 * level + 50;
                        if (ContractSuit == "NT") score += 20;
                        if (vul) score += 200 * diff;
                        else score += 100 * diff;
                        if (level <= 1)
                        {
                            score += 50;
                        }
                        else
                        {
                            if (vul) score += 500;
                            else score += 300;
                        }
                    }
                    else    // x = "xx"
                    {
                        score = 120 * level + 100;
                        if (ContractSuit == "NT") score += 40;
                        if (vul) score += 400 * diff + 500;
                        else score += 200 * diff + 300;
                    }
                }
                // Slam bonuses
                if (level == 6)
                {
                    if (vul) score += 750;
                    else score += 500;
                }
                else if (level == 7)
                {
                    if (vul) score += 1500;
                    else score += 1000;
                }
            }
            if (NSEW == "N" || NSEW == "S")
            {
                return score;
            }
            else
            {
                return -score;
            }
        }

        public void UpdateDB(string DB)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                // Delete any previous result
                string SQLString = $"DELETE FROM ReceivedData WHERE Section={SectionID} AND [Table]={Table} AND Round={Round} AND Board={Board}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                cmd.ExecuteNonQuery();

                // Add new result
                string contract;
                string declarer;
                if (ContractLevel == "PASS")
                {
                    contract = "PASS";
                    declarer = "0";
                }
                else
                {
                    contract = $"{ContractLevel} {ContractSuit}";
                    if (ContractX != "NONE")
                    {
                        contract = $"{contract} {ContractX}";
                    }
                    if (NSEW == "N" || NSEW == "S")
                    {
                        declarer = PairNS;
                    }
                    else
                    {
                        declarer = PairEW;
                    }
                }
                string leadcard;
                if (LeadCard == null || LeadCard == "" || LeadCard == "SKIP")
                {
                    leadcard = "";
                }
                else
                {
                    leadcard = LeadCard;
                }

                SQLString = $"INSERT INTO ReceivedData (Section, [Table], Round, Board, PairNS, PairEW, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({SectionID}, {Table}, {Round}, {Board}, {PairNS}, {PairEW}, {declarer}, '{NSEW}', '{contract}', '{TricksTakenSymbol}', '{leadcard}', '', #{DateTime.Now.ToString("yyyy-MM-dd")}#, #{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, False, False, False, False, False, False)";
                cmd = new OdbcCommand(SQLString, connection);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
        
        public void GetDBResult(string DB)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                string SQLString;
                SQLString = $"SELECT [NS/EW], Contract, Result FROM ReceivedData WHERE Section={SectionID} AND [Table]={Table} AND Round={Round} AND Board={Board}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                OdbcDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string contract = reader.GetString(1);
                    if (contract == "PASS")
                    {
                        ContractLevel = "PASS";
                        ContractSuit = "";
                        ContractX = "";
                        NSEW = "";
                        TricksTakenNumber = -1;
                    }
                    else
                    {
                        string[] temp = contract.Split(' ');
                        ContractLevel = temp[0];
                        ContractSuit = temp[1];
                        if (temp.Length > 2) ContractX = temp[2];
                        else ContractX = "NONE";
                        NSEW = reader.GetString(0);
                        TricksTakenSymbol = reader.GetString(2);
                    }
                }
                reader.Close();
                cmd.Dispose();
            }
        }
    }
}
