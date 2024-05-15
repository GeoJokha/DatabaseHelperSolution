using DatabaseHelper.MySQL;
using DatabaseHelper.SQL;
using MySql.Data.MySqlClient;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string mysqlConnectionString = "Server=DESKTOP-L9NGGNR;Database=NorthWind;Integrated Security=True; TrustServerCertificate = true;";
        string sqlConnectionString = "Server=DESKTOP-L9NGGNR;Database=NorthWind;Integrated Security=True; TrustServerCertificate = true;";

        using var mysqlDb = new DatabaseSQL(mysqlConnectionString);
        mysqlDb.OpenConnection();
        
        mysqlDb.CloseConnection();

        using var sqlDb = new DatabaseSQL(sqlConnectionString);
        sqlDb.OpenConnection();
        
        sqlDb.CloseConnection();
    }
}
