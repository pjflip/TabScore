// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowTravellerController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int boardNumber)
        {
            if (!Settings.ShowResults)
            {
                return RedirectToAction("Index", "ShowBoards");
            }

            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.ResultData == null)
            {
                // No result data for this board, so must have come from ShowBoards screen View button
                tableStatus.ResultData = new Result() { BoardNumber = boardNumber };
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            }
            else
            {
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabledAndBack;
            }

            Traveller traveller = new Traveller(tableStatus);
            ViewData["Title"] = $"Traveller - {tableStatus.SectionTableString}";
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"{tableStatus.RoundData.PairNS}+{tableStatus.RoundData.South}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"{tableStatus.RoundData.PairEW}+{tableStatus.RoundData.West}")}";
                return View("Individual", traveller);
            }
            else
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"NS {tableStatus.RoundData.PairNS}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"EW {tableStatus.RoundData.PairEW}")}";
                return View("Pairs", traveller);
            }
        }
    }
}
