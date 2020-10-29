// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterLeadController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, LeadValidationOptions leadValidation)
        {
            if (!Settings.EnterLeadCard)
            {
                return RedirectToAction("Index", "EnterTricksTaken");
            }

            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.ResultData.LeadCard == "")  // Lead not set, so use leadValidation value as passed to controller
            {
                tableStatus.LeadValidation = leadValidation;
            }
            else  // Lead already set, so must be an edit (ie no validation and no warning)
            {
                tableStatus.LeadValidation = LeadValidationOptions.NoWarning;
            }
            
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.ResultData.BoardNumber, $"{tableStatus.RoundData.PairNS}+{tableStatus.RoundData.South}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.ResultData.BoardNumber, $"{tableStatus.RoundData.PairEW}+{tableStatus.RoundData.West}")}";
            }
            else
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.ResultData.BoardNumber, $"NS {tableStatus.RoundData.PairNS}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.ResultData.BoardNumber, $"EW {tableStatus.RoundData.PairEW}")}";
            }
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabledAndBack;
            ViewData["Title"] = $"Enter Lead - {tableStatus.SectionTableString}";
            return View(tableStatus);
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, string card)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.LeadValidation != LeadValidationOptions.Validate || !Settings.ValidateLeadCard || Utilities.ValidateLead(tableStatus, card))
            {
                tableStatus.ResultData.LeadCard = card;
                return RedirectToAction("Index", "EnterTricksTaken", new { sectionID, tableNumber });
            }
            else
            {
                return RedirectToAction("Index", "EnterLead", new { sectionID, tableNumber, leadValidation = LeadValidationOptions.Warning });
            }
        }
    }
}