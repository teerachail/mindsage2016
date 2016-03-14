using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    public class ClassRoomRepository : IClassRoomRepository
    {
        #region Fields

        // HACK: Table name
        private const string TableName = "test.au.mindsage.ClassRooms";

        #endregion Fields

        #region IClassRoomRepository members

        /// <summary>
        /// ขอข้อมูล Class room จากรหัส
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอ</param>
        public ClassRoom GetClassRoomById(string classRoomId)
        {
            var result = MongoAccess.MongoUtil.Instance.GetCollection<ClassRoom>(TableName)
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
             .Set(it => it.Lessons, data.Lessons)
             .Set(it => it.LastUpdatedMessageDate, data.LastUpdatedMessageDate);

            var updateOption = new UpdateOptions { IsUpsert = true };
            MongoAccess.MongoUtil.Instance.GetCollection<ClassRoom>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        #endregion IClassRoomRepository members
    }
}
