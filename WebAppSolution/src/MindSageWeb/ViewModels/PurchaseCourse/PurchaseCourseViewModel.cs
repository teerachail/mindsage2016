using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.ViewModels.PurchaseCourse
{
    public class PurchaseCourseViewModel
    {
        [Required]
        public string CourseId { get; set; }
        public CreditCard CreditCardInfo { get; set; }
        public BillingAddress PrimaryAddress { get; set; }
        public double TotalChargeAmount { get; set; }
    }
}
