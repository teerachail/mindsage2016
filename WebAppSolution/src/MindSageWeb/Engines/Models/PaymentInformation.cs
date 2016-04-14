using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines.Models
{
    public class PaymentInformation
    {
        #region Properties

        // User information
        public string UserProfileId { get; set; }

        // Course information
        public string PurchaseForCourseId { get; set; }
        public double TotalPrice { get; set; }

        // Billing address
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }

        // Credit card information
        public string CVV { get; set; }
        public int ExpiredMonth { get; set; }
        public int ExpiredYear { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreditCardNumber { get; set; }
        public string CardType { get; set; }

        #endregion Properties
    }
}
