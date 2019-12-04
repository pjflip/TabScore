using System;
using System.Web.Mvc;
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
                Session["SectionLetter"] = sectionsList[0].Letter;
                Session["SectionID"] = sectionsList[0].ID;
                Session["NumTables"] = sectionsList[0].Tables;
                Session["MissingPair"] = sectionsList[0].MissingPair;
                Session["MaxRounds"] = UtilityFunctions.NumberOfRoundsInEvent(DBConnectionString, sectionsList[0].ID);
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

            Session["SectionLetter"] = sectionLetter;
            Section section = sectionsList.Find(x => x.Letter == sectionLetter);
            Session["SectionID"] = section.ID;
            Session["NumTables"] = section.Tables;
            Session["MissingPair"] = section.MissingPair;
            Session["MaxRounds"] = UtilityFunctions.NumberOfRoundsInEvent(DBConnectionString, section.ID);

            return RedirectToAction("Index", "EnterTableNumber");
        }
    }
}