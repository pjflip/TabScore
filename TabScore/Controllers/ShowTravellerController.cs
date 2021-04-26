// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowTravellerController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, int boardNumber, bool fromView = false)
        {
            if (!Settings.ShowResults)
            {
                return RedirectToAction("Index", "ShowBoards", new { tabletDeviceNumber });
            }

            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            if (fromView)
            {
                tableStatus.ResultData = new Result(tableStatus, boardNumber);
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            }
            else
            {
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabledAndBack;
            }

            Traveller traveller = new Traveller(tabletDeviceNumber, tableStatus);
            traveller.FromView = fromView;

            ViewData["Title"] = $"Traveller - {tabletDeviceStatus.Location}";
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"{tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"{tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}")}";
                return View("Individual", traveller);
            }
            else
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"NS {tableStatus.RoundData.NumberNorth}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"EW {tableStatus.RoundData.NumberEast}")}";
                return View("Pairs", traveller);
            }
        }
    }
}
