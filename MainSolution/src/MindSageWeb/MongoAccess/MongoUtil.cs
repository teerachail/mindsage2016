using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.MongoAccess
{
    static class MongoUtil
    {
        #region Fields

        private static IMongoClient _client;
        private static IMongoDatabase _database;

        #endregion Fields

        #region Constructors

        static MongoUtil()
        {
            // HACK: MongoDb's connection
            var connectionString = "mongodb://MongoLab-4o:UMOcc359jl3WoTatREpo9qAAEGFL87uwoUWVyfusDUk-@ds056288.mongolab.com:56288/MongoLab-4o";
            var dbName = "MongoLab-4o";
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(dbName);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// ดึงข้อมูลจากตาราง
        /// </summary>
        /// <typeparam name="T">ข้อมูลที่ทำงานด้วย</typeparam>
        /// <param name="tableName">ชื่อตาราง</param>
        public static IMongoCollection<T> GetCollection<T>(string tableName)
        {
            return _database.GetCollection<T>(tableName);
        }

        #endregion Methods
    }
}
