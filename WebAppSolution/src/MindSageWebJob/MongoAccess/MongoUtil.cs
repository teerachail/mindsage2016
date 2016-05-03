using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeWebJobsSDKQueue.MongoAccess
{
    public class MongoUtil
    {
        #region Fields

        private static IMongoClient _client;
        private static IMongoDatabase _database;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize database's connection
        /// </summary>
        /// <param name="appConfig">App configuration</param>
        public MongoUtil(string primaryDBConnectionString, string databaseName)
        {
            _client = new MongoClient(primaryDBConnectionString);
            _database = _client.GetDatabase(databaseName);
        }

        #endregion Constructors

        #region Methods

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
