// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Reflection;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class StartScreenController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Title"] = "Start Screen";
            ViewData["Header"] = ""; 
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            ViewData["Version"] = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return View();
        }

        public ActionResult OKButtonClick()
        {
            string errorString = AppData.Refresh();
            if (errorString != "")
            {
                TempData["warningMessage"] = errorString;
                return RedirectToAction("Index", "StartScreen");
            }
            Settings.Refresh();
            Utilities.SetTabletDevicesPerTable();
            if (Settings.ShowHandRecord || Settings.ValidateLeadCard)
            {
                HandRecords.Refresh();
            }

            return RedirectToAction("Index", "SelectSection");
        }
    }
}