// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTricksTakenController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            ResultInfo resultInfo = new ResultInfo(tabletDeviceNumber, tableStatus);

            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.ResultData.BoardNumber, $"{tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.ResultData.BoardNumber, $"{tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}")}";
            }
            else
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.ResultData.BoardNumber, $"NS {tableStatus.RoundData.NumberNorth}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.ResultData.BoardNumber, $"EW {tableStatus.RoundData.NumberEast}")}";
            }
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabledAndBack;
            ViewData["Title"] = $"Enter Tricks Taken - {tabletDeviceStatus.Location}";
            if (Settings.EnterResultsMethod == 1)
            {
                return View("TotalTricks", resultInfo);
            }
            else
            {
                return View("TricksPlusMinus", resultInfo);
            }
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber, int numTricks)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            Result result = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber).ResultData;
            result.TricksTakenNumber = numTricks;
            result.CalculateScore();
            return RedirectToAction("Index", "ConfirmResult", new { tabletDeviceNumber });
        }

        public ActionResult BackButtonClick(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            if (Settings.EnterLeadCard)
            {
                return RedirectToAction("Index", "EnterLead", new { tabletDeviceNumber, leadValidation = LeadValidationOptions.NoWarning });
            }
            else
            {
                return RedirectToAction("Index", "EnterContract", new { tabletDeviceNumber, boardNumber = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber).ResultData.BoardNumber });
            }
        }
    }
}