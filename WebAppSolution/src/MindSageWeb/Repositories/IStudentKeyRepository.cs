using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Student key
    /// </summary>
    public interface IStudentKeyRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล Student key ที่สามารถใช้ได้จากรหัส class room
        /// </summary>
        /// <param name="classRoomId">รหัส class room ที่ต้องการขอข้อมูล</param>
        StudentKey GetStudentKeyByClassRoomId(string classRoomId);

        /// <summary>
        /// ขอข้อมูล Student key ที่สามารถใช้ได้จากรหัส code และเกรด
        /// </summary>
        /// <param name="code">รหัส code ที่ต้องการขอข้อมูล</param>
        /// <param name="grade">เกรดที่ต้องการขอขอ้ฒุล</param>
        StudentKey GetStudentKeyByCodeAndGrade(string code, string grade);

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล Student key
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        void UpsertStudentKey(StudentKey data);

        /// <summary>
        /// เพิ่มข้อมูล Student key ใหม่
        /// </summary>
        /// <param name="data">ข้อมูล Student key ที่ต้องการเพิ่ม</param>
        Task CreateNewStudentKey(StudentKey data);

        #endregion Methods
    }
}
