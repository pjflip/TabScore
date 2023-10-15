// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;
using Resources;

namespace TabScore.Controllers
{
    public class EndScreenController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            ViewData["Header"] = $"{tabletDeviceStatus.Location}";
            ViewData["Title"] = $"{Strings.EndScreen} - {tabletDeviceStatus.Location}";
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            ViewData["TabletDeviceNumber"] = tabletDeviceNumber;
            return View();
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber)
        {
            // Check if new round has been added; can't apply to individuals
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            if (tabletDeviceStatus.RoundNumber == Utilities.NumberOfRoundsInEvent(tabletDeviceStatus.SectionID))  
            {
                // Final round, so no new rounds added
                return RedirectToAction("Index", "EndScreen", new { tabletDeviceNumber });
            }
            else
            {
                return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber = tabletDeviceStatus.RoundNumber + 1 });
            }
        }
    }
}