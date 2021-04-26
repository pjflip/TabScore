// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterContractController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, int boardNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            if (tableStatus.ResultData == null) {
                tableStatus.ResultData = new Result(tableStatus, boardNumber);
            }
            ResultInfo resultInfo = new ResultInfo(tabletDeviceNumber, tableStatus);

            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"{tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"{tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}")}";
            }
            else
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", boardNumber, $"NS {tableStatus.RoundData.NumberNorth}")} v {Utilities.ColourPairByVulnerability("EW", boardNumber, $"EW {tableStatus.RoundData.NumberEast}")}";
            }
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabledAndBack;
            ViewData["Title"] = $"Enter Contract - {tabletDeviceStatus.Location}";
            return View(resultInfo);
        }

        public ActionResult OKButtonContract(int tabletDeviceNumber, int contractLevel, string contractSuit, string contractX, string declarerNSEW)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            Result result = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber).ResultData;
            result.ContractLevel = contractLevel;
            result.ContractSuit = contractSuit;
            result.ContractX = contractX;
            result.DeclarerNSEW = declarerNSEW;
            return RedirectToAction("Index", "EnterLead", new { tabletDeviceNumber, leadValidation = LeadValidationOptions.Validate });
        }

        public ActionResult OKButtonPass(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            Result result = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber).ResultData;
            result.ContractLevel = 0;
            result.ContractSuit = "";
            result.ContractX = "";
            result.DeclarerNSEW = "";
            result.LeadCard = "";
            result.TricksTakenNumber = -1;
            result.CalculateScore();
            return RedirectToAction("Index", "ConfirmResult", new { tabletDeviceNumber });
        }
        
        public ActionResult OKButtonSkip(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            Result result = tableStatus.ResultData;
            result.ContractLevel = -1;
            result.ContractSuit = "";
            result.ContractX = "";
            result.DeclarerNSEW = "";
            result.LeadCard = "";
            result.TricksTakenNumber = -1;
            result.UpdateDB(tableStatus);
            return RedirectToAction("Index", "ShowBoards", new { tabletDeviceNumber });
        }
    }
}
