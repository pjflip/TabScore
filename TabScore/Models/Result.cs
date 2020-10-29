// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Text;

namespace TabScore.Models
{
    public class Result
    {
        public int BoardNumber { get; set; }
        public int PairNS { get; set; }
        public int South { get; set; }
        public int PairEW { get; set; }
        public int West { get; set; }
        public string DeclarerNSEW { get; set; }
        public int ContractLevel { get; set; }
        public string ContractSuit { get; set; }
        public string ContractX { get; set; }
        public string LeadCard { get; set; }
        public int Score { get; private set; }
        public int MatchpointsNS { get; set; }
        public int MatchpointsEW { get; set; }
        public int MatchpointsMax { get; set; }

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
            if (ContractLevel == -999)  // No result entry yet
            {
                return "";
            }
            else if (ContractLevel == -1)  // Board not played
            {
                return "<a style=\"color:red\">Not played</a>";
            }
            else if (ContractLevel == 0)
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
            s.Append(ContractX);
            s.Append($"{TricksTakenSymbol} by {DeclarerNSEW}");
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
            s.Append(ContractX);
            return s.ToString();
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
    }
}
