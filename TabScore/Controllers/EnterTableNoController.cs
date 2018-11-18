using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNoController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Header = $"Section {Session["SectionLetter"]}";
            ViewData["CancelButton"] = "FALSE";
            ViewData["NumTables"] = Session["NumTables"];
            return View();
        }

        public ActionResult OKButtonClick(string tn, string confirm)
        {
            if (confirm == "TRUE")
            {
                Session["Table"] = tn;
                Tables.LogonTable(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), tn);
                return RedirectToAction("Index", "ShowPlayerNos");
            }
            else
            {
                if (Tables.CheckTableLogonStatus(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), tn))
                {
                    TempData["LoggedOnTable"] = tn;
                    return RedirectToAction("Index", "EnterTableNo");
                }
                else
                {
                    Session["Table"] = tn;
                    Tables.LogonTable(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), tn);
                    return RedirectToAction("Index", "ShowPlayerNos");
                }
            }
        }
    }
}
