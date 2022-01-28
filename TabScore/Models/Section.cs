// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class Section
    {
        public int SectionID { get; set; }
        public string SectionLetter { get; set; }
        public int NumTables { get; set; }
        public int MissingPair { get; set; }
        public int Winners { get; set; }
        public int ScoringType { get; set; }
        public int TabletDevicesPerTable { get; set; } = 1;
    }
}