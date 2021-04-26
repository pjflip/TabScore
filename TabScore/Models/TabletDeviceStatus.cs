// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class TabletDeviceStatus
    {
        public int SectionID { get; private set; }
        public int TableNumber { get; set; }
        public int PairNumber { get; set; }
        public string Direction { get; set; }
        public string Location { get; set; }
        public int RoundNumber { get; set; }
        public bool NamesUpdateRequired { get; set; } = true;

        public TabletDeviceStatus(int sectionID, int tableNumber, string direction, int pairNumber, int roundNumber)
        {
            SectionID = sectionID;
            TableNumber = tableNumber;
            Direction = direction;
            PairNumber = pairNumber;
            RoundNumber = roundNumber;
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            if (section.TabletDevicesPerTable > 1)
            {
                Location = AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter + tableNumber.ToString() + " " + direction;
            }
            else
            {
                Location = "Table " + AppData.SectionsList.Find(x => x.SectionID == sectionID).SectionLetter + tableNumber.ToString();
            }
        }
        public void Update(int tableNumber, string direction, int roundNumber)
        {
            TableNumber = tableNumber;
            Direction = direction;
            RoundNumber = roundNumber;
            Section section = AppData.SectionsList.Find(x => x.SectionID == SectionID);
            if (tableNumber == 0)
            {
                Location = AppData.SectionsList.Find(x => x.SectionID == SectionID).SectionLetter + " Sitout";
            }
            else
            {
                Location = AppData.SectionsList.Find(x => x.SectionID == SectionID).SectionLetter + tableNumber.ToString() + " " + direction;
            }
        }
    }
}