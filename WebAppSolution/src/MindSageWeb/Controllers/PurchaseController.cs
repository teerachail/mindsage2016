using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using MindSageWeb.ViewModels.PurchaseCourse;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using PayPal.Api;

namespace MindSageWeb.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        #region Fields

        private IUserActivityRepository _userActivityRepo;
        private ILessonCatalogRepository _lessonCatalogRepo;
        private IClassCalendarRepository _classCalendarRepo;
        private IClassRoomRepository _classRoomRepo;
        private IUserProfileRepository _userprofileRepo;
        private IPaymentRepository _paymentRepo;
        private IDateTime _dateTime;
        private CourseController _courseCtrl;
        private MyCourseController _myCourseCtrl;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize purchase controller
        /// </summary>
        /// <param name="courseCtrl">Course API</param>
        /// <param name="myCourseCtrl">MyCourse API</param>
        /// <param name="userProfileRepo">User profile repository</param>
        /// <param name="classRoomRepo">Class room repository</param>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="lessonCatalogRepo">Lesson catalog repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        /// <param name="paymentRepo">Payment repository</param>
        public PurchaseController(CourseController courseCtrl,
            MyCourseController myCourseCtrl,
            IUserProfileRepository userProfileRepo,
            IClassRoomRepository classRoomRepo,
            IClassCalendarRepository classCalendarRepo,
            ILessonCatalogRepository lessonCatalogRepo,
            IUserActivityRepository userActivityRepo,
            IPaymentRepository paymentRepo,
            IDateTime dateTime)
        {
            _courseCtrl = courseCtrl;
            _myCourseCtrl = myCourseCtrl;
            _userprofileRepo = userProfileRepo;
            _classRoomRepo = classRoomRepo;
            _classCalendarRepo = classCalendarRepo;
            _lessonCatalogRepo = lessonCatalogRepo;
            _userActivityRepo = userActivityRepo;
            _paymentRepo = paymentRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Add new course using a code
        /// </summary>
        /// <param name="id">Code</param>
        /// <param name="grade">Course's grade name</param>
        /// <param name="courseId">Select course id</param>
        [HttpPost]
        public async Task<IActionResult> UserCode(string id, string grade, string courseId)
        {
            var body = new Repositories.Models.AddCourseRequest
            {
                Code = id,
                Grade = grade,
                UserProfileId = User.Identity.Name
            };

            var result = await _myCourseCtrl.AddCourse(body);
            if (result.IsSuccess)
            {
                return RedirectToAction("Preparing", "My");
            }

            return RedirectToAction("Detail", "Home", new { @id = courseId, isCouponInvalid = true });
        }

        /// <summary>
        /// Credit card form
        /// </summary>
        /// <param name="id">Course id</param>
        [HttpGet]
        public IActionResult Index(string id)
        {
            var selectedCourse = _courseCtrl.GetCourseDetail(id);
            if (selectedCourse == null) return View("Error");

            var canAddNewCourse = _myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, id);
            if (!canAddNewCourse) return RedirectToAction("entercourse", "my", new { @id = id });

            var model = new PurchaseCourseViewModel
            {
                CourseId = id,
                TotalChargeAmount = selectedCourse.PriceUSD
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PurchaseCourseViewModel model)
        {
            var canAddNewCourse = _myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, model.CourseId);
            if (!canAddNewCourse) return RedirectToAction("entercourse", "my", new { @id = model.CourseId });

            if (ModelState.IsValid)
            {
                var selectedCourse = _courseCtrl.GetCourseDetail(model.CourseId);
                if (selectedCourse == null) return View("Error");

                var selectedClassRoom = _classRoomRepo.GetPublicClassRoomByCourseCatalogId(model.CourseId);
                if (selectedClassRoom == null) return View("Error");

                var selectedUserProfile = _userprofileRepo.GetUserProfileById(User.Identity.Name);
                if (selectedUserProfile == null) return View("Error");

                // Pay with Paypal
                var isPaymentSuccessed = false;
                model.TotalChargeAmount = selectedCourse.PriceUSD;
                var paymentResult = payWithPaypal(model);
                if (paymentResult == "approve") isPaymentSuccessed = true;
                else
                {
                    ViewBag.ErrorMessage = "The Credit Card Verification System used by PayPal is currently unavailable. Please try to add your credit card at a later time. We apologize for this inconvenience";
                    return View("Error");
                }

                var now = _dateTime.GetCurrentTime();
                var lessonCatalogs = _lessonCatalogRepo.GetLessonCatalogById(selectedClassRoom.Lessons.Select(it => it.LessonCatalogId)).ToList();
                var newClassCalendar = createClassCalendar(selectedClassRoom, lessonCatalogs, now);
                newClassCalendar.CalculateCourseSchedule();

                var newSubscriptionId = string.Empty;
                selectedUserProfile.Subscriptions = addNewSelfPurchaseSubscription(
                    selectedUserProfile.Subscriptions, selectedClassRoom,
                    newClassCalendar.id, model.CourseId,
                    now, out newSubscriptionId);

                var userActivity = selectedUserProfile.CreateNewUserActivity(selectedClassRoom, newClassCalendar, lessonCatalogs, now);

                _classCalendarRepo.UpsertClassCalendar(newClassCalendar);
                _userprofileRepo.UpsertUserProfile(selectedUserProfile);
                _userActivityRepo.UpsertUserActivity(userActivity);

                var payment = createNewPayment(selectedCourse.id, selectedCourse.SideName, newSubscriptionId, model, selectedCourse.PriceUSD, now, isPaymentSuccessed);
                await _paymentRepo.CreateNewPayment(payment);
                return RedirectToAction("Finished", new { @id = payment.id });
            }
            return View(model);
        }

        /// <summary>
        /// Charging CreditCard
        /// </summary>
        /// <param name="model">PurchaseCourse details</param>
        /// <returns></returns>
        private string payWithPaypal(PurchaseCourseViewModel model)
        {
            var clientKey = "AciMCMM9IZHz0akSQdpyuOjQ9HaZI0nEjpS_Ik28qb681ZTXJFHuo03BF1LC7Cb_jgn6K0FQJ7LF_Cbd";
            var clientSecret = "EJe__bJ7Sz0CwQiJvz59ZpFMDn-7L8Ix3OdlaJHFWR8O2LouaYOY-AYJwE9YSvY6pl6pei9fcE_IC1f9";
            var tokenCredential = new OAuthTokenCredential(clientKey, clientSecret, new Dictionary<string, string>());

            var accessToken = tokenCredential.GetAccessToken();
            var config = new Dictionary<string, string>();
            config.Add("mode", "sandbox"); //Set mode to 'live' or 'sandbox'
            var apiContext = new APIContext
            {
                Config = config,
                AccessToken = accessToken
            };

            // A transaction defines the contract of a payment - what is the payment for and who is fulfilling it. 
            var description = $"User {User.Identity.Name} pay {model.TotalChargeAmount} for course {model.CourseId}";
            var transaction = new Transaction()
            {
                amount = new Amount()
                {
                    currency = "USD",
                    total = model.TotalChargeAmount.ToString(),
                    details = new Details()
                    {
                        shipping = "0",
                        subtotal = model.TotalChargeAmount.ToString(),
                        tax = "0"
                    }
                },
                description = description,
            };

            // A resource representing a Payer that funds a payment.
            var payer = new Payer()
            {
                payment_method = "credit_card",
                funding_instruments = new List<FundingInstrument>()
                {
                    new FundingInstrument()
                    {
                        credit_card = new PayPal.Api.CreditCard()
                        {
                            billing_address = new Address()
                            {
                                city = model.PrimaryAddress.City,
                                country_code = model.PrimaryAddress.Country.ToString(),
                                line1 = model.PrimaryAddress.Address,
                                postal_code = model.PrimaryAddress.ZipCode,
                                state = model.PrimaryAddress.State
                            },
                            cvv2 = model.CreditCardInfo.CVV.ToString(),
                            expire_month = model.CreditCardInfo.ExpiredMonth,
                            expire_year = model.CreditCardInfo.ExpiredYear,
                            first_name = model.CreditCardInfo.FirstName,
                            last_name = model.CreditCardInfo.LastName,
                            number = model.CreditCardInfo.CardNumber,
                            type = model.CreditCardInfo.CardType.ToString().ToLower()
                        }
                     }
                },
                payer_info = new PayerInfo
                {
                    email = User.Identity.Name,
                }
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
            return createdPayment.state;
        }

        private IEnumerable<UserProfile.Subscription> addNewSelfPurchaseSubscription(IEnumerable<UserProfile.Subscription> subscriptions, ClassRoom selectedClassRoom, string classCalendarId, string courseCatalogId, DateTime currentTime, out string newSubscriptionId)
        {
            newSubscriptionId = Guid.NewGuid().ToString();
            var newSubscriptions = subscriptions.ToList();
            var newSubscription = new UserProfile.Subscription
            {
                id = newSubscriptionId,
                Role = UserProfile.AccountRole.SelfPurchaser,
                LastActiveDate = currentTime,
                ClassRoomId = selectedClassRoom.id,
                ClassCalendarId = classCalendarId,
                CreatedDate = currentTime,
                ClassRoomName = selectedClassRoom.Name,
                CourseCatalogId = courseCatalogId
            };
            newSubscriptions.Add(newSubscription);
            return newSubscriptions;
        }
        private ClassCalendar createClassCalendar(ClassRoom classRoom, IEnumerable<LessonCatalog> lessonCatalogs, DateTime currentTime)
        {
            var lessonOrderRunner = 1;
            var lessonCalendars = classRoom.Lessons.Select(it =>
            {
                var lessonCatalog = lessonCatalogs.FirstOrDefault(lc => lc.id == it.LessonCatalogId);
                if (lessonCatalog == null) return null;

                var topicOfTheDays = lessonCatalog.TopicOfTheDays.Select(totd => new ClassCalendar.TopicOfTheDay
                {
                    id = Guid.NewGuid().ToString(),
                    CreatedDate = currentTime,
                    Message = totd.Message,
                    SendOnDay = totd.SendOnDay,
                    RequiredSendTopicOfTheDayDate = currentTime
                }).ToList();

                var result = new ClassCalendar.LessonCalendar
                {
                    id = Guid.NewGuid().ToString(),
                    Order = lessonOrderRunner++,
                    BeginDate = currentTime.Date,
                    CreatedDate = currentTime,
                    LessonId = it.id,
                    SemesterGroupName = lessonCatalog.SemesterName,
                    TopicOfTheDays = topicOfTheDays
                };
                return result;
            }).Where(it => it != null).ToList();
            var classCalendar = new ClassCalendar
            {
                id = Guid.NewGuid().ToString(),
                BeginDate = currentTime.Date,
                ClassRoomId = classRoom.id,
                CreatedDate = currentTime,
                Holidays = Enumerable.Empty<DateTime>(),
                ShiftDays = Enumerable.Empty<DateTime>(),
                LessonCalendars = lessonCalendars,
            };
            return classCalendar;
        }
        private Repositories.Models.Payment createNewPayment(string courseCatalogId, string courseName, string newSubscriptionId, PurchaseCourseViewModel model, double totalChargedAmount, DateTime currentTime, bool isPaymentSuccess)
        {
            var payment = new Repositories.Models.Payment
            {
                id = Guid.NewGuid().ToString(),
                FirstName = model.CreditCardInfo.FirstName,
                LastName = model.CreditCardInfo.LastName,
                Last4Digits = model.CreditCardInfo.LastFourDigits,
                CardType = model.CreditCardInfo.CardType.ToString(),
                CardNumber = APIUtil.EncodeCreditCard(model.CreditCardInfo.CardNumber),
                TotalChargedAmount = totalChargedAmount,
                BillingAddress = model.PrimaryAddress.Address,
                State = model.PrimaryAddress.State,
                City = model.PrimaryAddress.City,
                Country = model.PrimaryAddress.Country.ToString(),
                ZipCode = model.PrimaryAddress.ZipCode,
                CourseName = courseName,
                IsCompleted = isPaymentSuccess,
                CourseCatalogId = courseCatalogId,
                SubscriptionId = newSubscriptionId,
                CreatedDate = currentTime,
            };
            return payment;
        }

        /// <summary>
        /// Get purchased information
        /// </summary>
        /// <param name="id">Tracking id</param>
        [HttpGet]
        public async Task<IActionResult> Finished(string id)
        {
            var payment = await _paymentRepo.GetPaymentById(id);
            if (payment == null) return View("Error");

            var address = APIUtil.CreateAddressSummary(payment.BillingAddress, payment.State, payment.City, payment.Country, payment.ZipCode);
            var model = new CoursePurchasedViewModel
            {
                AddressSummary = address,
                CardType = payment.CardType,
                CourseId = payment.CourseCatalogId,
                LastFourDigits = payment.Last4Digits,
                TotalChargeAmount = payment.TotalChargedAmount
            };
            return View(model);
        }

        #endregion Methods
    }
}
