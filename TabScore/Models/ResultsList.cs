using System.Collections.Generic;

namespace TabScore.Models
{
    public class ResultsList : List<Result>
    {
        public bool GotAllResults { get; private set; }

        public ResultsList(string DB, int sectionID, int tableNumber, int roundNumber, int lowBoard, int highBoard)
        {
            int resultCount = 0;
            for (int i = lowBoard; i <= highBoard; i++)
            {
                Result result = new Result(DB, sectionID, tableNumber, roundNumber, i);
                Add(result);
                if (result.ContractLevel != -999) resultCount++;
            }
            GotAllResults = (resultCount == highBoard - lowBoard + 1);
        }
    }
}