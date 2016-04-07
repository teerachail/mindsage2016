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
    /// ตัวเชื่อมต่อกับ payment
    /// </summary>
    public class PaymentRepository : IPaymentRepository
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
        public PaymentRepository(MongoAccess.MongoUtil mongoUtil, IOptions<DatabaseTableOptions> option)
        {
            _mongoUtil = mongoUtil;
            TableName = option.Value.Payments;
        }

        #endregion Constructors

        #region IPaymentRepository members

        /// <summary>
        /// เพิ่ม payment ใหม่
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการเพิ่ม</param>
        public async Task CreateNewPayment(Payment data)
        {
            var collection = _mongoUtil.GetCollection<Payment>(TableName);
            await collection.InsertOneAsync(data);
        }

        /// <summary>
        /// ขอข้อมูล payment จากรหัส
        /// </summary>
        /// <param name="id">รหัส payment ที่ต้องการขอข้อมูล</param>
        public async Task<Payment> GetPaymentById(string id)
        {
            var result = await _mongoUtil.GetCollection<Payment>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.id == id)
                .FirstOrDefaultAsync();
            return result;
        }

        #endregion IPaymentRepository members
    }
}
