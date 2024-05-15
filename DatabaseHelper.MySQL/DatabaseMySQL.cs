using MySql.Data.MySqlClient;
using DatabaseHelper.Common;
using System.Data;

namespace DatabaseHelper.MySQL
{
    public sealed class DatabaseMySQL : DatabaseCommon<MySqlConnection, MySqlCommand, MySqlParameter, MySqlTransaction>
    {
        public DatabaseMySQL(string connectionString) : base(connectionString) { }

        protected override IDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new MySqlDataAdapter((MySqlCommand)command);
        }
    }
}
