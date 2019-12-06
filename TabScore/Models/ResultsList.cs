using System.Collections.Generic;

namespace TabScore.Models
{
    public class ResultsList : List<Result>
    {
        public bool GotAllResults { get; private set; }

        public ResultsList(string DB, int sectionID, int table, int roundNumber, int lowBoard, int highBoard)
        {
            int resultCount = 0;
            for (int i = lowBoard; i <= highBoard; i++)
            {
                Result result = new Result
                {
                    SectionID = sectionID,
                    TableNumber = table,
                    RoundNumber = roundNumber,
                    BoardNumber = i,
                    ContractLevel = -1
                };
                result.ReadFromDB(DB);
                Add(result);
                if (result.ContractLevel > -1) resultCount++;
            }
            GotAllResults = (resultCount == highBoard - lowBoard + 1);
        }
    }
}