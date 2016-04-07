using Microsoft.Extensions.OptionsModel;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ contract
    /// </summary>
    public class ContractRepository : IContractRepository
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
        public ContractRepository(MongoAccess.MongoUtil mongoUtil, IOptions<DatabaseTableOptions> option)
        {
            _mongoUtil = mongoUtil;
            TableName = option.Value.Contracts;
        }

        #endregion Constructors

        #region IContractRepository members

        /// <summary>
        /// ขอข้อมูล contract จากรหัส teacher และ grade ที่ระบุ
        /// </summary>
        /// <param name="teacherCode">รหัส teacher ที่ต้องการค้นหา</param>
        /// <param name="grade">Grade</param>
        public async Task<Contract> GetContractsByTeacherCode(string teacherCode, string grade)
        {
            var result = await _mongoUtil.GetCollection<Contract>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.Licenses
                        .Where(l => !l.DeletedDate.HasValue)
                        .SelectMany(l => l.TeacherKeys)
                        .Where(t => !t.DeletedDate.HasValue)
                        .Any(t => t.Code == teacherCode && t.Grade == grade))
                .FirstOrDefaultAsync();
            return result;
        }

        #endregion IContractRepository members
    }
}
