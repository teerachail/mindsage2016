using Microsoft.Extensions.OptionsModel;
using MindSageWeb.Engines.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    /// <summary>
    /// ตัวจ่ายเงินของ Paypal
    /// </summary>
    public class PaypalPayment : IPayment
    {
        #region Fields

        private AppConfigOptions _appConfig;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize Paypal payment
        /// </summary>
        public PaypalPayment(IOptions<AppConfigOptions> appConfig)
        {
            _appConfig = appConfig.Value;
        }

        #endregion Constructors

        #region IPayment members

        /// <summary>
        /// ตัดบัตรเครดิต
        /// </summary>
        /// <param name="payment">ข้อมูลบัตรเครดิตที่ต้องการดำเนินการ</param>
        public PaymentResult ChargeCreditCard(PaymentInformation paymentInfo)
        {
            var tokenCredential = new OAuthTokenCredential(_appConfig.PaypalClientId, _appConfig.PaypalClientSecret, new Dictionary<string, string>());
            var accessToken = tokenCredential.GetAccessToken();
            var config = new Dictionary<string, string>();
            config.Add("mode", "live");
            var apiContext = new APIContext
            {
                Config = config,
                AccessToken = accessToken
            };

            // A transaction defines the contract of a payment - what is the payment for and who is fulfilling it. 
            var transaction = new Transaction()
            {
                amount = new Amount()
                {
                    currency = "USD",
                    total = paymentInfo.TotalPrice.ToString(),
                    details = new Details()
                    {
                        shipping = "0",
                        subtotal = paymentInfo.TotalPrice.ToString(),
                        tax = "0"
                    }
                },
                description = $"User { paymentInfo.UserProfileId } pay { paymentInfo.TotalPrice.ToString("C2") } for course { paymentInfo.PurchaseForCourseId }",
            };

            // A resource representing a Payer that funds a payment.
            var payer = new Payer()
            {
                payment_method = "credit_card",
                funding_instruments = new List<FundingInstrument>()
                {
                    new FundingInstrument()
                    {
                        credit_card = new CreditCard()
                        {
                            billing_address = new Address()
                            {
                                city = paymentInfo.City,
                                country_code = paymentInfo.Country,
                                line1 = paymentInfo.Address,
                                postal_code = paymentInfo.PostalCode,
                                state = paymentInfo.State
                            },
                            cvv2 = paymentInfo.CVV,
                            expire_month = paymentInfo.ExpiredMonth,
                            expire_year = paymentInfo.ExpiredYear,
                            first_name = paymentInfo.FirstName,
                            last_name = paymentInfo.LastName,
                            number = paymentInfo.CreditCardNumber,
                            type = paymentInfo.CardType.ToLower()
                        }
                     }
                },
                payer_info = new PayerInfo { email = paymentInfo.UserProfileId }
            };

            // A Payment resource; create one using the above types and intent as `sale` or `authorize`
            var payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = new List<Transaction>() { transaction }
            };

            // Create a payment using a valid APIContext
            var createdPayment = payment.Create(apiContext);
            var result = PaymentResult.Unknow;
            return Enum.TryParse<PaymentResult>(createdPayment.state, out result) ? result : PaymentResult.Unknow;
        }

        #endregion IPayment members
    }
}
