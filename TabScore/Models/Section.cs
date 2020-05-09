// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class Section
    {
        public int ID {get; set;}
        public string Letter {get; set;}
        public int NumTables {get; set;}
        public int MissingPair {get; set;}
    }
}