// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRoundInfoController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];

            ViewData["Title"] = $"Show Round Info - {tabletDeviceStatus.Location}";
            ViewData["Header"] = $"{tabletDeviceStatus.Location}";
            if (tabletDeviceStatus.TableNumber == 0)
            {
                SitoutRoundInfo sitoutRoundInfo = new SitoutRoundInfo(tabletDeviceNumber);
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
                return View("Sitout", sitoutRoundInfo);
            }

            // Update player names if not just immediately done in ShowPlayerNumbers
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            if (tabletDeviceStatus.NamesUpdateRequired) tableStatus.RoundData.UpdateNames(tableStatus);
            tabletDeviceStatus.NamesUpdateRequired = true;

            RoundInfo roundInfo = new RoundInfo(tabletDeviceNumber, tableStatus);
            Section section = AppData.SectionsList.Find(x => x.SectionID == tabletDeviceStatus.SectionID);
            if (tableStatus.RoundData.NumberNorth == 0 || tableStatus.RoundData.NumberNorth == section.MissingPair)
            {
                tableStatus.ReadyForNextRoundNorth = true;
                tableStatus.ReadyForNextRoundSouth = true;
                roundInfo.NSMissing = true;
            }
            else if (tableStatus.RoundData.NumberEast == 0 || tableStatus.RoundData.NumberEast == section.MissingPair)
            {
                tableStatus.ReadyForNextRoundEast = true;
                tableStatus.ReadyForNextRoundWest = true;
                roundInfo.EWMissing = true;
            }

            if (tabletDeviceStatus.RoundNumber == 1 || section.TabletDevicesPerTable > 1)
            {
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            }
            else
            {
                // Back button needed if one tablet device per table, in case EW need to check their move details 
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabledAndBack;
            }
            if (AppData.IsIndividual)
            {
                return View("Individual", roundInfo);
            }
            else
            {
                return View("Pair", roundInfo);
            }
        }

        public ActionResult BackButtonClick(int tabletDeviceNumber)
        {
            // Only for one tablet device per table.  Reset to the previous round; RoundNumber > 1 else no Back button and cannot get here
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            tabletDeviceStatus.RoundNumber--;
            tableStatus.RoundNumber--;
            tableStatus.RoundData =  new Round(tableStatus);
            return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber = tabletDeviceStatus.RoundNumber + 1 });
        }
    }
}