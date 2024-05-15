using Microsoft.Data.SqlClient;
using DatabaseHelper.Common;
using System.Data;

namespace DatabaseHelper.SQL
{
    public sealed class DatabaseSQL : DatabaseCommon<SqlConnection, SqlCommand, SqlParameter, SqlTransaction>
    {
        public DatabaseSQL(string connectionString) : base(connectionString) { }

        protected override IDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new SqlDataAdapter((SqlCommand)command);
        }
    }
}
