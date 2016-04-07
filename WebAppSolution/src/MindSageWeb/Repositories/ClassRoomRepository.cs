using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;
using Microsoft.Extensions.OptionsModel;

namespace MindSageWeb.Repositories
{
    public class ClassRoomRepository : IClassRoomRepository
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
        public ClassRoomRepository(MongoAccess.MongoUtil mongoUtil, IOptions<DatabaseTableOptions> option)
        {
            _mongoUtil = mongoUtil;
            TableName = option.Value.ClassRooms;
        }

        #endregion Constructors

        #region IClassRoomRepository members

        /// <summary>
        /// ขอข้อมูล Class room จากรหัส
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอ</param>
        public ClassRoom GetClassRoomById(string classRoomId)
        {
            var result = _mongoUtil.GetCollection<ClassRoom>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.id == classRoomId)
                .ToEnumerable()
                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// อัพเดทข้อมูล Class room
        /// </summary>
        /// <param name="data">ข้อมูลที่จะทำการอัพเดท</param>
        public void UpsertClassRoom(ClassRoom data)
        {
            var update = Builders<ClassRoom>.Update
             .Set(it => it.Name, data.Name)
             .Set(it => it.CourseCatalogId, data.CourseCatalogId)
             .Set(it => it.DeletedDate, data.DeletedDate)
             .Set(it => it.Message, data.Message)
             .Set(it => it.IsPublic, data.IsPublic)
             .Set(it => it.Lessons, data.Lessons)
             .Set(it => it.LastUpdatedMessageDate, data.LastUpdatedMessageDate);

            var updateOption = new UpdateOptions { IsUpsert = true };
            _mongoUtil.GetCollection<ClassRoom>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        /// <summary>
        /// ขอข้อมูล public class room จากรหัส course catalog
        /// </summary>
        /// <param name="courseCatalogId">รหัส course catalog ที่ต้องการขอ</param>
        public ClassRoom GetPublicClassRoomByCourseCatalogId(string courseCatalogId)
        {
            var result = _mongoUtil.GetCollection<ClassRoom>(TableName)
               .Find(it => !it.DeletedDate.HasValue && it.IsPublic && it.CourseCatalogId == courseCatalogId)
               .ToEnumerable()
               .OrderByDescending(it => it.CreatedDate)
               .FirstOrDefault();
            return result;
        }

        #endregion IClassRoomRepository members
    }
}
