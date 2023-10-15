// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;
using Resources;

namespace TabScore.Controllers
{
    public class EnterPlayerIDController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, Direction direction)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
            ViewData["Title"] = $"{Strings.EnterPlayerIDs} - {tabletDeviceStatus.Location}";
            ViewData["Header"] = $"{tabletDeviceStatus.Location}";
            EnterPlayerID enterPlayerID = new EnterPlayerID()
            {
                TabletDeviceNumber = tabletDeviceNumber,
                Direction = direction,
                DisplayDirection = Enum.GetName(typeof(Direction), direction)
            };
            return View(enterPlayerID);
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber, Direction direction, int playerID)
        {
            // Update Round with new player
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            tableStatus.RoundData.UpdatePlayer(tabletDeviceStatus.SectionID, tabletDeviceStatus.TableNumber, direction, tabletDeviceStatus.RoundNumber, playerID);
            
            return RedirectToAction("Index", "ShowPlayerIDs", new { tabletDeviceNumber });
        }
    }
}