using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Student key
    /// </summary>
    public class StudentKeyRepository : IStudentKeyRepository
    {
        #region Fields

        // HACK: Table name
        private const string StudentKeysTableName = "test.au.mindsage.StudentKeys";

        #endregion Fields

        #region IStudentKeyRepository members

        /// <summary>
        /// ขอข้อมูล Student key ที่สามารถใช้ได้จากรหัส class room
        /// </summary>
        /// <param name="classRoomId">รหัส class room ที่ต้องการขอข้อมูล</param>
        public StudentKey GetStudentKeyByClassRoomId(string classRoomId)
        {
            var result = MongoAccess.MongoUtil.Instance.GetCollection<StudentKey>(StudentKeysTableName)
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
            var result = MongoAccess.MongoUtil.Instance.GetCollection<StudentKey>(StudentKeysTableName)
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
            throw new NotImplementedException();
        }

        #endregion IStudentKeyRepository members
    }
}
