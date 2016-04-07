using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;
using Microsoft.Extensions.OptionsModel;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Student key
    /// </summary>
    public class StudentKeyRepository : IStudentKeyRepository
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
        public StudentKeyRepository(MongoAccess.MongoUtil mongoUtil, IOptions<DatabaseTableOptions> option)
        {
            _mongoUtil = mongoUtil;
            TableName = option.Value.StudentKeys;
        }

        #endregion Constructors

        #region IStudentKeyRepository members

        /// <summary>
        /// ขอข้อมูล Student key ที่สามารถใช้ได้จากรหัส class room
        /// </summary>
        /// <param name="classRoomId">รหัส class room ที่ต้องการขอข้อมูล</param>
        public StudentKey GetStudentKeyByClassRoomId(string classRoomId)
        {
            var result = _mongoUtil.GetCollection<StudentKey>(TableName)
               .Find(it => !it.DeletedDate.HasValue && it.ClassRoomId == classRoomId)
               .ToEnumerable()
               .OrderByDescending(it => it.CreatedDate)
               .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// ขอข้อมูล Student key ที่สามารถใช้ได้จากรหัส code และเกรด
        /// </summary>
        /// <param name="code">รหัส code ที่ต้องการขอข้อมูล</param>
        /// <param name="grade">เกรดที่ต้องการขอขอ้ฒุล</param>
        public StudentKey GetStudentKeyByCodeAndGrade(string code, string grade)
        {
            var result = _mongoUtil.GetCollection<StudentKey>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.Code == code && it.Grade == grade)
                .ToEnumerable()
                .OrderByDescending(it => it.CreatedDate)
                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล Student key
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public void UpsertStudentKey(StudentKey data)
        {
            var update = Builders<StudentKey>.Update
               .Set(it => it.Code, data.Code)
               .Set(it => it.Grade, data.Grade)
               .Set(it => it.CourseCatalogId, data.CourseCatalogId)
               .Set(it => it.ClassRoomId, data.ClassRoomId)
               .Set(it => it.CreatedDate, data.CreatedDate)
               .Set(it => it.DeletedDate, data.DeletedDate);

            var updateOption = new UpdateOptions { IsUpsert = true };
            _mongoUtil.GetCollection<StudentKey>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        #endregion IStudentKeyRepository members
    }
}
