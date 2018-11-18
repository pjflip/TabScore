using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TabScore.Models
{
    public static class Dealer
    {
        public static string GetDealerForBoard(string boardNo)
        {
            int board = Convert.ToInt32(boardNo);
            switch ((board - 1) % 4)
            {
                case 0:
                    return "N";
                case 1:
                    return "E";
                case 2:
                    return "S";
                case 3:
                    return "W";
                default:
                    return "#";

            }
        }
    }
}