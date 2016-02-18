using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.MongoAccess
{
    public class MongoUtil
    {
        #region Fields

        private static IMongoClient _client;
        private static IMongoDatabase _database;
        private static MongoUtil _instance;

        #endregion Fields

        #region Properties

        public static MongoUtil Instance
        {
            get
            {
                if (_instance == null) _instance = new MongoUtil();
                return _instance;
            }

            set
            {
                _instance = value;
            }
        }

        #endregion Properties

        #region Constructors

        private MongoUtil()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize database's connection
        /// </summary>
        /// <param name="appConfig">App configuration</param>
        public void Initialize(AppConfigOptions appConfig)
        {
            var connectionString = appConfig.PrimaryDBConnectionString;
            var dbName = appConfig.PrimaryDBName;
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(dbName);
        }

        /// <summary>
        /// ดึงข้อมูลจากตาราง
        /// </summary>
        /// <typeparam name="T">ข้อมูลที่ทำงานด้วย</typeparam>
        /// <param name="tableName">ชื่อตาราง</param>
        public IMongoCollection<T> GetCollection<T>(string tableName)
        {
            return _database.GetCollection<T>(tableName);
        }

        #endregion Methods
    }
}
