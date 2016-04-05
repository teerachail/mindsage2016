using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebManagementPortal.Repositories.Models;

namespace WebManagementPortal.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ course catalog
    /// </summary>
    public class LessonCatalogRepository : ILessonCatalogRepository
    {
        #region ILessonCatalogRepository members

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล lesson catalog
        /// </summary>
        /// <param name="lessonCatalogId">ข้อมูลที่ต้องการดำเนินการ</param>
        public async Task UpsertLessonCatalog(LessonCatalog data)
        {
            var update = Builders<LessonCatalog>.Update
              .Set(it => it.Order, data.Order)
              .Set(it => it.Title, data.Title)
              .Set(it => it.UnitNo, data.UnitNo)
              .Set(it => it.SemesterName, data.SemesterName)
              .Set(it => it.ShortDescription, data.ShortDescription)
              .Set(it => it.MoreDescription, data.MoreDescription)
              .Set(it => it.ShortTeacherLessonPlan, data.ShortTeacherLessonPlan)
              .Set(it => it.MoreTeacherLessonPlan, data.MoreTeacherLessonPlan)
              .Set(it => it.PrimaryContentURL, data.PrimaryContentURL)
              .Set(it => it.PrimaryContentDescription, data.PrimaryContentDescription)
              .Set(it => it.IsPreviewable, data.IsPreviewable)
              .Set(it => it.ExtraContents, data.ExtraContents)
              .Set(it => it.CourseCatalogId, data.CourseCatalogId)
              .Set(it => it.CreatedDate, data.CreatedDate)
              .Set(it => it.DeletedDate, data.DeletedDate)
              .Set(it => it.Advertisments, data.Advertisments)
              .Set(it => it.TopicOfTheDays, data.TopicOfTheDays);

            var updateOption = new UpdateOptions { IsUpsert = true };
            await MongoAccess.MongoUtil.Instance
                .GetCollection<LessonCatalog>(AppConfigOptions.LessonCatalogTableName)
               .UpdateOneAsync(it => it.id == data.id, update, updateOption);
        }

        #endregion ILessonCatalogRepository members
    }
}
