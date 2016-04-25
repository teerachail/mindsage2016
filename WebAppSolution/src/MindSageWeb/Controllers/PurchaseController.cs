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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using MindSageWeb.Engines;

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
        private ErrorMessageOptions _errorMsgs;
        private AppConfigOptions _appConfig;
        private ILogger _logger;
        private IPayment _payment;

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
            IOptions<AppConfigOptions> appConfig,
            IOptions<ErrorMessageOptions> errorMsgs,
            ILoggerFactory loggerFactory,
            IPayment payment,
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
            _appConfig = appConfig.Value;
            _errorMsgs = errorMsgs.Value;
            _logger = loggerFactory.CreateLogger("PurchaseController");
            _payment = payment;
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
            try
            {
                var body = new AddCourseRequest
                {
                    Code = id,
                    Grade = grade,
                    UserProfileId = User.Identity.Name
                };
                var result = await _myCourseCtrl.AddCourse(body);
                if (result.IsSuccess) return RedirectToAction("Preparing", "My");
                else return RedirectToAction("Detail", "Home", new { @id = courseId, isCouponInvalid = true });
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = _errorMsgs.CanNotConnectToTheDatabase;
                return View("Error");
            }
        }

        /// <summary>
        /// Credit card form
        /// </summary>
        /// <param name="id">Course id</param>
        [HttpGet]
        public IActionResult Index(string id)
        {
            try
            {
                var selectedCourse = _courseCtrl.GetCourseDetail(id);
                if (selectedCourse == null)
                {
                    ViewBag.ErrorMessage = _errorMsgs.CourseNotFound;
                    return View("Error");
                }

                var selectedUserProfile = _userprofileRepo.GetUserProfileById(User.Identity.Name);
                if (selectedUserProfile == null)
                {
                    _logger.LogCritical($"User profile { User.Identity.Name } not found.");
                    ViewBag.ErrorMessage = _errorMsgs.UserProfileNotFound;
                    return View("Error");
                }

                var selectedClassRoom = _classRoomRepo.GetPublicClassRoomByCourseCatalogId(id);
                if (selectedClassRoom == null)
                {
                    _logger.LogCritical($"ClassRoom of CourseId: { id } not found.");
                    ViewBag.ErrorMessage = _errorMsgs.CourseInformationIncorrect;
                    return View("Error");
                }

                var isAlreadyHaveTheSelectedCourse = !_myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, id);
                if (isAlreadyHaveTheSelectedCourse) return RedirectToAction("entercourse", "my", new { @id = id });

                var model = new PurchaseCourseViewModel
                {
                    CourseId = id,
                    TotalChargeAmount = selectedCourse.PriceUSD
                };
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"MongoDB: { e.ToString() }");
                ViewBag.ErrorMessage = _errorMsgs.CanNotConnectToTheDatabase;
                return View("Error");
            }
        }

        [HttpPost, ActionName("Index")]
        public async Task<IActionResult> ChargeACreditCard(PurchaseCourseViewModel model)
        {
            try
            {
                var selectedUserProfile = _userprofileRepo.GetUserProfileById(User.Identity.Name);
                var isUserProfileValid = selectedUserProfile != null && !selectedUserProfile.DeletedDate.HasValue;
                if (!isUserProfileValid)
                {
                    _logger.LogCritical($"User profile { User.Identity.Name } not found.");
                    ViewBag.ErrorMessage = _errorMsgs.UserProfileNotFound;
                    return View("Error");
                }

                var selectedCourse = _courseCtrl.GetCourseDetail(model.CourseId);
                if (selectedCourse == null)
                {
                    ViewBag.ErrorMessage = _errorMsgs.CourseNotFound;
                    return View("Error");
                }

                var selectedClassRoom = _classRoomRepo.GetPublicClassRoomByCourseCatalogId(model.CourseId);
                var isClassRoomValid = selectedClassRoom != null && !selectedClassRoom.DeletedDate.HasValue;
                if (!isClassRoomValid)
                {
                    _logger.LogCritical($"ClassRoom of CourseId: { model.CourseId } not found.");
                    ViewBag.ErrorMessage = _errorMsgs.CourseInformationIncorrect;
                    return View("Error");
                }

                var isAlreadyHaveTheSelectedCourse = !_myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, model.CourseId);
                if (isAlreadyHaveTheSelectedCourse) return RedirectToAction("entercourse", "my", new { @id = model.CourseId });

                if (ModelState.IsValid)
                {
                    var paymentResult = Engines.Models.PaymentResult.Unknow;
                    var isPaymentSuccessed = false;
                    var newSubscriptionId = Guid.NewGuid().ToString();
                    var newPaymentId = string.Empty;
                    var now = _dateTime.GetCurrentTime();

                    try
                    {
                        // Pay with Paypal
                        paymentResult = _payment.ChargeCreditCard(new Engines.Models.PaymentInformation
                        {
                            Address = model.PrimaryAddress.Address,
                            City = model.PrimaryAddress.City,
                            Country = model.PrimaryAddress.Country.ToString(),
                            PostalCode = model.PrimaryAddress.ZipCode,
                            State = model.PrimaryAddress.State,
                            TotalPrice = selectedCourse.PriceUSD,
                            UserProfileId = User.Identity.Name,
                            PurchaseForCourseId = model.CourseId,
                            FirstName = model.CreditCardInfo.FirstName,
                            LastName = model.CreditCardInfo.LastName,
                            ExpiredYear = model.CreditCardInfo.ExpiredYear,
                            ExpiredMonth = model.CreditCardInfo.ExpiredMonth,
                            CVV = model.CreditCardInfo.CVV.ToString(),
                            CreditCardNumber = model.CreditCardInfo.CardNumber,
                            CardType = model.CreditCardInfo.CardType.ToString()
                        });
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Paypal payment error, from user: { User.Identity.Name }, course id: { model.CourseId }, Error: { e.ToString() }");
                        ViewBag.ErrorMessage = _errorMsgs.CanNotChargeACreditCard;
                        return View("Error");
                    }
                    finally
                    {
                        isPaymentSuccessed = paymentResult == Engines.Models.PaymentResult.approved;
                        newSubscriptionId = isPaymentSuccessed ? newSubscriptionId : "None";
                        var payment = createNewPayment(selectedCourse.id, selectedCourse.SideName, newSubscriptionId, model, selectedCourse.PriceUSD, now, isPaymentSuccessed);
                        await _paymentRepo.CreateNewPayment(payment);
                        newPaymentId = payment.id;
                    }

                    if (!isPaymentSuccessed)
                    {
                        ViewBag.ErrorMessage = _errorMsgs.CanNotChargeACreditCard;
                        return View("Error");
                    }

                    try
                    {
                        var requestLessonCatalogIds = selectedClassRoom.Lessons.Select(it => it.LessonCatalogId);
                        var lessonCatalogs = _lessonCatalogRepo.GetLessonCatalogById(requestLessonCatalogIds).ToList();
                        var newClassCalendar = createClassCalendar(selectedClassRoom, lessonCatalogs, now);
                        newClassCalendar.CalculateCourseSchedule();
                        newClassCalendar.ExpiredDate = null;
                        selectedUserProfile.Subscriptions = addNewSelfPurchaseSubscription(selectedUserProfile.Subscriptions, selectedClassRoom, newClassCalendar.id, model.CourseId, now, newSubscriptionId);
                        var userActivity = selectedUserProfile.CreateNewUserActivity(selectedClassRoom, newClassCalendar, lessonCatalogs, now);

                        _classCalendarRepo.UpsertClassCalendar(newClassCalendar);
                        _userprofileRepo.UpsertUserProfile(selectedUserProfile);
                        _userActivityRepo.UpsertUserActivity(userActivity);

                        return RedirectToAction("Finished", new { @id = newPaymentId });
                    }
                    catch (Exception e)
                    {
                        _logger.LogCritical($"User: '{ User.Identity.Name }' already purchased course id: '{ model.CourseId }' with payment id: '{ newPaymentId }' but the system can't create new course.");
                        throw e;
                    }
                }
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"MongoDB: { e.ToString() }");
                ViewBag.ErrorMessage = _errorMsgs.CanNotConnectToTheDatabase;
                return View("Error");
            }
        }
        private IEnumerable<UserProfile.Subscription> addNewSelfPurchaseSubscription(IEnumerable<UserProfile.Subscription> subscriptions, ClassRoom selectedClassRoom, string classCalendarId, string courseCatalogId, DateTime currentTime, string newSubscriptionId)
        {
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
            try
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
            catch (Exception e)
            {
                _logger.LogError($"MongoDB: { e.ToString() }");
                ViewBag.ErrorMessage = _errorMsgs.CanNotConnectToTheDatabase;
                return View("Error");
            }
        }

        #endregion Methods
    }
}
