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
    public class CourseCatalogRepository : ICourseCatalogRepository
    {
        #region ICourseCatalogRepository members

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล course catalog
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public async Task UpsertCourseCatalog(CourseCatalog data)
        {
            var update = Builders<CourseCatalog>.Update
               .Set(it => it.GroupName, data.GroupName)
               .Set(it => it.Advertisements, data.Advertisements)
               .Set(it => it.Grade, data.Grade)
               .Set(it => it.SideName, data.SideName)
               .Set(it => it.PriceUSD, data.PriceUSD)
               .Set(it => it.Series, data.Series)
               .Set(it => it.Title, data.Title)
               .Set(it => it.FullDescription, data.FullDescription)
               .Set(it => it.DescriptionImageUrl, data.DescriptionImageUrl)
               .Set(it => it.CreatedDate, data.CreatedDate)
               .Set(it => it.DeletedDate, data.DeletedDate)
               .Set(it => it.Semesters, data.Semesters)
               .Set(it => it.TotalWeeks, data.TotalWeeks);

            var updateOption = new UpdateOptions { IsUpsert = true };
            await MongoAccess.MongoUtil.Instance
                .GetCollection<CourseCatalog>(AppConfigOptions.CourseCatalogTableName)
               .UpdateOneAsync(it => it.id == data.id, update, updateOption);
        }

        #endregion ICourseCatalogRepository members
    }
}
