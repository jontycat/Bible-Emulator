using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Logging;
using log4net.Config;
using System.Windows.Forms;
using Networking;
using Networking.Sessions;
using Messages;
using Bible.Framework.DatabaseManager;
using MySql.Data.MySqlClient;

namespace Bible
{
    public class BibleEnvironment
    {
        public NetworkServer ClientServer
        {
            get { return mServer; }
        }

        public SessionManager SessionManager
        {
            get { return mSessionManager; }
        }

        public MessageHandler MessageHandler
        {
            get { return mMessageHandler; }
        }

        public Database DatabaseManager
        {
            get { return mDatabaseManager; }
        }

        private NetworkServer mServer = null;
        private SessionManager mSessionManager = null;
        private MessageHandler mMessageHandler = null;
        private Database mDatabaseManager = null;

        private Dictionary<string, string> ConfigurationData = new Dictionary<string, string>();

        private int ListenPort;
        private int MaxConnections;
        private int MaxAcceptConnections;
        private int MaxReceiveConnections;

        private string DatabaseHost;
        private uint DatabasePort;
        private string DatabaseUser;
        private string DatabasePass;
        private string DatabaseName;
        private uint MinPoolSize;
        private uint MaxPoolSize;

        public int test = 1;

        public BibleEnvironment()
        {
            LoadConfigurationFile();
            ListenPort = Convert.ToInt16(GetConfigurationValue("tcp.bindport"));
            MaxConnections = Convert.ToInt16(GetConfigurationValue("tcp.maxconnections"));
            MaxAcceptConnections = Convert.ToInt16(GetConfigurationValue("tcp.maxaccepts"));
            MaxReceiveConnections = Convert.ToInt16(GetConfigurationValue("tcp.maxrecv"));
            DatabaseHost = GetConfigurationValue("mysql.hostname");
            DatabasePort = Convert.ToUInt16(GetConfigurationValue("mysql.port"));
            DatabaseUser = GetConfigurationValue("mysql.username");
            DatabasePass = GetConfigurationValue("mysql.password");
            DatabaseName = GetConfigurationValue("mysql.database");
            MinPoolSize = Convert.ToUInt16(GetConfigurationValue("pooling.minsize"));
            MaxPoolSize = Convert.ToUInt16(GetConfigurationValue("pooling.maxsize"));
            ConfigurationData.Clear();

            XmlConfigurator.Configure(new System.IO.FileInfo(Application.StartupPath + @"\appender.xml"));
            mServer = new NetworkServer(ListenPort, MaxConnections, MaxAcceptConnections, MaxReceiveConnections);
            mSessionManager = new SessionManager();
            mMessageHandler = new MessageHandler();
            mDatabaseManager = new Database(DatabaseHost, DatabasePort, DatabaseUser, DatabasePass, DatabaseName, MinPoolSize, MaxPoolSize);

            mServer.StartListening();

            if (mServer.IsListening)
            {
                ActionLogger.writeInfo("Listening for connections on port " + ListenPort);
            }
            else
            {
                Console.ReadKey();
                Environment.Exit(1);
                return;
            }
        }

        private void LoadConfigurationFile()
        {
            try
            {
                StreamReader Configuration = new StreamReader(@"./config.ini");
                if (File.Exists(@"./config.ini"))
                {                   
                    do
                    {
                        string[] Value = Configuration.ReadLine().Split('=');
                        ConfigurationData.Add(Value[0], Value[1]);
                    }
                    while (Configuration.Peek() != -1);              
                    return;
                }
                else
                {
                    ActionLogger.writeFatal("Configuration file not found.");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }
            catch (Exception e)
            {
                ActionLogger.writeInfo(e.ToString());
            }
            return;
        }

        private string GetConfigurationValue(string Key)
        {
            string Value = String.Empty;

            if (ConfigurationData.TryGetValue(Key, out Value))
            {
                return Value;
            }
            else
            {
                ActionLogger.writeFatal("Configuration key " + Key + " not found.");
                this.Shutdown();
                return null;
            }
        }

        public void Shutdown()
        {
            Console.ReadKey();
            Environment.Exit(1);
            return;
        }
    }
}
