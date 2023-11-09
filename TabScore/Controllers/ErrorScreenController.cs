// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;
using Resources;

namespace TabScore.Controllers
{
    public class ErrorScreenController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Header"] = "";
            ViewData["Title"] = Strings.ErrorScreen;
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            return View();
        }

        public ActionResult OKButtonClick()
        {
           if (Utilities.IsDatabaseOK())  // Successful database read/write, so must have been a temporary glitch
            {
                Settings.Refresh();
                if (Settings.ShowHandRecord || Settings.ValidateLeadCard)
                {
                    HandRecords.Refresh();
                }
                return RedirectToAction("Index", "SelectSection");  // Need to re-establish Section/TableNumber/Direction for this tablet device
            }
            else  // Can't read/write to database after the error, so pass error to StartScreen and await database update 
            {
                AppData.PermanentDBError = true;
                TempData["warningMessage"] = Strings.ErrorPermanentDB;
                return RedirectToAction("Index", "StartScreen");
            }
        }
    }
}