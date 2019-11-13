using System;

namespace TabScore.Models
{
    public static class Dealer
    {
        public static string GetDealerForBoard(int board)
        {
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