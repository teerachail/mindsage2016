using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebManagementPortal.Repositories.Models;

namespace WebManagementPortal.Repositories
{
    public class ContractRepository : IContractRepository
    {
        #region IContractRepository members

        /// <summary>
        /// ขอข้อมูล contract จากรหัสที่ระบุ
        /// </summary>
        /// <param name="contractIds">รหัส contract ที่ต้องการค้นหา</param>
        public async Task<IEnumerable<Contract>> GetContractsById(IEnumerable<string> contractIds)
        {
            var qry = (await MongoAccess.MongoUtil.Instance.GetCollection<Contract>(AppConfigOptions.ContractTableName)
               .FindAsync(it => contractIds.Contains(it.id)))
               .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล contract
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public async Task UpsertContract(Contract data)
        {
            var update = Builders<Contract>.Update
             .Set(it => it.Name, data.Name)
             .Set(it => it.StartDate, data.StartDate)
             .Set(it => it.ExpiredDate, data.ExpiredDate)
             .Set(it => it.TimeZone, data.TimeZone)
             .Set(it => it.CreatedDate, data.CreatedDate)
             .Set(it => it.DeletedDate, data.DeletedDate)
             .Set(it => it.Licenses, data.Licenses);

            var updateOption = new UpdateOptions { IsUpsert = true };
            await MongoAccess.MongoUtil.Instance.GetCollection<Contract>(AppConfigOptions.ContractTableName)
               .UpdateOneAsync(it => it.id == data.id, update, updateOption);
        }

        #endregion IContractRepository members
    }
}
