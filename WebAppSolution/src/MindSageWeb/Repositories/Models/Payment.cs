using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class Payment
    {
        #region Properties

        public string id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Last4Digits { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string TotalChargedAmount { get; set; }
        public string BillingAddress { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string CourseName { get; set; }
        public bool IsCompleted { get; set; }
        public string CourseCatalogId { get; set; }
        public string SubscriptionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        #endregion Properties
    }
}
