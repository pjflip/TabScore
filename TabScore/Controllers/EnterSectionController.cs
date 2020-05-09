// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterSectionController : Controller
    {
        public ActionResult Index()
        {
            SectionsList sectionsList = new SectionsList();

            // Check if only one section - if so use it
            if (sectionsList.Count == 1)
            {
                Session["SessionData"] = new SessionData()
                {
                    TableNumber = 0,
                    SectionID = sectionsList[0].ID,
                    SectionLetter = sectionsList[0].Letter,
                    NumTables = sectionsList[0].NumTables,
                    MissingPair = sectionsList[0].MissingPair,
                };
                return RedirectToAction("Index", "EnterTableNumber");
            }
            else
            // Get section
            {
                ViewData["BackButton"] = "FALSE";
                return View(sectionsList);
            }
        }
        public ActionResult OKButtonClick(string sectionLetter)
        {
            SectionsList sectionsList = new SectionsList();

            Section section = sectionsList.Find(x => x.Letter == sectionLetter);
            Session["SessionData"] = new SessionData()
            {
                TableNumber = 0,
                SectionID = section.ID,
                SectionLetter = section.Letter,
                NumTables = section.NumTables,
                MissingPair = section.MissingPair,
            };
            return RedirectToAction("Index", "EnterTableNumber");
        }
    }
}