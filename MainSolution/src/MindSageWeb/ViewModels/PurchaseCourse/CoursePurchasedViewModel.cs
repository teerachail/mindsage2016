using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.ViewModels.PurchaseCourse
{
    public class CoursePurchasedViewModel
    {
        public string CourseId { get; set; }
        public string AddressSummary { get; set; }
        public double TotalChargeAmount { get; set; }
        public string CardType { get; set; }
        public string LastFourDigits { get; set; }
    }
}
