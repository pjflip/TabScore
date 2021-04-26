// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowHandRecordController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, int boardNumber, bool fromView)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            HandRecord handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.BoardNumber == boardNumber);
            if (handRecord == null)     // Can't find matching hand record, so use default SectionID = 1
            {
                handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == 1 && x.BoardNumber == boardNumber);
            }
            handRecord.TabletDeviceNumber = tabletDeviceNumber;
            handRecord.FromView = fromView;

            ViewData["Title"] = $"Hand Record - {tabletDeviceStatus.Location}";
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"{tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"{tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}")}";
            }
            else
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"NS {tableStatus.RoundData.NumberNorth}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"EW {tableStatus.RoundData.NumberEast}")}";
            }
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            return View(handRecord);
        }
    }
}