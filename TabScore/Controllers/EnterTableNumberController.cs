// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNumberController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber) 
        {
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            ViewData["Title"] = $"Enter Table Number - Section {section.SectionLetter}";
            ViewData["Header"] = $"Section {section.SectionLetter}";
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
            EnterTableNumber tableNumberData = new EnterTableNumber
            {
                SectionID = sectionID,
                TableNumber = tableNumber,   // At this stage, TableNumber > 0 means we've already tried to log on and need to confirm
                NumTables = section.NumTables
            };
            return View(tableNumberData);   
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, string confirm)
        {
            if (confirm == "FALSE")
            {
                if (Utilities.TableLogonStatus(sectionID, tableNumber) == 1)  // Table is already logged on, so need to confirm
                {
                    return RedirectToAction("Index", "EnterTableNumber", new { sectionID, tableNumber });
                }
                else
                {
                    Utilities.LogonTable(sectionID, tableNumber);
                }
            }

            // Check if results data exist - set round number accordingly, create round info and table status
            int lastRoundWithResults = Utilities.GetLastRoundWithResults(sectionID, tableNumber);
            Round round = new Round(sectionID, tableNumber, lastRoundWithResults);
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus == null)
            {
                tableStatus = new TableStatus(sectionID, tableNumber, round);
                AppData.TableStatusList.Add(tableStatus);
            }
            else
            {
                tableStatus.RoundData = round;
            }

            if (lastRoundWithResults == 1 || Settings.NumberEntryEachRound)
            {
                return RedirectToAction("Index", "ShowPlayerNumbers", new { sectionID, tableNumber });
            }
            else
            {
                return RedirectToAction("Index", "ShowRoundInfo", new { sectionID, tableNumber });
            }
        }
    }
}