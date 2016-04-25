using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using MindSageWeb.Controllers;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using Moq;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace MindSageWeb.Specs.Steps
{
    [Binding]
    public class BackgroundSteps
    {
        [Given(@"Initialize mocking data")]
        public void GivenInitializeMockingData()
        {
            var mock = ScenarioContext.Current.Get<MockRepository>();

            var classRoomRepo = mock.Create<IClassRoomRepository>();
            var likeLessonRepo = mock.Create<ILikeLessonRepository>();
            var userprofileRepo = mock.Create<IUserProfileRepository>();
            var classCalendarRepo = mock.Create<IClassCalendarRepository>();
            var lessonCatalogRepo = mock.Create<ILessonCatalogRepository>();
            var commentRepo = mock.Create<ICommentRepository>();
            var friendRequestRepo = mock.Create<IFriendRequestRepository>();
            var userActivityRepo = mock.Create<IUserActivityRepository>();
            var likeCommentRepo = mock.Create<ILikeCommentRepository>();
            var likeDiscussionRepo = mock.Create<ILikeDiscussionRepository>();
            var studentKeyRepo = mock.Create<IStudentKeyRepository>();
            var notificationRepo = mock.Create<INotificationRepository>();
            var contractRepo = mock.Create<IContractRepository>();
            var courseCatalogRepo = mock.Create<ICourseCatalogRepository>();
            var paymentRepo = mock.Create<IPaymentRepository>();
            var payment = mock.Create<Engines.IPayment>();
            var logger = mock.Create<ILogger>();
            var loggerFactory = mock.Create<ILoggerFactory>();
            var appConfigOption = mock.Create<IOptions<AppConfigOptions>>();
            var errorOption = mock.Create<IOptions<ErrorMessageOptions>>();
            var httpContext = mock.Create<Microsoft.AspNet.Http.HttpContext>();
            var dateTime = mock.Create<IDateTime>();

            appConfigOption.Setup(it => it.Value).Returns(new AppConfigOptions());
            errorOption.Setup(it => it.Value).Returns(new ErrorMessageOptions());

            ScenarioContext.Current.Set(classRoomRepo);
            ScenarioContext.Current.Set(likeLessonRepo);
            ScenarioContext.Current.Set(userprofileRepo);
            ScenarioContext.Current.Set(classCalendarRepo);
            ScenarioContext.Current.Set(lessonCatalogRepo);
            ScenarioContext.Current.Set(commentRepo);
            ScenarioContext.Current.Set(friendRequestRepo);
            ScenarioContext.Current.Set(userActivityRepo);
            ScenarioContext.Current.Set(likeCommentRepo);
            ScenarioContext.Current.Set(likeDiscussionRepo);
            ScenarioContext.Current.Set(studentKeyRepo);
            ScenarioContext.Current.Set(notificationRepo);
            ScenarioContext.Current.Set(contractRepo);
            ScenarioContext.Current.Set(courseCatalogRepo);
            ScenarioContext.Current.Set(paymentRepo);
            ScenarioContext.Current.Set(payment);
            ScenarioContext.Current.Set(logger);
            ScenarioContext.Current.Set(loggerFactory);
            ScenarioContext.Current.Set(appConfigOption);
            ScenarioContext.Current.Set(errorOption);
            ScenarioContext.Current.Set(httpContext);
            ScenarioContext.Current.Set(dateTime);

            loggerFactory.Setup(it => it.CreateLogger(It.IsAny<string>()))
                .Returns(() => logger.Object);

            var notificationCtrl = new NotificationController(userprofileRepo.Object,
                notificationRepo.Object,
                likeLessonRepo.Object,
                likeCommentRepo.Object,
                likeDiscussionRepo.Object,
                commentRepo.Object,
                classCalendarRepo.Object,
                friendRequestRepo.Object,
                dateTime.Object);

            var lessonCtrl = new LessonController(classCalendarRepo.Object,
                userprofileRepo.Object,
                classRoomRepo.Object,
                likeLessonRepo.Object,
                lessonCatalogRepo.Object,
                commentRepo.Object,
                friendRequestRepo.Object,
                userActivityRepo.Object,
                notificationCtrl,
                appConfigOption.Object,
                dateTime.Object);

            var commentCtrl = new CommentController(classCalendarRepo.Object,
                userprofileRepo.Object,
                commentRepo.Object,
                userActivityRepo.Object,
                likeCommentRepo.Object,
                notificationCtrl,
                dateTime.Object);

            var discussionCtrl = new DiscussionController(classCalendarRepo.Object,
                userprofileRepo.Object,
                commentRepo.Object,
                userActivityRepo.Object,
                likeDiscussionRepo.Object,
                notificationCtrl,
                dateTime.Object);

            var myCourseCtrl = new MyCourseController(classCalendarRepo.Object,
                userprofileRepo.Object,
                userActivityRepo.Object,
                classRoomRepo.Object,
                studentKeyRepo.Object,
                lessonCatalogRepo.Object,
                likeLessonRepo.Object,
                likeCommentRepo.Object,
                likeDiscussionRepo.Object,
                contractRepo.Object,
                courseCatalogRepo.Object,
                loggerFactory.Object,
                dateTime.Object);

            var friendCtrl = new FriendController(classCalendarRepo.Object,
                userprofileRepo.Object,
                friendRequestRepo.Object,
                userActivityRepo.Object,
                classRoomRepo.Object,
                dateTime.Object);

            var courseCtrl = new CourseController(courseCatalogRepo.Object, appConfigOption.Object);
            var purchaseCtrl = new PurchaseController(courseCtrl,
                myCourseCtrl,
                userprofileRepo.Object,
                classRoomRepo.Object,
                classCalendarRepo.Object,
                lessonCatalogRepo.Object,
                userActivityRepo.Object,
                paymentRepo.Object,
                appConfigOption.Object,
                errorOption.Object,
                loggerFactory.Object,
                payment.Object,
                dateTime.Object);

            ScenarioContext.Current.Set(notificationCtrl);
            ScenarioContext.Current.Set(lessonCtrl);
            ScenarioContext.Current.Set(commentCtrl);
            ScenarioContext.Current.Set(discussionCtrl);
            ScenarioContext.Current.Set(myCourseCtrl);
            ScenarioContext.Current.Set(friendCtrl);
            ScenarioContext.Current.Set(purchaseCtrl);
        }

        [Given(@"Initialize mocking notifications' repositories")]
        public void GivenInitializeMockingNotificationsRepositories()
        {
            var mockLikeLessonRepo = ScenarioContext.Current.Get<Mock<ILikeLessonRepository>>();
            mockLikeLessonRepo.Setup(it => it.GetRequireNotifyLikeLessons()).Returns(() => Enumerable.Empty<LikeLesson>());

            var mockCommentRepo = ScenarioContext.Current.Get<Mock<ICommentRepository>>();
            mockCommentRepo.Setup(it => it.GetRequireNotifyComments()).Returns(() => Enumerable.Empty<Comment>());
            mockCommentRepo.Setup(it => it.GetRequireNotifyDiscussions()).Returns(() => Enumerable.Empty<Comment>());

            var mockLikeCommentRepo = ScenarioContext.Current.Get<Mock<ILikeCommentRepository>>();
            mockLikeCommentRepo.Setup(it => it.GetRequireNotifyLikeComments()).Returns(() => Enumerable.Empty<LikeComment>());

            var mockLikeDiscussionRepo = ScenarioContext.Current.Get<Mock<ILikeDiscussionRepository>>();
            mockLikeDiscussionRepo.Setup(it => it.GetRequireNotifyLikeDiscussions()).Returns(() => Enumerable.Empty<LikeDiscussion>());

            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Setup(it => it.GetRequireNotifyTopicOfTheDay(It.IsAny<DateTime>())).Returns<DateTime>(it => Enumerable.Empty<ClassCalendar>());

            var mockNotificationRepo = ScenarioContext.Current.Get<Mock<INotificationRepository>>();
            var mockFriendRequestRepo = ScenarioContext.Current.Get<Mock<IFriendRequestRepository>>();
        }
    }
}
