// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterContractController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int boardNumber)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.ResultData == null) tableStatus.GetDbResult(boardNumber);

            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"{tableStatus.RoundData.PairNS}+{tableStatus.RoundData.South}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"{tableStatus.RoundData.PairEW}+{tableStatus.RoundData.West}")}";
            }
            else
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"NS {tableStatus.RoundData.PairNS}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"EW {tableStatus.RoundData.PairEW}")}";
            }
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabledAndBack;
            ViewData["Title"] = $"Enter Contract - {tableStatus.SectionTableString}";
            return View(tableStatus);
        }

        public ActionResult OKButtonContract(int sectionID, int tableNumber, int contractLevel, string contractSuit, string contractX, string declarerNSEW)
        {
            Result result = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber).ResultData;
            result.ContractLevel = contractLevel;
            result.ContractSuit = contractSuit;
            result.ContractX = contractX;
            result.DeclarerNSEW = declarerNSEW;
            return RedirectToAction("Index", "EnterLead", new { sectionID, tableNumber, leadValidation = LeadValidationOptions.Validate });
        }

        public ActionResult OKButtonPass(int sectionID, int tableNumber)
        {
            Result result = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber).ResultData;
            result.ContractLevel = 0;
            result.ContractSuit = "";
            result.ContractX = "";
            result.DeclarerNSEW = "";
            result.LeadCard = "";
            result.TricksTakenNumber = -1;
            result.CalculateScore();
            return RedirectToAction("Index", "ConfirmResult", new { sectionID, tableNumber });
        }
        
        public ActionResult OKButtonSkip(int sectionID, int tableNumber)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            tableStatus.ResultData.ContractLevel = -1;
            tableStatus.ResultData.ContractSuit = "";
            tableStatus.ResultData.ContractX = "";
            tableStatus.ResultData.DeclarerNSEW = "";
            tableStatus.ResultData.LeadCard = "";
            tableStatus.ResultData.TricksTakenNumber = -1;
            tableStatus.UpdateDbResult();
            return RedirectToAction("Index", "ShowBoards", new { sectionID, tableNumber });
        }
    }
}
