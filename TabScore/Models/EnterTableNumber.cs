// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class EnterTableNumber
    {
        public int SectionID { get; set; }
        public int TableNumber { get; set; }
        public int NumTables { get; set; }
        public bool Confirm { get; set; }

        public EnterTableNumber(Section section, int tableNumber, bool confirm)
        {
            SectionID = section.SectionID;
            TableNumber = tableNumber;
            NumTables = section.NumTables;
            Confirm = confirm;
        }
    }
}