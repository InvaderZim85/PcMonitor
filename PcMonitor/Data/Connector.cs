using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace PcMonitor.Data
{
    public static class Connector
    {
        /// <summary>
        /// Contains the connection
        /// </summary>
        private static MySqlConnection _connection;

        /// <summary>
        /// Gets the sql connection
        /// </summary>
        public static MySqlConnection Connection
        {
            get
            {
                if (_connection == null || _connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
                    CreateConnection();

                return _connection;
            }
        }

        /// <summary>
        /// Creates the connection to the database
        /// </summary>
        private static void CreateConnection()
        {
            _connection = new MySqlConnection(new MySqlConnectionStringBuilder
            {
                Server = Helper.Settings.DbServer,
                Port = (uint) Helper.Settings.DbPort,
                UserID = Helper.Settings.DbUser,
                Password = Helper.Settings.DbPassword,
                Database = Helper.Settings.DbDatabase,
                ConnectionTimeout = 5
            }.ConnectionString);

            try
            {
                _connection.Open();
                Helper.HasDatabaseConnection = true;
            }
            catch (Exception)
            {
                Helper.HasDatabaseConnection = false;
            }
        }

        /// <summary>
        /// Checks the connection
        /// </summary>
        public static void CheckConnection()
        {
            CreateConnection();
        }
    }
}
