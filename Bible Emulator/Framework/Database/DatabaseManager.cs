using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bible;
using MySql.Data.MySqlClient;
using Logging;
using System.Data.SqlClient;

namespace Bible.Framework.DatabaseManager
{
    public class Database
    {
        private MySqlConnection Connection;
        private string ConnectionString;

        public MySqlConnection GetConnection
        {
            get { return Connection; }
        }

        public Database(string Hostname, uint PortBase, string Username, string Password, string DatabaseName, uint MinPoolSize, uint MaxPoolSize)
        {
            try
            {
                MySqlConnectionStringBuilder stringBuilder = new MySqlConnectionStringBuilder
                {
                    Server = Hostname,
                    Port = PortBase,
                    UserID = Username,
                    Password = Password,
                    Database = DatabaseName,
                    MinimumPoolSize = MinPoolSize,
                    MaximumPoolSize = MaxPoolSize,
                    AllowZeroDateTime = true,
                    ConvertZeroDateTime = true,
                    DefaultCommandTimeout = 300,
                    ConnectionTimeout = 10
                };

                SetConnectionString(stringBuilder.ToString());
                Logging.ActionLogger.writeInfo("MySQL Configuration set");

                Connection = new MySqlConnection(GetConnectionString());
                Connection.Open();

                if (Connection != null)
                {
                    Logging.ActionLogger.writeInfo("Connected to MySQL database");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Logging.ActionLogger.writeError("Failed to connect to MySQL database. Please check the configuration file. Error shown below:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(e.ToString());
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        private void SetConnectionString(string newString)
        {
            ConnectionString = newString;  
        }

        private string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}
