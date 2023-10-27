// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;
using Resources;

namespace TabScore.Controllers
{
    public class EnterTricksTakenController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            if (tableStatus.ResultData == null)  // Probably from browser 'Back' button.  Don't know boardNumber so go to ShowBoards
            {
                return RedirectToAction("Index", "ShowBoards", new { tabletDeviceNumber });
            }
            ResultInfo resultInfo = new ResultInfo(tabletDeviceNumber, tableStatus);

            if (Settings.ShowTimer) ViewData["TimerSeconds"] = Utilities.SetTimerSeconds(tabletDeviceStatus);
            ViewData["Title"] = $"{Strings.EnterTricksTaken} - {tabletDeviceStatus.Location}";
            ViewData["Header"] = Utilities.HeaderString(tabletDeviceStatus, tableStatus);
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabledAndBack;
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