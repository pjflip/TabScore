﻿using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterSectionController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            SectionsList sectionsList = new SectionsList(DBConnectionString);
            if (sectionsList == null) return RedirectToAction("Index", "ErrorScreen");

            // Check if only one section - if so use it
            if (sectionsList.Count == 1)
            {
                Session["Section"] = sectionsList[0];
                return RedirectToAction("Index", "EnterTableNumber");
            }
            else
            // Get Section
            {
                ViewData["BackButton"] = "FALSE";
                return View(sectionsList);
            }
        }

        public ActionResult OKButtonClick(string sectionLetter)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            SectionsList sectionsList = new SectionsList(DBConnectionString);
            if (sectionsList == null) return RedirectToAction("Index", "ErrorScreen");

            Session["Section"] = sectionsList.Find(x => x.Letter == sectionLetter);

            return RedirectToAction("Index", "EnterTableNumber");
        }
    }
}