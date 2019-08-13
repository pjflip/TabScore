using System.Collections.Generic;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterSectionController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "ErrorScreen");
            }

            List<SectionClass> sectionsList = new List<SectionClass>();
            sectionsList = Sections.GetSections(DBConnectionString);

            // Check if only one section - if so use it
            if (sectionsList.Count == 1)
            {
                Session["SectionLetter"] = sectionsList[0].Letter;
                Session["SectionID"] = sectionsList[0].ID.ToString();
                Session["NumTables"] = sectionsList[0].Tables.ToString();
                Session["MissingPair"] = sectionsList[0].MissingPair.ToString();
                return RedirectToAction("Index", "EnterTableNumber");
            }
            else
            // Get Section
            {
                ViewBag.Header = "";
                ViewData["BackButton"] = "FALSE";
                return View(sectionsList);
            }
        }

        public ActionResult OKButtonClick(string sectionLetter)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "ErrorScreen");
            }

            List<SectionClass> sectionsList = Sections.GetSections(DBConnectionString);
            Session["SectionLetter"] = sectionLetter;
            SectionClass section = sectionsList.Find(x => x.Letter == sectionLetter);
            Session["SectionID"] = section.ID.ToString();
            Session["NumTables"] = section.Tables.ToString();
            Session["MissingPair"] = section.MissingPair.ToString();
            return RedirectToAction("Index", "EnterTableNumber");
        }
    }
}