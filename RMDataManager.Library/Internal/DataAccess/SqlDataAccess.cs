using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace RMDataManager.Library.Internal.DataAccess
{
    /*
     * "Internal" makes sure that the database can't be accessed directly from outside this class library.
     *
     * The aim of this class is to provide the necessary data for the methods in UserData.cs, which talks
     * to the database in order to get back the information about users.
     */
    internal class SqlDataAccess : IDisposable
    {
        // Gets the connection string with the matching name from WebConfig
        // (DefaultConnection) and returns it back.
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        // Loading data from the database using GetConnectionString().
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Connects the database, makes a query and returns back a set of rows.
                var rows = connection
                    .Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Stores data into the database
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        // Open Connection / Start Transaction 
        public void StartTransaction(string connectionStringName)
        {
            _connection = new SqlConnection();
            _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            // Connects the database, makes a query and returns back a set of rows.
                var rows = _connection
                    .Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, 
                        transaction: _transaction).ToList();

                return rows;
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            // Stores data into the database
            _connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction:_transaction);
        }

        // Commit Transaction
        public void CommitTransaction()
        {
            _transaction?.Commit();
        }

        // Rollback transaction in case of failing
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection.Close();
        }
        // Load using the transaction

        // Save using the transaction

        // Close transaction / Stop Transaction Method()

        // Dispose

        public void Dispose()
        {
            CommitTransaction();
        }
    }
}
