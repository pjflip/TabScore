using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNumberController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Header = $"Section {Session["SectionLetter"]}";
            ViewData["CancelButton"] = "FALSE";
            ViewData["NumTables"] = Session["NumTables"];
            return View();
        }

        public ActionResult OKButtonClick(string tableNumber, string confirm)
        {
            if (confirm == "TRUE")
            {
                Session["Table"] = tableNumber;
                Tables.LogonTable(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), tableNumber);
                return RedirectToAction("Index", "ShowPlayerNumbers");
            }
            else
            {
                if (Tables.CheckTableLogonStatus(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), tableNumber))
                {
                    TempData["LoggedOnTable"] = tableNumber;
                    return RedirectToAction("Index", "EnterTableNumber");
                }
                else
                {
                    Session["Table"] = tableNumber;
                    Tables.LogonTable(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), tableNumber);
                    return RedirectToAction("Index", "ShowPlayerNumbers");
                }
            }
        }
    }
}
