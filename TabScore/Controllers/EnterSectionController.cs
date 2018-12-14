using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterSectionController : Controller
    {
        public ActionResult Index()
        {
            List<SectionClass> sectionsList = new List<SectionClass>();
            sectionsList = Sections.GetSections(Session["DBConnectionString"].ToString());

            // Check if only one section - if so use it
            if (sectionsList.Count == 1)
            {
                Session["SectionLetter"] = sectionsList[0].Letter;
                Session["SectionID"] = sectionsList[0].ID.ToString();
                Session["NumTables"] = sectionsList[0].Tables.ToString();
                Session["MissingPair"] = sectionsList[0].MissingPair.ToString();
                Session["Winners"] = sectionsList[0].Winners.ToString();
                return RedirectToAction("Index", "EnterTableNumber");
            }
            else
            // Get Section
            {
                ViewBag.Header = "";
                ViewData["CancelButton"] = "FALSE";
                return View(sectionsList);
            }
        }

        public ActionResult OKButtonClick(string sectionLetter)
        {
            List<SectionClass> sectionsList = Sections.GetSections(Session["DBConnectionString"].ToString());

            Session["SectionLetter"] = sectionLetter;
            SectionClass section = sectionsList.Find(x => x.Letter == sectionLetter);
            Session["SectionID"] = section.ID.ToString();
            Session["NumTables"] = section.Tables.ToString();
            Session["MissingPair"] = section.MissingPair.ToString();
            Session["Winners"] = section.Winners.ToString();
            return RedirectToAction("Index", "EnterTableNumber");
        }
    }
}