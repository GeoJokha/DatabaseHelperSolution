using System;
using System.Data;

namespace DatabaseHelper.Common
{
    public abstract class DatabaseCommon<TConnection, TCommand, TParameter, TTransaction> : IDisposable
        where TConnection : IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TParameter : IDbDataParameter
        where TTransaction : IDbTransaction
    {
        private readonly string _connectionString;
        protected TConnection? _connection;
        protected TTransaction? _transaction;

        protected DatabaseCommon(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public TConnection GetConnection() => _connection ??= (TConnection)Activator.CreateInstance(typeof(TConnection), _connectionString);

        public TConnection OpenConnection()
        {
            GetConnection().Open();
            return _connection!;
        }

        public void CloseConnection() => _connection?.Close();

        public TCommand GetCommand(string commandText, CommandType commandType, params TParameter[] parameters)
        {
            TCommand command = new TCommand
            {
                Connection = _connection,
                CommandText = commandText,
                CommandType = commandType
            };
            foreach (var param in parameters)
            {
                command.Parameters.Add(param);
            }
            if (_transaction != null)
            {
                command.Transaction = _transaction;
            }

            return command;
        }

        public TCommand GetCommand(string commandText, params TParameter[] parameters) =>
            GetCommand(commandText, CommandType.Text, parameters);

        public int ExecuteNonQuery(string commandText, CommandType commandType, params TParameter[] parameters)
        {
            using TCommand command = GetCommand(commandText, commandType, parameters);
            return command.ExecuteNonQuery();
        }

        public int ExecuteNonQuery(string commandText, params TParameter[] parameters) =>
            ExecuteNonQuery(commandText, CommandType.Text, parameters);

        public object? ExecuteScalar(string commandText, CommandType commandType, params TParameter[] parameters)
        {
            using TCommand command = GetCommand(commandText, commandType, parameters);
            return command.ExecuteScalar();
        }

        public object? ExecuteScalar(string commandText, params TParameter[] parameters) =>
            ExecuteScalar(commandText, CommandType.Text, parameters);

        public IDataReader ExecuteReader(string commandText, CommandType commandType, params TParameter[] parameters)
        {
            using TCommand command = GetCommand(commandText, commandType, parameters);
            return command.ExecuteReader();
        }

        public IDataReader ExecuteReader(string commandText, params TParameter[] parameters) =>
            ExecuteReader(commandText, CommandType.Text, parameters);
        protected abstract IDataAdapter CreateDataAdapter(IDbCommand command);

        public TTransaction BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Transaction already started.");
            }
            _transaction = (TTransaction)GetConnection().BeginTransaction();
            return _transaction;
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction not started.");
            }
            _transaction.Commit();
            _transaction = default;
        }

        public void RollbackTransaction()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction not started.");
            }
            _transaction.Rollback();
            _transaction = default;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
            _transaction = default;
            _connection = default;
        }
    }
}
