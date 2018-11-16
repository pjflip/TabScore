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
                return RedirectToAction("Index", "EnterTableNo");
            }
            else
            // Get Section
            {
                ViewBag.Header = "";
                ViewData["CancelButton"] = "FALSE";
                return View(sectionsList);
            }
        }

        public ActionResult OKButtonClick(string s)
        {
            List<SectionClass> sectionsList = new List<SectionClass>();
            sectionsList = Sections.GetSections(Session["DBConnectionString"].ToString());

            Session["SectionLetter"] = s;
            SectionClass sect = sectionsList.Find(x => x.Letter == s);
            Session["SectionID"] = sect.ID.ToString();
            Session["NumTables"] = sect.Tables.ToString();
            Session["MissingPair"] = sect.MissingPair.ToString();
            Session["Winners"] = sect.Winners.ToString();
            return RedirectToAction("Index", "EnterTableNo");
        }

        public ActionResult ControlButtonClick()
        {
            Session["ControlReturnScreen"] = "EnterSection";
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}