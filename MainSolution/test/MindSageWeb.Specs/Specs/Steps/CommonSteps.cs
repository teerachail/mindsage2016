using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace MindSageWeb.Specs.Steps
{
    [Binding]
    public sealed class CommonSteps
    {
        [Given(@"System have UserProfile collection with JSON format are")]
        public void GivenSystemHaveUserProfileCollectionWithJSONFormatAre(string multilineText)
        {
            var userprofiles = JsonConvert.DeserializeObject<IEnumerable<UserProfile>>(multilineText);
            var mockUserprofileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserprofileRepo.Setup(it => it.GetUserProfileById(It.IsAny<string>()))
                .Returns<string>(userprofileId => userprofiles.FirstOrDefault(it => it.id == userprofileId));
            mockUserprofileRepo.Setup(it => it.GetUserProfilesByClassRoomId(It.IsAny<string>()))
                .Returns<string>(classRoomId =>
                {
                    var qry = from userprofile in userprofiles
                              from subscription in userprofile.Subscriptions
                              where subscription.ClassRoomId == classRoomId
                              select userprofile;
                    return qry;
                });
        }

        [Given(@"System have ClassCalendar collection with JSON format are")]
        public void GivenSystemHaveClassCalendarCollectionWithJSONFormatAre(string multilineText)
        {
            var classCalendars = JsonConvert.DeserializeObject<IEnumerable<ClassCalendar>>(multilineText);
            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Setup(it => it.GetClassCalendarByClassRoomId(It.IsAny<string>()))
                .Returns<string>(classRoomId => classCalendars.Where(it => it.ClassRoomId == classRoomId).FirstOrDefault());
        }

        [Given(@"Today is '(.*)'")]
        public void GivenTodayIs(DateTime currentTime)
        {
            var mockDateTime = ScenarioContext.Current.Get<Mock<IDateTime>>();
            mockDateTime.Setup(it => it.GetCurrentTime()).Returns(currentTime);
        }

        [Given(@"System have ClassRoom collection with JSON format are")]
        public void GivenSystemHaveClassRoomCollectionWithJSONFormatAre(string multilineText)
        {
            var classRooms = JsonConvert.DeserializeObject<IEnumerable<ClassRoom>>(multilineText);
            var mockClassRoomRepo = ScenarioContext.Current.Get<Moq.Mock<IClassRoomRepository>>();
            mockClassRoomRepo.Setup(it => it.GetClassRoomById(It.IsAny<string>()))
                .Returns<string>(id => classRooms.FirstOrDefault(it => it.id == id));
        }

        [Given(@"System have LikeLesson collection with JSON format are")]
        public void GivenSystemHaveLikeLessonCollectionWithJSONFormatAre(string multilineText)
        {
            var likeLessons = JsonConvert.DeserializeObject<IEnumerable<LikeLesson>>(multilineText);
            var mockLikeLessonRepo = ScenarioContext.Current.Get<Moq.Mock<ILikeLessonRepository>>();
            mockLikeLessonRepo.Setup(it => it.GetLikeLessonsByLessonId(It.IsAny<string>()))
                .Returns<string>(id => likeLessons.Where(it => it.LessonId == id));
        }

        [Given(@"System have LessonCatalog collection with JSON format are")]
        public void GivenSystemHaveLessonCatalogCollectionWithJSONFormatAre(string multilineText)
        {
            var lessonCatalogs = JsonConvert.DeserializeObject<IEnumerable<LessonCatalog>>(multilineText);
            var mockLessonCatalogRepo = ScenarioContext.Current.Get<Moq.Mock<ILessonCatalogRepository>>();
            mockLessonCatalogRepo.Setup(it => it.GetLessonCatalogById(It.IsAny<string>()))
                .Returns<string>(id => lessonCatalogs.Where(it => it.id == id).FirstOrDefault());
        }

        [Given(@"System have FriendRequest collection with JSON format are")]
        public void GivenSystemHaveFriendRequestCollectionWithJSONFormatAre(string multilineText)
        {
            var friendRequests = JsonConvert.DeserializeObject<IEnumerable<FriendRequest>>(multilineText);
            var mockFriendRequestRepo = ScenarioContext.Current.Get<Moq.Mock<IFriendRequestRepository>>();
            mockFriendRequestRepo.Setup(it => it.GetFriendRequestByUserProfileId(It.IsAny<string>()))
                .Returns<string>(id => friendRequests.Where(it => it.FromUserProfileId == id));
        }

        [Given(@"System have Comment collection with JSON format are")]
        public void GivenSystemHaveCommentCollectionWithJSONFormatAre(string multilineText)
        {
            var comments = JsonConvert.DeserializeObject<IEnumerable<Comment>>(multilineText);
            var mockCommentRepo = ScenarioContext.Current.Get<Moq.Mock<ICommentRepository>>();
            mockCommentRepo.Setup(it => it.GetCommentsByLessonId(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns<string, IEnumerable<string>>((id, creators) => comments.Where(it => it.LessonId == id && creators.Contains(it.CreatedByUserProfileId)));
            mockCommentRepo.Setup(it => it.GetCommentById(It.IsAny<string>()))
                .Returns<string>(id => comments.FirstOrDefault(it => it.id == id));
        }

        [Given(@"System have UserActivity collection with JSON format are")]
        public void GivenSystemHaveUserActivityCollectionWithJSONFormatAre(string multilineText)
        {
            var userActivities = JsonConvert.DeserializeObject<IEnumerable<UserActivity>>(multilineText);
            var mockUserActivityRepo = ScenarioContext.Current.Get<Moq.Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.GetUserActivityByUserProfileIdAndClassRoomId(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((userprofile, classRoomId) => userActivities.FirstOrDefault(it => it.UserProfileId == userprofile && it.ClassRoomId == classRoomId));
        }

        [Given(@"System have LikeComment collection with JSON format are")]
        public void GivenSystemHaveLikeCommentCollectionWithJSONFormatAre(string multilineText)
        {
            var likeComments = JsonConvert.DeserializeObject<IEnumerable<LikeComment>>(multilineText);
            var mockLikeCommentRepo = ScenarioContext.Current.Get<Moq.Mock<ILikeCommentRepository>>();
            mockLikeCommentRepo.Setup(it => it.GetLikeCommentByCommentId(It.IsAny<string>()))
                .Returns<string>(id => likeComments.Where(it => it.CommentId == id));
        }

        [Given(@"System have LikeDiscussion collection with JSON format are")]
        public void GivenSystemHaveLikeDiscussionCollectionWithJSONFormatAre(string multilineText)
        {
            var likeDiscussions = JsonConvert.DeserializeObject<IEnumerable<LikeDiscussion>>(multilineText);
            var mockLikeDiscussionRepo = ScenarioContext.Current.Get<Moq.Mock<ILikeDiscussionRepository>>();
            mockLikeDiscussionRepo.Setup(it => it.GetLikeDiscussionByDiscusionId(It.IsAny<string>()))
                .Returns<string>(id => likeDiscussions.Where(it => it.DiscussionId == id));
        }
    }
}
