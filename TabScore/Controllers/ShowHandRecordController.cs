// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowHandRecordController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int boardNumber)
        {
            HandRecord handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == sectionID && x.BoardNumber == boardNumber);
            if (handRecord == null)     // Can't find matching hand record, so use default SectionID = 1
            {
                handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == 1 && x.BoardNumber == boardNumber);
            }
            handRecord.TableNumber = tableNumber;

            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            ViewData["Title"] = $"Hand Record - {tableStatus.SectionTableString}";
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"{tableStatus.RoundData.PairNS}+{tableStatus.RoundData.South}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"{tableStatus.RoundData.PairEW}+{tableStatus.RoundData.West}")}";
            }
            else
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"NS {tableStatus.RoundData.PairNS}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"EW {tableStatus.RoundData.PairEW}")}";
            }
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            return View(handRecord);
        }
    }
}