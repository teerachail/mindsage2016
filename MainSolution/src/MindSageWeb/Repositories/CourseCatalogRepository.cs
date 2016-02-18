using System;
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
        private const string CourseCatalogTableName = "test.au.mindsage.CourseCatalogs";

        #endregion Fields

        #region ICourseCatalogRepository members

        /// <summary>
        /// ขอข้อมูล course catalog จากรหัส
        /// </summary>
        /// <param name="courseCatalogId">รหัส course catalog ที่ต้องการข้อมูล</param>
        public IEnumerable<CourseCatalog> GetAvailableCourses()
        {
               var qry = MongoAccess.MongoUtil.Instance.GetCollection<CourseCatalog>(CourseCatalogTableName)
                .Find(it => !it.DeletedDate.HasValue)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอข้อมูล course ทั้งหมดที่สามารถเลือกได้
        /// </summary>
        public CourseCatalog GetCourseCatalogById(string courseCatalogId)
        {
            var result = MongoAccess.MongoUtil.Instance.GetCollection<CourseCatalog>(CourseCatalogTableName)
                .Find(it => !it.DeletedDate.HasValue && it.id == courseCatalogId)
                .ToEnumerable()
                .FirstOrDefault();
            return result;
        }

        #endregion ICourseCatalogRepository members
    }
}
