using System;
using System.Collections.Generic;
using System.IO;

namespace TabScoreStarter
{
    public static class PBNRead
    {
        public static List<HandClass> ReadFile(string pathToFile)
        {
            List<HandClass> handList = new List<HandClass>();
            string[] dealLine;
            string line = null;
            char[] quoteDelimiter = { '"' };
            char[] PBNDelimiter = { ':', '.', ' ' };
            bool newBoard = false;

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
                    HandClass hand = new HandClass()
                    {
                        NorthSpades = "###",
                        EvalNorthNT = -1
                    };
                    hand.Board = line.Split(quoteDelimiter)[1];
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Length > 6 && line.Substring(0, 6) == "[Deal ")
                        {
                            line = line.Split(quoteDelimiter)[1];
                            dealLine = line.Split(PBNDelimiter);
                            switch (dealLine[0])
                            {
                                case "N":
                                    hand.NorthSpades = dealLine[1];
                                    hand.NorthHearts = dealLine[2];
                                    hand.NorthDiamonds = dealLine[3];
                                    hand.NorthClubs = dealLine[4];
                                    hand.EastSpades = dealLine[5];
                                    hand.EastHearts = dealLine[6];
                                    hand.EastDiamonds = dealLine[7];
                                    hand.EastClubs = dealLine[8];
                                    hand.SouthSpades = dealLine[9];
                                    hand.SouthHearts = dealLine[10];
                                    hand.SouthDiamonds = dealLine[11];
                                    hand.SouthClubs = dealLine[12];
                                    hand.WestSpades = dealLine[13];
                                    hand.WestHearts = dealLine[14];
                                    hand.WestDiamonds = dealLine[15];
                                    hand.WestClubs = dealLine[16];
                                    break;
                                case "E":
                                    hand.EastSpades = dealLine[1];
                                    hand.EastHearts = dealLine[2];
                                    hand.EastDiamonds = dealLine[3];
                                    hand.EastClubs = dealLine[4];
                                    hand.SouthSpades = dealLine[5];
                                    hand.SouthHearts = dealLine[6];
                                    hand.SouthDiamonds = dealLine[7];
                                    hand.SouthClubs = dealLine[8];
                                    hand.WestSpades = dealLine[9];
                                    hand.WestHearts = dealLine[10];
                                    hand.WestDiamonds = dealLine[11];
                                    hand.WestClubs = dealLine[12];
                                    hand.NorthSpades = dealLine[13];
                                    hand.NorthHearts = dealLine[14];
                                    hand.NorthDiamonds = dealLine[15];
                                    hand.NorthClubs = dealLine[16];
                                    break;
                                case "S":
                                    hand.SouthSpades = dealLine[1];
                                    hand.SouthHearts = dealLine[2];
                                    hand.SouthDiamonds = dealLine[3];
                                    hand.SouthClubs = dealLine[4];
                                    hand.WestSpades = dealLine[5];
                                    hand.WestHearts = dealLine[6];
                                    hand.WestDiamonds = dealLine[7];
                                    hand.WestClubs = dealLine[8];
                                    hand.NorthSpades = dealLine[9];
                                    hand.NorthHearts = dealLine[10];
                                    hand.NorthDiamonds = dealLine[11];
                                    hand.NorthClubs = dealLine[12];
                                    hand.EastSpades = dealLine[13];
                                    hand.EastHearts = dealLine[14];
                                    hand.EastDiamonds = dealLine[15];
                                    hand.EastClubs = dealLine[16];
                                    break;
                                case "W":
                                    hand.WestSpades = dealLine[1];
                                    hand.WestHearts = dealLine[2];
                                    hand.WestDiamonds = dealLine[3];
                                    hand.WestClubs = dealLine[4];
                                    hand.NorthSpades = dealLine[5];
                                    hand.NorthHearts = dealLine[6];
                                    hand.NorthDiamonds = dealLine[7];
                                    hand.NorthClubs = dealLine[8];
                                    hand.EastSpades = dealLine[9];
                                    hand.EastHearts = dealLine[10];
                                    hand.EastDiamonds = dealLine[11];
                                    hand.EastClubs = dealLine[12];
                                    hand.SouthSpades = dealLine[13];
                                    hand.SouthHearts = dealLine[14];
                                    hand.SouthDiamonds = dealLine[15];
                                    hand.SouthClubs = dealLine[16];
                                    break;
                            }
                            hand.HCPNorth = 0;
                            hand.HCPEast = 0;
                            hand.HCPSouth = 0;
                            hand.HCPWest = 0;
                            if (hand.NorthSpades.Contains("A")) hand.HCPNorth += 4;
                            if (hand.NorthHearts.Contains("A")) hand.HCPNorth += 4;
                            if (hand.NorthDiamonds.Contains("A")) hand.HCPNorth += 4;
                            if (hand.NorthClubs.Contains("A")) hand.HCPNorth += 4;
                            if (hand.EastSpades.Contains("A")) hand.HCPEast += 4;
                            if (hand.EastHearts.Contains("A")) hand.HCPEast += 4;
                            if (hand.EastDiamonds.Contains("A")) hand.HCPEast += 4;
                            if (hand.EastClubs.Contains("A")) hand.HCPEast += 4;
                            if (hand.SouthSpades.Contains("A")) hand.HCPSouth += 4;
                            if (hand.SouthHearts.Contains("A")) hand.HCPSouth += 4;
                            if (hand.SouthDiamonds.Contains("A")) hand.HCPSouth += 4;
                            if (hand.SouthClubs.Contains("A")) hand.HCPSouth += 4;
                            if (hand.WestSpades.Contains("A")) hand.HCPWest += 4;
                            if (hand.WestHearts.Contains("A")) hand.HCPWest += 4;
                            if (hand.WestDiamonds.Contains("A")) hand.HCPWest += 4;
                            if (hand.WestClubs.Contains("A")) hand.HCPWest += 4;
                            if (hand.NorthSpades.Contains("K")) hand.HCPNorth += 3;
                            if (hand.NorthHearts.Contains("K")) hand.HCPNorth += 3;
                            if (hand.NorthDiamonds.Contains("K")) hand.HCPNorth += 3;
                            if (hand.NorthClubs.Contains("K")) hand.HCPNorth += 3;
                            if (hand.EastSpades.Contains("K")) hand.HCPEast += 3;
                            if (hand.EastHearts.Contains("K")) hand.HCPEast += 3;
                            if (hand.EastDiamonds.Contains("K")) hand.HCPEast += 3;
                            if (hand.EastClubs.Contains("K")) hand.HCPEast += 3;
                            if (hand.SouthSpades.Contains("K")) hand.HCPSouth += 3;
                            if (hand.SouthHearts.Contains("K")) hand.HCPSouth += 3;
                            if (hand.SouthDiamonds.Contains("K")) hand.HCPSouth += 3;
                            if (hand.SouthClubs.Contains("K")) hand.HCPSouth += 3;
                            if (hand.WestSpades.Contains("K")) hand.HCPWest += 3;
                            if (hand.WestHearts.Contains("K")) hand.HCPWest += 3;
                            if (hand.WestDiamonds.Contains("K")) hand.HCPWest += 3;
                            if (hand.WestClubs.Contains("K")) hand.HCPWest += 3;
                            if (hand.NorthSpades.Contains("Q")) hand.HCPNorth += 2;
                            if (hand.NorthHearts.Contains("Q")) hand.HCPNorth += 2;
                            if (hand.NorthDiamonds.Contains("Q")) hand.HCPNorth += 2;
                            if (hand.NorthClubs.Contains("Q")) hand.HCPNorth += 2;
                            if (hand.EastSpades.Contains("Q")) hand.HCPEast += 2;
                            if (hand.EastHearts.Contains("Q")) hand.HCPEast += 2;
                            if (hand.EastDiamonds.Contains("Q")) hand.HCPEast += 2;
                            if (hand.EastClubs.Contains("Q")) hand.HCPEast += 2;
                            if (hand.SouthSpades.Contains("Q")) hand.HCPSouth += 2;
                            if (hand.SouthHearts.Contains("Q")) hand.HCPSouth += 2;
                            if (hand.SouthDiamonds.Contains("Q")) hand.HCPSouth += 2;
                            if (hand.SouthClubs.Contains("Q")) hand.HCPSouth += 2;
                            if (hand.WestSpades.Contains("Q")) hand.HCPWest += 2;
                            if (hand.WestHearts.Contains("Q")) hand.HCPWest += 2;
                            if (hand.WestDiamonds.Contains("Q")) hand.HCPWest += 2;
                            if (hand.WestClubs.Contains("Q")) hand.HCPWest += 2;
                            if (hand.NorthSpades.Contains("J")) hand.HCPNorth += 1;
                            if (hand.NorthHearts.Contains("J")) hand.HCPNorth += 1;
                            if (hand.NorthDiamonds.Contains("J")) hand.HCPNorth += 1;
                            if (hand.NorthClubs.Contains("J")) hand.HCPNorth += 1;
                            if (hand.EastSpades.Contains("J")) hand.HCPEast += 1;
                            if (hand.EastHearts.Contains("J")) hand.HCPEast += 1;
                            if (hand.EastDiamonds.Contains("J")) hand.HCPEast += 1;
                            if (hand.EastClubs.Contains("J")) hand.HCPEast += 1;
                            if (hand.SouthSpades.Contains("J")) hand.HCPSouth += 1;
                            if (hand.SouthHearts.Contains("J")) hand.HCPSouth += 1;
                            if (hand.SouthDiamonds.Contains("J")) hand.HCPSouth += 1;
                            if (hand.SouthClubs.Contains("J")) hand.HCPSouth += 1;
                            if (hand.WestSpades.Contains("J")) hand.HCPWest += 1;
                            if (hand.WestHearts.Contains("J")) hand.HCPWest += 1;
                            if (hand.WestDiamonds.Contains("J")) hand.HCPWest += 1;
                            if (hand.WestClubs.Contains("J")) hand.HCPWest += 1;
                        }
                        else if (line.Length > 19 && line.Substring(0, 19) == "[DoubleDummyTricks ")
                        {
                            line = line.Split(quoteDelimiter)[1];
                            hand.EvalNorthNT = ddConvert(line.Substring(0, 1));
                            hand.EvalNorthSpades = ddConvert(line.Substring(1, 1));
                            hand.EvalNorthHearts = ddConvert(line.Substring(2, 1));
                            hand.EvalNorthDiamonds = ddConvert(line.Substring(3, 1));
                            hand.EvalNorthClubs = ddConvert(line.Substring(4, 1));
                            hand.EvalSouthNT = ddConvert(line.Substring(5, 1));
                            hand.EvalSouthSpades = ddConvert(line.Substring(6, 1));
                            hand.EvalSouthHearts = ddConvert(line.Substring(7, 1));
                            hand.EvalSouthDiamonds = ddConvert(line.Substring(8, 1));
                            hand.EvalSouthClubs = ddConvert(line.Substring(9, 1));
                            hand.EvalEastNT = ddConvert(line.Substring(10, 1));
                            hand.EvalEastSpades = ddConvert(line.Substring(11, 1));
                            hand.EvalEastHearts = ddConvert(line.Substring(12, 1));
                            hand.EvalEastDiamonds = ddConvert(line.Substring(13, 1));
                            hand.EvalEastClubs = ddConvert(line.Substring(14, 1));
                            hand.EvalWestSpades = ddConvert(line.Substring(15, 1));
                            hand.EvalWestNT = ddConvert(line.Substring(16, 1));
                            hand.EvalWestHearts = ddConvert(line.Substring(17, 1));
                            hand.EvalWestDiamonds = ddConvert(line.Substring(18, 1));
                            hand.EvalWestClubs = ddConvert(line.Substring(19, 1));
                        }
                        else if (line.Length > 7 && line.Substring(0, 7) == "[Board ")
                        {
                            newBoard = true;
                            if (hand.NorthSpades != "###") handList.Add(hand);
                            break;
                        }
                    }
                    if (file.EndOfStream)
                    {
                        if (hand.NorthSpades != "###") handList.Add(hand);
                    }
                }
                else if (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    newBoard = line.Length > 7 && line.Substring(0, 7) == "[Board ";
                }
            }
            file.Close();
            return handList;
        }

        private static int ddConvert(string ddVal)
        {
            switch (ddVal)
            {
                case "d":
                    return 13;
                case "c":
                    return 12;
                case "b":
                    return 11;
                case "a":
                    return 10;
                default:
                    return Convert.ToInt32(ddVal);
            }
        }
    }
}
