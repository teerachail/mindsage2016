using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using MindSageWeb.Controllers;
using MindSageWeb.Engines;
using MindSageWeb.Engines.Models;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using MindSageWeb.ViewModels.PurchaseCourse;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace MindSageWeb.Specs.Steps
{
    [Binding]
    public class Buy_New_CourseSpecs
    {
        [Given(@"Payment system will return result '(.*)'")]
        public void GivenPaymentSystemWillReturnResult(PaymentResult paymentResult)
        {
            var mockPayment = ScenarioContext.Current.Get<Mock<IPayment>>();
            mockPayment.Setup(it => it.ChargeCreditCard(It.IsAny<PaymentInformation>()))
                .Returns<PaymentInformation>(it => paymentResult);
        }

        [When(@"UserProfileId '(.*)' buy new course by using information in JSON format is")]
        public void WhenUserProfileIdBuyNewCourseByUsingInformationInJSONFormatIs(string userprofileId, string multilineText)
        {
            var mockPaymentRepo = ScenarioContext.Current.Get<Mock<IPaymentRepository>>();
            mockPaymentRepo.Setup(it => it.CreateNewPayment(It.IsAny<Payment>()))
                .Returns<Payment>(it => Task.Delay(0));

            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Setup(it => it.UpsertClassCalendar(It.IsAny<ClassCalendar>()));

            var mockUserProfileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserProfileRepo.Setup(it => it.UpsertUserProfile(It.IsAny<UserProfile>()));

            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

            var purchaseCtrl = ScenarioContext.Current.Get<PurchaseController>();
            var mockHttpContext = ScenarioContext.Current.Get<Mock<Microsoft.AspNet.Http.HttpContext>>();
            mockHttpContext.Setup(it => it.User.Identity.Name).Returns(userprofileId.GetMockStrinValue());
            purchaseCtrl.ActionContext.HttpContext = mockHttpContext.Object;

            var body = JsonConvert.DeserializeObject<PurchaseCourseViewModel>(multilineText);
            var result = purchaseCtrl.ChargeACreditCard(body).Result;
        }

        [Then(@"System upsert UserProfileId '(.*)' for update new subscription with JSON format are")]
        public void ThenSystemUpsertUserProfileIdForUpdateNewSubscriptionWithJSONFormatAre(string userprofileId, string multilineText)
        {
            var expecteds = JsonConvert.DeserializeObject<IEnumerable<UserProfile.Subscription>>(multilineText).ToList();

            Func<List<UserProfile.Subscription>, bool> validateUserSubscriptions = actuals =>
            {
                for (int index = 0; index < expecteds.Count; index++)
                {
                    Assert.True(!string.IsNullOrEmpty(actuals[index].id));
                    Assert.Equal(expecteds[index].Role, actuals[index].Role);
                    Assert.Equal(expecteds[index].ClassRoomName, actuals[index].ClassRoomName);
                    Assert.Equal(expecteds[index].CourseCatalogId, actuals[index].CourseCatalogId);
                    Assert.Equal(expecteds[index].ClassRoomId, actuals[index].ClassRoomId);
                    Assert.True(!string.IsNullOrEmpty(actuals[index].ClassCalendarId));
                    Assert.Equal(expecteds[index].LicenseId, actuals[index].LicenseId);
                    Assert.Equal(expecteds[index].LastActiveDate, actuals[index].LastActiveDate);
                    Assert.Equal(expecteds[index].CreatedDate, actuals[index].CreatedDate);
                    Assert.Equal(expecteds[index].DeletedDate, actuals[index].DeletedDate);
                }
                return true;
            };

            var mockUserProfileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserProfileRepo.Verify(it => it.UpsertUserProfile(It.Is<UserProfile>(userprofile =>
                  userprofile.id.Equals(userprofileId)
                  && validateUserSubscriptions(userprofile.Subscriptions.ToList())
            )));
        }

        [Then(@"System create new UserActivity with JSON format is")]
        public void ThenSystemCreateNewUserActivityWithJSONFormatIs(string multilineText)
        {
            Func<List<UserActivity.LessonActivity>, List<UserActivity.LessonActivity>, bool> validateLessonActivities = (expectes, actuals) =>
            {
                for (int index = 0; index < expectes.Count; index++)
                {
                    Assert.True(!string.IsNullOrEmpty(actuals[index].id));
                    Assert.Equal(expectes[index].BeginDate.ToUniversalTime(), actuals[index].BeginDate.ToUniversalTime());
                    Assert.Equal(expectes[index].TotalContentsAmount, actuals[index].TotalContentsAmount);
                    Assert.Equal(expectes[index].SawContentIds, actuals[index].SawContentIds);
                    Assert.Equal(expectes[index].CreatedCommentAmount, actuals[index].CreatedCommentAmount);
                    Assert.Equal(expectes[index].ParticipationAmount, actuals[index].ParticipationAmount);
                    Assert.Equal(expectes[index].LessonId, actuals[index].LessonId);
                }
                return true;
            };

            var expected = JsonConvert.DeserializeObject<UserActivity>(multilineText);
            Func<UserActivity, bool> validateCreateUserActivity = actual =>
            {
                Assert.True(!string.IsNullOrEmpty(actual.id));
                Assert.Equal(expected.IsTeacher, actual.IsTeacher);
                Assert.Equal(expected.IsPrivateAccount, actual.IsPrivateAccount);
                Assert.Equal(expected.UserProfileName, actual.UserProfileName);
                Assert.Equal(expected.UserProfileImageUrl, actual.UserProfileImageUrl);
                Assert.Equal(expected.HideClassRoomMessageDate, actual.HideClassRoomMessageDate);
                Assert.Equal(expected.UserProfileId, actual.UserProfileId);
                Assert.Equal(expected.ClassRoomId, actual.ClassRoomId);
                Assert.Equal(expected.CreatedDate, actual.CreatedDate);
                Assert.Equal(expected.DeletedDate, actual.DeletedDate);
                Assert.Equal(expected.LessonActivities.Count(), actual.LessonActivities.Count());
                Assert.True(validateLessonActivities(expected.LessonActivities.ToList(), actual.LessonActivities.ToList()));
                return true;
            };

            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Verify(it => it.UpsertUserActivity(It.Is<UserActivity>(actual =>
                validateCreateUserActivity(actual)
            )));
        }
    }
}
