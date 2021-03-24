// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterSectionController : Controller
    {
        public ActionResult Index()
        {
            // Check if only one section - if so use it
            if (AppData.SectionsList.Count == 1)
            {
                return RedirectToAction("Index", "EnterTableNumber", new { sectionID = AppData.SectionsList[0].SectionID });
            }
            else
            // Get section
            {
                ViewData["Title"] = "Enter Section";
                ViewData["Header"] = "";
                ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
                return View(AppData.SectionsList);
            }
        }
    }
}