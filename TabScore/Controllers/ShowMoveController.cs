using TabScore.Models;
using System.Web.Mvc;
using System;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index(int newRoundNumber)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Section section = Session["Section"] as Section;
            if (newRoundNumber > UtilityFunctions.NumberOfRoundsInEvent(DBConnectionString, section.ID))  // Session complete
            {
                if (Settings.GetSetting<int>(DBConnectionString, SettingName.ShowRanking) == 2)
                {
                    return RedirectToAction("Index", "ShowFinalRankingList");
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen");
                }
            }

            int tableNumber = Convert.ToInt32(Session["TableNumber"]);
            bool individual = Convert.ToBoolean(Session["IndividualEvent"]);
            Round round = Session["Round"] as Round;
            MoveView moveView = new MoveView
            {
                RoundNumber = newRoundNumber
            };

            if (individual)
            {
                if (round.PairNS != 0)
                {
                    Move move = new Move(DBConnectionString, section.ID, newRoundNumber, round.PairNS);
                    moveView.PairNS = round.PairNS;
                    moveView.NorthNewTable = move.TableNumber;
                    moveView.NorthNewDirection = move.Direction;
                    moveView.NorthStay = (move.TableNumber == tableNumber && move.Direction == "North");
                }
                if (round.PairEW != 0)
                {
                    Move move = new Move(DBConnectionString, section.ID, newRoundNumber, round.PairEW);
                    moveView.PairEW = round.PairEW;
                    moveView.EastNewTable = move.TableNumber;
                    moveView.EastNewDirection = move.Direction;
                    moveView.EastStay = (move.TableNumber == tableNumber && move.Direction == "East");
                }
                if (round.South != 0)
                {
                    Move move = new Move(DBConnectionString, section.ID, newRoundNumber, round.South);
                    moveView.South = round.South;
                    moveView.SouthNewTable = move.TableNumber;
                    moveView.SouthNewDirection = move.Direction;
                    moveView.SouthStay = (move.TableNumber == tableNumber && move.Direction == "South");
                }
                if (round.West != 0)
                {
                    Move move = new Move(DBConnectionString, section.ID, newRoundNumber, round.West);
                    moveView.West = round.West;
                    moveView.WestNewTable = move.TableNumber;
                    moveView.WestNewDirection = move.Direction;
                    moveView.WestStay = (move.TableNumber == tableNumber && move.Direction == "West");
                }
            }
            else
            {
                if (round.PairNS != 0)
                {
                    Move move = new Move(DBConnectionString, section.ID, newRoundNumber, round.PairNS, "NS");
                    moveView.PairNS = round.PairNS;
                    moveView.NorthNewTable = move.TableNumber;
                    moveView.NorthNewDirection = move.Direction;
                    moveView.NorthStay = (move.TableNumber == tableNumber && move.Direction == "NS");
                }
                if (round.PairEW != 0)
                {
                    Move move = new Move(DBConnectionString, section.ID, newRoundNumber, round.PairEW, "EW");
                    moveView.PairEW = round.PairEW;
                    moveView.EastNewTable = move.TableNumber;
                    moveView.EastNewDirection = move.Direction;
                    moveView.EastStay = (move.TableNumber == tableNumber && move.Direction == "EW");
                }
            }

            moveView.LowBoard = round.LowBoard;
            moveView.HighBoard = round.HighBoard;
            BoardMove boardMove = new BoardMove(DBConnectionString, section.ID, newRoundNumber, tableNumber, round.LowBoard);
            moveView.BoardsNewTable = boardMove.TableNumber;

            Session["Header"] = $"Table {section.Letter}{Session["TableNumber"]}";
            ViewData["BackButton"] = "FALSE";

            moveView.NSMissing = (round.PairNS == 0 || round.PairNS == section.MissingPair);
            moveView.EWMissing = (round.PairEW == 0 || round.PairEW == section.MissingPair);

            // Update session to new round info
            Session["Round"] = new Round(DBConnectionString, section.ID, tableNumber, newRoundNumber, individual);

            if (individual)
            {
                return View("Individual", moveView);
            }
            else
            {
                return View("Pair", moveView);
            }
        }

        public ActionResult OKButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (Settings.GetSetting<bool>(DBConnectionString, SettingName.NumberEntryEachRound))
            {
                return RedirectToAction("Index", "ShowPlayerNumbers");
            }
            else
            {
                return RedirectToAction("Index", "ShowRoundInfo");
            }
        }
    }
}