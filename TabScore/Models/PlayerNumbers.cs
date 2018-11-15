using System.Data.Odbc;

namespace TabScore.Models
{
    public static class PlayerNumbers
    {
        public static void UpdateNameNumber(string DB, string sectionID, string table, string direction, string number, string name) 
        {     
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                name = name.Replace("'", "''");
                string SQLString = $"UPDATE PlayerNumbers SET [Number]='{number}', [Name]='{name}' WHERE Section={sectionID} AND [Table]={table} AND Direction='{direction}'";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
    }
}