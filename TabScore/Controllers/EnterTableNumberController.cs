// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
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
            EnterTableNumber enterTableNumber = new EnterTableNumber(section, tableNumber, confirm);
            ViewData["Title"] = $"Enter Table Number - Section {section.SectionLetter}";
            ViewData["Header"] = $"Section {section.SectionLetter}";
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
            ViewData["TimerSeconds"] = -1;
            return View(enterTableNumber);   
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, bool confirm)
        {
            // Register table in database
            Utilities.RegisterTable(sectionID, tableNumber);
            
            // Check if table status has already been created; if not, add it to the list
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus == null)
            {
                tableStatus = new TableStatus(sectionID, tableNumber, Utilities.GetLastRoundWithResults(sectionID));
                AppData.TableStatusList.Add(tableStatus);
            }
            
            if (AppData.SectionsList.Find(x => x.SectionID == sectionID).TabletDevicesPerTable == 1)
            {
                // One tablet device per table
                TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);

                // Check if tablet device is already registered for this location, and if so confirm
                if (tabletDeviceStatus != null && !confirm)
                {
                    return RedirectToAction("Index", "EnterTableNumber", new { sectionID, tableNumber, confirm = true });
                }
                else if (tabletDeviceStatus == null)  
                {
                    // Not on list, so need to add it.  Direction is fixed as North as only one tablet per table
                    tabletDeviceStatus = new TabletDeviceStatus(sectionID, tableNumber, Direction.North, tableStatus.RoundData.NumberNorth, tableStatus.RoundNumber);
                    AppData.TabletDeviceStatusList.Add(tabletDeviceStatus);
                }
                
                // tabletDeviceNumber is the key for identifying this particular tablet device and is used throughout the rest of the application
                int tabletDeviceNumber = AppData.TabletDeviceStatusList.LastIndexOf(tabletDeviceStatus);

                if (tableStatus.ReadyForNextRoundNorth)
                {
                    return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNummber = tableStatus.RoundNumber + 1 });
                }
                else if (tabletDeviceStatus.RoundNumber == 1 || Settings.NumberEntryEachRound)
                {
                    return RedirectToAction("Index", "ShowPlayerNumbers", new { tabletDeviceNumber });
                }
                else
                {
                    return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
                } 
            }
            else   // More than one tablet device per table, so need to know direction for this tablet device
            {
                return RedirectToAction("Index", "EnterDirection", new { sectionID, tableNumber, tableStatus.RoundNumber });
            }
        }
    }
}