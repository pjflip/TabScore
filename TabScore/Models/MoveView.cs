namespace TabScore.Models
{
    public class MoveView
    {
        public int RoundNumber { get; set; }
        public int LowBoard { get; set; }
        public int HighBoard { get; set; }
        public int PairNS { get; set; }
        public int NorthNewTable {get; set;}
        public string NorthNewDirection {get; set;}
        public bool NorthStay {get; set;}
        public int South { get; set; }
        public int SouthNewTable {get; set;}
        public string SouthNewDirection {get; set;}
        public bool SouthStay {get; set;}
        public int PairEW { get; set; }
        public int EastNewTable {get; set;}
        public string EastNewDirection {get; set;}
        public bool EastStay {get; set;}
        public int West { get; set; }
        public int WestNewTable {get; set;}
        public string WestNewDirection {get; set;}
        public bool WestStay {get; set;}
        public int BoardsNewTable {get; set;}
        public bool NSMissing {get; set;}
        public bool EWMissing {get; set;}
    }
}