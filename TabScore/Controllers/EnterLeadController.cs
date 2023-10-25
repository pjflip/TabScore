// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;
using Resources;

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

            if (Settings.ShowTimer) ViewData["TimerSeconds"] = Utilities.SetTimerSeconds(tabletDeviceStatus);
            ViewData["Title"] = $"{Strings.EnterLead} - {tabletDeviceStatus.Location}";
            ViewData["Header"] = Utilities.HeaderString(tabletDeviceStatus, tableStatus);
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabledAndBack;
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