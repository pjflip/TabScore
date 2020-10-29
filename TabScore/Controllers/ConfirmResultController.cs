// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ConfirmResultController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);

            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.ResultData.BoardNumber, $"{tableStatus.RoundData.PairNS}+{tableStatus.RoundData.South}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.ResultData.BoardNumber, $"{tableStatus.RoundData.PairEW}+{tableStatus.RoundData.West}")}";
            }
            else
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.ResultData.BoardNumber, $"NS {tableStatus.RoundData.PairNS}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.ResultData.BoardNumber, $"EW {tableStatus.RoundData.PairEW}")}";
            }
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabledAndBack;
            ViewData["Title"] = $"Confirm Result - {tableStatus.SectionTableString}";
            return View(tableStatus);
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            tableStatus.UpdateDbResult();
            return RedirectToAction("Index", "ShowTraveller", new { sectionID, tableNumber, boardNumber = tableStatus.ResultData.BoardNumber });
        }

        public ActionResult BackButtonClick(int sectionID, int tableNumber)
        {
            Result result = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber).ResultData;
            if (result.ContractLevel == 0)  // This was passed out, so Back goes all the way to Enter Contract screen
            {
                return RedirectToAction("Index", "EnterContract", new { sectionID, tableNumber, boardNumber = result.BoardNumber });
            }
            else
            {
                return RedirectToAction("Index", "EnterTricksTaken", new { sectionID, tableNumber });
            }
        }
    }
}
