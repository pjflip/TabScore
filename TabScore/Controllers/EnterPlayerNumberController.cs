// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterPlayerNumberController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, string direction)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            EnterPlayerNumber enterPlayerNumber = new EnterPlayerNumber
            {
                SectionID = tableStatus.SectionID,
                TableNumber = tableStatus.TableNumber,
                Direction = direction
            };
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
            ViewData["Title"] = $"Enter Player Number - {tableStatus.SectionTableString} {direction}";
            ViewData["Header"] = $"Table {tableStatus.SectionTableString}";
            return View(enterPlayerNumber);
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, string direction, int playerNumber)
        {
            // Update Round with new player
            AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber).RoundData.UpdatePlayer(sectionID, tableNumber, direction, playerNumber);
            
            return RedirectToAction("Index", "ShowPlayerNumbers", new { sectionID, tableNumber });
        }
    }
}