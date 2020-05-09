// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNumberController : Controller
    {
        public ActionResult Index() 
        {
            SessionData sessionData = Session["SessionData"] as SessionData;
            Session["Header"] = $"Section {sessionData.SectionLetter}";
            ViewData["BackButton"] = "FALSE"; 
            return View(sessionData);   // At this stage, TableNumber > 0 means we've already tried to log on and need to confirm
        }

        public ActionResult OKButtonClick(int tableNumber, string confirm)
        {
            SessionData sessionData = Session["SessionData"] as SessionData;
            sessionData.TableNumber = tableNumber;

            if (confirm == "FALSE")
            {
                if (sessionData.TableLogonStatus() == 1)  // Table is already logged on, so need to confirm
                {
                    Session["SessionData"] = sessionData;   // Save table number
                    return RedirectToAction("Index", "EnterTableNumber");
                }
                else
                {
                    sessionData.LogonTable();
                }
            }

            sessionData.SectionTableString = sessionData.SectionLetter + tableNumber.ToString();   // Concatenate valid table number to section letter to give eg "A1", and save
            Session["SessionData"] = sessionData;    
            
            // Check if results data exist - set round number accordingly and create round info
            int lastRoundWithResults = UtilityFunctions.GetLastRoundWithResults(sessionData.SectionID, tableNumber);
            Session["Round"] = new Round(sessionData, lastRoundWithResults);

            if (lastRoundWithResults == 1 || Settings.NumberEntryEachRound)
            {
                return RedirectToAction("Index", "ShowPlayerNumbers");
            }
            else
            {
                return RedirectToAction("Index", "ShowRoundInfo");
            }
        }
    }
}