using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.ViewModels.PurchaseCourse
{
    public class CreditCard
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public CreditCardType CardType { get; set; }
        [Required]
        public int ExpiredMonth { get; set; }
        [Required]
        public int ExpiredYear { get; set; }
        [Required]
        public int CVV { get; set; }

        public string LastFourDigits
        {
            get
            {
                var result = Controllers.APIUtil.GetLast4Characters(CardNumber);
                return result;
            }
        }
    }

    public class BillingAddress
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public Country Country { get; set; }
        [Required]
        public string ZipCode { get; set; }

        public string AddressSummary
        {
            get
            {
                var result = Controllers.APIUtil
                    .CreateAddressSummary(Address, State, City, Country.ToString(), ZipCode);
                return result;
            }
        }
    }

    public enum CreditCardType
    {
        [Display(Name = "Visa")]
        Visa,
        [Display(Name = "Master Card")]
        MasterCard,
    }

    public enum Country
    {
        USA
    }
}
