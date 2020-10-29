// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTricksTakenController : Controller
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
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabledAndBack;
            ViewData["Title"] = $"Enter Tricks Taken - {tableStatus.SectionTableString}";
            if (Settings.EnterResultsMethod == 1)
            {
                return View("TotalTricks", tableStatus);
            }
            else
            {
                return View("TricksPlusMinus", tableStatus);
            }
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, int numTricks)
        {
            Result result = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber).ResultData;
            result.TricksTakenNumber = numTricks;
            result.CalculateScore();
            return RedirectToAction("Index", "ConfirmResult", new { sectionID, tableNumber });
        }

        public ActionResult BackButtonClick(int sectionID, int tableNumber)
        {
            if (Settings.EnterLeadCard)
            {
                return RedirectToAction("Index", "EnterLead", new { sectionID, tableNumber, leadValidation = LeadValidationOptions.NoWarning });
            }
            else
            {
                return RedirectToAction("Index", "EnterContract", new { sectionID, tableNumber, boardNumber = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber).ResultData.BoardNumber });
            }
        }
    }
}