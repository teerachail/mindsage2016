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
                if (string.IsNullOrEmpty(CardNumber)) return string.Empty;
                const int MinimumDigitRequired = 4;
                if (CardNumber.Length < MinimumDigitRequired) return string.Empty;
                var beginIndex = CardNumber.Length - MinimumDigitRequired;
                return CardNumber.Substring(beginIndex, MinimumDigitRequired);
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
                var areArgumentValid = !string.IsNullOrEmpty(Address)
                    && !string.IsNullOrEmpty(State)
                    && !string.IsNullOrEmpty(City)
                    && !string.IsNullOrEmpty(ZipCode);
                if (!areArgumentValid) return string.Empty;
                return string.Format("{0} {1} {2} {3} {4}", Address, State, City, Country, ZipCode);
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
