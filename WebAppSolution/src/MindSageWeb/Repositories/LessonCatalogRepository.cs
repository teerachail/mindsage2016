using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Lesson catalog
    /// </summary>
    public class LessonCatalogRepository : ILessonCatalogRepository
    {
        #region Fields

        // HACK: Table name
        private const string TableName = "test.au.mindsage.LessonCatalogs";
        private MongoAccess.MongoUtil _mongoUtil;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="mongoUtil">Mongo access utility</param>
        public LessonCatalogRepository(MongoAccess.MongoUtil mongoUtil)
        {
            _mongoUtil = mongoUtil;
        }

        #endregion Constructors

        #region ILessonCatalogRepository members

        /// <summary>
        /// ขอข้อมูล Lesson catalog จากรหัส
        /// </summary>
        /// <param name="lessonCatalogId">รหัส Lesson catalog ที่ต้องการ</param>
        public LessonCatalog GetLessonCatalogById(string lessonCatalogId)
        {
            var result = _mongoUtil.GetCollection<LessonCatalog>(TableName)
               .Find(it => !it.DeletedDate.HasValue && it.id == lessonCatalogId)
               .ToEnumerable()
               .OrderByDescending(it => it.CreatedDate)
               .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// ขอข้อมูล Lesson catalog จากรหัส
        /// </summary>
        /// <param name="lessonCatalogId">รหัส Lesson catalog ที่ต้องการ</param>
        public IEnumerable<LessonCatalog> GetLessonCatalogById(IEnumerable<string> lessonCatalogIds)
        {
            var qry = _mongoUtil.GetCollection<LessonCatalog>(TableName)
               .Find(it => !it.DeletedDate.HasValue && lessonCatalogIds.Contains(it.id))
               .ToEnumerable();
            return qry;
        }

        #endregion ILessonCatalogRepository members
    }
}
