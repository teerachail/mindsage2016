using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using Microsoft.Extensions.OptionsModel;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    public class LessonTestResultRepository : ILessonTestResultRepository
    {
        #region Fields

        private readonly string TableName;
        private MongoAccess.MongoUtil _mongoUtil;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="mongoUtil">Mongo access utility</param>
        public LessonTestResultRepository(MongoAccess.MongoUtil mongoUtil, IOptions<DatabaseTableOptions> option)
        {
            _mongoUtil = mongoUtil;
            TableName = option.Value.LessonTestResult;
        }

        #endregion Constructors


        #region ILessonTestResultRepository members

        public LessonTestResult GetTestedResult(string classRoomId, string lessonId, string username)
        {
            var result = _mongoUtil.GetCollection<LessonTestResult>(TableName)
                .Find(it => it.ClassRoomId == classRoomId && it.LessonId == lessonId && it.UserProfileId == username)
                .ToEnumerable()
                .FirstOrDefault();
            return result;
        }

        public async Task UpsertTestedResult(LessonTestResult data)
        {
            var isNewRecord = string.IsNullOrEmpty(data.id);
            if (isNewRecord)
            {
                data.id = Guid.NewGuid().ToString();
                data.CreatedDate = DateTime.Now;
                await _mongoUtil.GetCollection<LessonTestResult>(TableName).InsertOneAsync(data);
            }
            else
            {
                var update = Builders<LessonTestResult>.Update
                 .Set(it => it.Answers, data.Answers);

                var updateOption = new UpdateOptions { IsUpsert = true };
                _mongoUtil.GetCollection<LessonTestResult>(TableName)
                   .UpdateOne(it => it.id == data.id, update, updateOption);
            }
        }

        #endregion ILessonTestResultRepository members
    }
}
