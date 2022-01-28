// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterLeadController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, LeadValidationOptions leadValidation)
        {
            if (!Settings.EnterLeadCard)
            {
                return RedirectToAction("Index", "EnterTricksTaken", new { tabletDeviceNumber });
            }

            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            if (tableStatus.ResultData.LeadCard == "")  // Lead not set, so use leadValidation value as passed to controller
            {
                tableStatus.ResultData.LeadValidation = leadValidation;
            }
            else  // Lead already set, so must be an edit (ie no validation and no warning)
            {
                tableStatus.ResultData.LeadValidation = LeadValidationOptions.NoWarning;
            }
            ResultInfo resultInfo = new ResultInfo(tabletDeviceNumber, tableStatus);

            if (Settings.ShowTimer)
            {
                DateTime StartTime = AppData.RoundStartTimesList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.RoundNumber == tabletDeviceStatus.RoundNumber).StartTime;
                int TimerSeconds = tableStatus.TotalSecondsPerRound - Convert.ToInt32(DateTime.Now.Subtract(StartTime).TotalSeconds);
                if (TimerSeconds < 0) TimerSeconds = 0;
                ViewData["TimerSeconds"] = TimerSeconds;
            }
            else
            {
                ViewData["TimerSeconds"] = -1;
            }
            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.ResultData.BoardNumber, $"{tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.ResultData.BoardNumber, $"{tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}")}";
            }
            else
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {Utilities.ColourPairByVulnerability("NS", tableStatus.ResultData.BoardNumber, $"NS {tableStatus.RoundData.NumberNorth}")} v {Utilities.ColourPairByVulnerability("EW", tableStatus.ResultData.BoardNumber, $"EW {tableStatus.RoundData.NumberEast}")}";
            }
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabledAndBack;
            ViewData["Title"] = $"Enter Lead - {tabletDeviceStatus.Location}";
            return View(resultInfo);
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber, string card)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            if (tableStatus.ResultData.LeadValidation != LeadValidationOptions.Validate || !Settings.ValidateLeadCard || Utilities.ValidateLead(tableStatus, card))
            {
                tableStatus.ResultData.LeadCard = card;
                return RedirectToAction("Index", "EnterTricksTaken", new { tabletDeviceNumber });
            }
            else
            {
                return RedirectToAction("Index", "EnterLead", new { tabletDeviceNumber, leadValidation = LeadValidationOptions.Warning });
            }
        }
    }
}