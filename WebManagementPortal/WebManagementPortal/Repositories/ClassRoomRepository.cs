using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebManagementPortal.Repositories.Models;

namespace WebManagementPortal.Repositories
{
    public class ClassRoomRepository : IClassRoomRepository
    {
        #region IClassRoomRepository members

        /// <summary>
        /// อัพเดทข้อมูล Class room
        /// </summary>
        /// <param name="data">ข้อมูลที่จะทำการอัพเดท</param>
        public async Task UpsertClassRoom(ClassRoom data)
        {
            var update = Builders<ClassRoom>.Update
             .Set(it => it.Name, data.Name)
             .Set(it => it.CourseCatalogId, data.CourseCatalogId)
             .Set(it => it.CreatedDate, data.CreatedDate)
             .Set(it => it.DeletedDate, data.DeletedDate)
             .Set(it => it.Message, data.Message)
             .Set(it => it.IsPublic, data.IsPublic)
             .Set(it => it.Lessons, data.Lessons)
             .Set(it => it.LastUpdatedMessageDate, data.LastUpdatedMessageDate);

            var updateOption = new UpdateOptions { IsUpsert = true };
            await MongoAccess.MongoUtil.Instance.GetCollection<ClassRoom>(AppConfigOptions.ClassRoomTableName)
               .UpdateOneAsync(it => it.id == data.id, update, updateOption);
        }

        /// <summary>
        /// ขอข้อมูล public class room จากรหัส course catalog
        /// </summary>
        /// <param name="courseCatalogId">รหัส course catalog ที่ต้องการขอ</param>
        public async Task<IEnumerable<ClassRoom>> GetPublicClassRoomByCourseCatalogId(IEnumerable<string> courseCatalogIds)
        {
            var qry = (await MongoAccess.MongoUtil.Instance.GetCollection<ClassRoom>(AppConfigOptions.ClassRoomTableName)
               .FindAsync(it => it.IsPublic && courseCatalogIds.Contains(it.CourseCatalogId)))
               .ToEnumerable();
            return qry;
        }

        #endregion IClassRoomRepository members
    }
}
