using System;
using System.Data.Odbc;
using System.Text;

namespace TabScore.Models
{
    public class Result
    {
        public int SectionID { get; set; }
        public int Table { get; set; }
        public int RoundNumber { get; set; }
        public int Board { get; set; }
        public int PairNS { get; set; }
        public int South { get; set; }
        public int PairEW { get; set; }
        public int West { get; set; }
        public string NSEW { get; set; }
        public int ContractLevel { get; set; }
        public string ContractSuit { get; set; }
        public string ContractX { get; set; }
        public string LeadCard { get; set; }
        public int MPNS { get; set; }
        public int MPEW { get; set; }
        public int MPMax { get; set; }

        public string Contract
        {
            get
            {
                if (ContractLevel < 0)
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
                    if (ContractX != "NONE")
                    {
                        contract = $"{contract} {ContractX}";
                    }
                    return contract;
                }
            }
            set
            {
                if (value == "PASS")
                {
                    ContractLevel = 0;
                    ContractSuit = "";
                    ContractX = "";
                }
                else
                {
                    string[] temp = value.Split(' ');
                    ContractLevel = Convert.ToInt32(temp[0]);
                    ContractSuit = temp[1];
                    if (temp.Length > 2) ContractX = temp[2];
                    else ContractX = "NONE";
                }
            }
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
                    int tricksTakenLevel = tricksTakenNumber - ContractLevel - 6;
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
                    tricksTakenNumber = ContractLevel + 6;
                }
                else
                {
                    tricksTakenNumber = ContractLevel + Convert.ToInt32(tricksTakenSymbol) + 6;
                }
            }
        }

        public string DisplayContract()
        {
            if (ContractLevel < 0)
            {
                return "";
            }
            if (ContractLevel == 0)
            {
                return "<a style=\"color:darkgreen\">PASSed Out</a>";
            }
            StringBuilder s = new StringBuilder("");
            s.Append(ContractLevel);
            switch (ContractSuit)
            {
                case "NT":
                    s.Append("NT");
                    break;
                case "S":
                    s.Append("<a style=\"color:black\">&spades;</a>");
                    break;
                case "H":
                    s.Append("<a style=\"color:red\">&hearts;</a>");
                    break;
                case "D":
                    s.Append("<a style=\"color:lightsalmon\">&diams;</a>");
                    break;
                case "C":
                    s.Append("<a style=\"color:lightslategrey\">&clubs;</a>");
                    break;
            }
            if (ContractX != "NONE")
            {
                s.Append(ContractX);
            }
            s.Append($"{TricksTakenSymbol} by ");
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
            return s.ToString();
        }

        public string DisplayLeadCard()
        {
            string s = LeadCard;
            if (s == null || s == "" || s == "SKIP") return "";
            s = s.Replace("S", "<a style=\"color:black\">&spades;</a>");
            s = s.Replace("H", "<a style=\"color:red\">&hearts;</a>");
            s = s.Replace("D", "<a style=\"color:lightsalmon\">&diams;</a>");
            s = s.Replace("C", "<a style=\"color:lightslategrey\">&clubs;</a>");
            s = s.Replace("10", "T");
            return s;
        }

        public string DisplayTravellerContract()
        {
            if (ContractLevel < 0) return "";
            if (ContractLevel == 0) return "<a style=\"color:darkgreen\">PASS</a>";
            StringBuilder s = new StringBuilder(ContractLevel.ToString());
            switch (ContractSuit) {
                case "S":
                    s.Append("<a style=\"color:black\">&spades;</a>");
                    break;
                case "H":
                    s.Append("<a style=\"color:red\">&hearts;</a>");
                    break;
                case "D":
                    s.Append("<a style=\"color:lightsalmon\">&diams;</a>");
                    break;
                case "C":
                    s.Append("<a style=\"color:lightslategrey\">&clubs;</a>");
                    break;
                case "NT":
                    s.Append("NT");
                    break;
            }
            if (ContractX != "NONE")
            {
                s.Append(ContractX);
            }
            return s.ToString();
        }

        public int Score { get; private set; }

        public void CalculateScore()
        {
            Score = 0;
            if (ContractLevel == 0) return;

            bool vul;
            if (NSEW == "N" || NSEW == "S")
            {
                vul = UtilityFunctions.NSVulnerability[(Convert.ToInt32(Board) - 1) % 16];
            }
            else
            {
                vul = UtilityFunctions.EWVulnerability[(Convert.ToInt32(Board) - 1) % 16];
            }
            int diff = TricksTakenNumber - ContractLevel - 6;
            if (diff < 0)      // Contract not made
            {
                if (ContractX == "NONE")
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
                    if (ContractX == "NONE")
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
                    if (ContractX == "NONE")
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
            if (NSEW == "E" || NSEW == "W") Score = -Score;
        }

        public void WriteToDB(string DB, bool individual)
        {
            int declarer;
            if (ContractLevel == 0)
            {
                declarer = 0;
            }
            else
            {
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

            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                // Delete any previous result
                connection.Open();
                string SQLString = $"DELETE FROM ReceivedData WHERE Section={SectionID} AND [Table]={Table} AND Round={RoundNumber} AND Board={Board}";
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

                // Add new result
                if (individual)
                {
                    SQLString = $"INSERT INTO ReceivedData (Section, [Table], Round, Board, PairNS, PairEW, South, West, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({SectionID}, {Table}, {RoundNumber}, {Board}, {PairNS}, {PairEW}, {South}, {West}, {declarer}, '{NSEW}', '{Contract}', '{TricksTakenSymbol}', '{leadcard}', '', #{DateTime.Now.ToString("yyyy-MM-dd")}#, #{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, False, False, False, False, False, False)";
                }
                else
                {
                    SQLString = $"INSERT INTO ReceivedData (Section, [Table], Round, Board, PairNS, PairEW, Declarer, [NS/EW], Contract, Result, LeadCard, Remarks, DateLog, TimeLog, Processed, Processed1, Processed2, Processed3, Processed4, Erased) VALUES ({SectionID}, {Table}, {RoundNumber}, {Board}, {PairNS}, {PairEW}, {declarer}, '{NSEW}', '{Contract}', '{TricksTakenSymbol}', '{leadcard}', '', #{DateTime.Now.ToString("yyyy-MM-dd")}#, #{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}#, False, False, False, False, False, False)";
                }
                OdbcCommand cmd2 = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        cmd2.ExecuteNonQuery();
                    });
                }
                finally
                {
                    cmd2.Dispose();
                }
            }
        }
        
        public void ReadFromDB(string DB)
        {
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                string SQLString = $"SELECT [NS/EW], Contract, Result, LeadCard FROM ReceivedData WHERE Section={SectionID} AND [Table]={Table} AND Round={RoundNumber} AND Board={Board}";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                OdbcDataReader reader = null;
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            string temp = reader.GetString(1);
                            Contract = temp;
                            if (ContractLevel == 0)
                            {
                                NSEW = "";
                                TricksTakenNumber = -1;
                                LeadCard = "";
                            }
                            else
                            {
                                NSEW = reader.GetString(0);
                                TricksTakenSymbol = reader.GetString(2);
                                LeadCard = reader.GetString(3);
                            }
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
    }
}
