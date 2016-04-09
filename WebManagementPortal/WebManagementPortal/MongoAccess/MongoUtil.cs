using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebManagementPortal.MongoAccess
{
    public class MongoUtil
    {
        #region Fields

        private static IMongoClient _client;
        private static IMongoDatabase _database;

        #endregion Fields

        #region Properties

        public static MongoUtil Instance
        {
            get { return new MongoUtil(); }
        }

        #endregion Properties

        #region Constructors

        private MongoUtil()
        {
            var connectionString = AppConfigOptions.PrimaryDBConnectionString;
            var dbName = AppConfigOptions.PrimaryDBName;
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
        public IMongoCollection<T> GetCollection<T>(string tableName)
        {
            return _database.GetCollection<T>(tableName);
        }

        #endregion Methods
    }
}
