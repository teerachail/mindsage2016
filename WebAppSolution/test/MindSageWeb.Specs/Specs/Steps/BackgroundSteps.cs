using Microsoft.Extensions.OptionsModel;
using MindSageWeb.Controllers;
using MindSageWeb.Repositories;
using Moq;
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
            var option = mock.Create<IOptions<AppConfigOptions>>();
            var dateTime = mock.Create<IDateTime>();

            option.Setup(it => it.Value).Returns(new AppConfigOptions());

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
            ScenarioContext.Current.Set(option);
            ScenarioContext.Current.Set(dateTime);

            var notificationCtrl = new NotificationController(userprofileRepo.Object,
                notificationRepo.Object,
                likeLessonRepo.Object,
                likeCommentRepo.Object,
                likeDiscussionRepo.Object,
                commentRepo.Object,
                classCalendarRepo.Object,
                friendRequestRepo.Object,
                dateTime.Object);

            var myCourseCtrl = new LessonController(classCalendarRepo.Object,
                userprofileRepo.Object,
                classRoomRepo.Object,
                likeLessonRepo.Object,
                lessonCatalogRepo.Object,
                commentRepo.Object,
                friendRequestRepo.Object,
                userActivityRepo.Object,
                notificationCtrl,
                option.Object,
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

            var mycourseCtrl = new MyCourseController(classCalendarRepo.Object,
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
                dateTime.Object);

            var friendCtrl = new FriendController(classCalendarRepo.Object,
                userprofileRepo.Object,
                friendRequestRepo.Object,
                userActivityRepo.Object,
                dateTime.Object);

            ScenarioContext.Current.Set(notificationCtrl);
            ScenarioContext.Current.Set(myCourseCtrl);
            ScenarioContext.Current.Set(commentCtrl);
            ScenarioContext.Current.Set(discussionCtrl);
            ScenarioContext.Current.Set(mycourseCtrl);
            ScenarioContext.Current.Set(friendCtrl);
        }
    }
}
