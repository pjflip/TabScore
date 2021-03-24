namespace TabScore.Models
{
    public class PlayerEntry
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string Direction { get; set; }

        public PlayerEntry (Round round, string direction)
        {
            Direction = direction;
            if (direction == "North")
            {
                Name = round.NameNorth;
                Number = round.NumberNorth;
            }
            else if (direction == "East")
            {
                Name = round.NameEast;
                Number = round.NumberEast;
            }
            else if (direction == "South")
            {
                Name = round.NameSouth;
                Number = round.NumberSouth;
            }
            else
            {
                Name = round.NameWest;
                Number = round.NumberWest;
            }
        }
    }
}