namespace TabScore.Models
{
    public class EnterTableNumberDirection
    {
        public int SectionID { get; set; }
        public int TableNumber { get; set; }
        public int NumTables { get; set; }
        public string Direction { get; set; }
        public int RoundNumber { get; set; }
        public bool NorthMissing { get; set; } = false;
        public bool EastMissing { get; set; } = false;
        public bool SouthMissing { get; set; } = false;
        public bool WestMissing { get; set; } = false;
        public bool Confirm { get; set; }
    }
}