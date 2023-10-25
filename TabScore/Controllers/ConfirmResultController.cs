// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;
using Resources;

namespace TabScore.Controllers
{
    public class ConfirmResultController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            ResultInfo resultInfo = new ResultInfo(tabletDeviceNumber, tableStatus);

            if (Settings.ShowTimer) ViewData["TimerSeconds"] = Utilities.SetTimerSeconds(tabletDeviceStatus);
            ViewData["Title"] = $"{Strings.ConfirmResult} - {tabletDeviceStatus.Location}";
            ViewData["Header"] = Utilities.HeaderString(tabletDeviceStatus, tableStatus);
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabledAndBack;
            return View(resultInfo);
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            tableStatus.ResultData.UpdateDB(tableStatus);
            return RedirectToAction("Index", "ShowTraveller", new { tabletDeviceNumber, boardNumber = tableStatus.ResultData.BoardNumber });
        }

        public ActionResult BackButtonClick(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            Result result = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber).ResultData;
            if (result.ContractLevel == 0)  // This was passed out, so Back goes all the way to Enter Contract screen
            {
                return RedirectToAction("Index", "EnterContract", new { tabletDeviceNumber, boardNumber = result.BoardNumber });
            }
            else
            {
                return RedirectToAction("Index", "EnterTricksTaken", new { tabletDeviceNumber });
            }
        }
    }
}
