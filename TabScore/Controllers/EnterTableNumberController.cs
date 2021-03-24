// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNumberController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber = 0, bool confirm = false) 
        {
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            ViewData["Title"] = $"Enter Table Number - Section {section.SectionLetter}";
            ViewData["Header"] = $"Section {section.SectionLetter}";
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
            EnterTableNumberDirection tableNumberData = new EnterTableNumberDirection
            {
                SectionID = sectionID,
                TableNumber = tableNumber,
                NumTables = section.NumTables,
                Confirm = confirm
            };
            return View(tableNumberData);   
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, bool confirm)
        {
            // Log on table in database
            Utilities.LogonTable(sectionID, tableNumber);

            // Check if table status has already been created.  If not, get round info and set table status accordingly
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus == null)
            {
                int lastRoundWithResults = Utilities.GetLastRoundWithResults(sectionID, tableNumber);
                Round round = new Round(sectionID, tableNumber, lastRoundWithResults);
                tableStatus = new TableStatus(sectionID, tableNumber, round);
                AppData.TableStatusList.Add(tableStatus);
            }

            if (AppData.SectionsList.Find(x => x.SectionID == sectionID).TabletDevicesPerTable == 1)
            {
                // One tablet device per table, so direction is North
                TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber && x.Direction == "North");

                // Check if tablet device is already registered for this location, and if so confirm
                if (tabletDeviceStatus != null && !confirm)
                {
                    return RedirectToAction("Index", "EnterTableNumber", new { sectionID, tableNumber, confirm = true });
                }
                else if (tabletDeviceStatus == null)  
                {
                    // Not on list, so need to add it
                    tabletDeviceStatus = new TabletDeviceStatus(sectionID, tableNumber, "North", 0, tableStatus.RoundData.RoundNumber);
                    AppData.TabletDeviceStatusList.Add(tabletDeviceStatus);
                }
                
                // tabletDeviceNumber is the key for identifying this particular tablet device and is used throughout the rest of the application
                int tabletDeviceNumber = AppData.TabletDeviceStatusList.LastIndexOf(tabletDeviceStatus);

                if (tableStatus.ReadyForNextRoundNorth)
                {
                    return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNummber = tableStatus.RoundData.RoundNumber + 1 });
                }
                else if (tableStatus.RoundData.RoundNumber == 1 || Settings.NumberEntryEachRound)
                {
                    return RedirectToAction("Index", "ShowPlayerNumbers", new { tabletDeviceNumber });
                }
                else
                {
                    return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
                } 
            }
            else   // TabletDevicesPerTable > 1
            {
                return RedirectToAction("Index", "EnterDirection", new { sectionID, tableNumber });
            }
        }
    }
}