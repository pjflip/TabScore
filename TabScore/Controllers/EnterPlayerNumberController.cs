// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterPlayerNumberController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, Direction direction)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
            ViewData["Title"] = $"Enter Player Number - {tabletDeviceStatus.Location}";
            ViewData["Header"] = $"{tabletDeviceStatus.Location}";
            ViewData["TimerSeconds"] = -1;
            EnterPlayerNumber enterPlayerNumber = new EnterPlayerNumber()
            {
                TabletDeviceNumber = tabletDeviceNumber,
                Direction = direction,
                DisplayDirection = Enum.GetName(typeof(Direction), direction)
            };
            return View(enterPlayerNumber);
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber, Direction direction, int playerNumber)
        {
            // Update Round with new player
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            tableStatus.RoundData.UpdatePlayer(tabletDeviceStatus.SectionID, tabletDeviceStatus.TableNumber, direction, tabletDeviceStatus.RoundNumber, playerNumber);
            
            return RedirectToAction("Index", "ShowPlayerNumbers", new { tabletDeviceNumber });
        }
    }
}