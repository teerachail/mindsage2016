﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MindSageWeb.Repositories.Models;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ course catalog
    /// </summary>
    public class CourseCatalogRepository : ICourseCatalogRepository
    {
        #region Fields

        // HACK: Table name
        private const string TableName = "test.au.mindsage.CourseCatalogs";

        #endregion Fields

        #region ICourseCatalogRepository members

        /// <summary>
        /// ขอข้อมูล course catalog จากรหัส
        /// </summary>
        /// <param name="courseCatalogId">รหัส course catalog ที่ต้องการข้อมูล</param>
        public IEnumerable<CourseCatalog> GetAvailableCourses()
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<CourseCatalog>(TableName)
             .Find(it => !it.DeletedDate.HasValue)
             .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอข้อมูล course ทั้งหมดที่สามารถเลือกได้
        /// </summary>
        public CourseCatalog GetCourseCatalogById(string courseCatalogId)
        {
            var result = MongoAccess.MongoUtil.Instance.GetCollection<CourseCatalog>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.id == courseCatalogId)
                .ToEnumerable()
                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// ขอข้อมูล course ที่อยู่ในกลุ่มเดียวกัน
        /// </summary>
        /// <param name="groupName">ชื่อกลุ่มที่ต้องการค้นหา</param>
        public IEnumerable<CourseCatalog> GetRelatedCoursesByGroupName(string groupName)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<CourseCatalog>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.GroupName == groupName)
                .ToEnumerable();
            return qry;
        }

        #endregion ICourseCatalogRepository members
    }
}
