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
                Session["SessionData"] = new SessionData(DBConnectionString)
                {
                    SectionID = sectionsList[0].ID,
                    SectionLetter = sectionsList[0].Letter,
                    NumTables = sectionsList[0].NumTables,
                    MissingPair = sectionsList[0].MissingPair,
                };
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

            Section section = sectionsList.Find(x => x.Letter == sectionLetter);
            Session["SessionData"] = new SessionData(DBConnectionString)
            {
                SectionID = section.ID,
                SectionLetter = section.Letter,
                NumTables = section.NumTables,
                MissingPair = section.MissingPair,
            };
            return RedirectToAction("Index", "EnterTableNumber");
        }
    }
}