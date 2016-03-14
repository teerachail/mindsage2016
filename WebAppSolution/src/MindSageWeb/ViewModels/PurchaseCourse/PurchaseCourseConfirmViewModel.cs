using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.ViewModels.PurchaseCourse
{
    public class PurchaseCourseConfirmViewModel : PurchaseCourseViewModel
    {
        public PurchaseCourseConfirmViewModel(PurchaseCourseViewModel data)
        {
            CourseId = data.CourseId;
            CreditCardInfo = data.CreditCardInfo;
            PrimaryAddress = data.PrimaryAddress;
        }

        public double TotalChargeAmount { get; set; }
    }
}
