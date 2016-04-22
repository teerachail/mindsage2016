using MindSageWeb.Controllers;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace MindSageWeb.Specs.Steps
{
    [Binding]
    public class Click_Like_A_LessonSteps
    {
        [When(@"UserProfileId '(.*)' click the like button in the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileIdClickTheLikeButtonInTheLessonOfClassRoom(string userprofileId, string lessonId, string classRoomId)
        {
            var mockLikeLessonRepo = ScenarioContext.Current.Get<Moq.Mock<ILikeLessonRepository>>();
            mockLikeLessonRepo.Setup(it => it.UpsertLikeLesson(It.IsAny<LikeLesson>()));

            var mockClassRoomRepo = ScenarioContext.Current.Get<Moq.Mock<IClassRoomRepository>>();
            mockClassRoomRepo.Setup(it => it.UpsertClassRoom(It.IsAny<ClassRoom>()));

            var mockUserActivityRepo = ScenarioContext.Current.Get<Moq.Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

            var myCourseCtrl = ScenarioContext.Current.Get<LessonController>();
            myCourseCtrl.Post(new LikeLessonRequest
            {
                ClassRoomId = classRoomId,
                LessonId = lessonId,
                UserProfileId = userprofileId
            });
        }

        [Then(@"System update total likes in the lesson '(.*)' of ClassRoom '(.*)' to '(.*)' likes")]
        public void ThenSystemUpdateTotalLikesInTheLessonOfClassRoomToLikes(string lessonId, string classRoomId, int totalLikes)
        {
            var mockClassRoomRepo = ScenarioContext.Current.Get<Moq.Mock<IClassRoomRepository>>();
            mockClassRoomRepo.Verify(it => it.UpsertClassRoom(It.Is<ClassRoom>(cr =>
                cr.id == classRoomId
                && cr.Lessons.First(l => l.id == lessonId).TotalLikes == totalLikes
            )));
        }

        [Then(@"System add new LikeLesson by JSON format is")]
        public void ThenSystemAddNewLikeLessonByJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<LikeLesson>(multilineText);
            var mockLikeLessonRepo = ScenarioContext.Current.Get<Moq.Mock<ILikeLessonRepository>>();
            mockLikeLessonRepo.Verify(it => it.UpsertLikeLesson(It.Is<LikeLesson>(ll =>
                ll.ClassRoomId == expected.ClassRoomId
                && ll.LessonId == expected.LessonId
                && ll.LikedByUserProfileId == expected.LikedByUserProfileId
                && ll.CreatedDate == expected.CreatedDate
                && !ll.DeletedDate.HasValue
            )));
        }

        [Then(@"System update UserActivity collection with JSON format is")]
        public void ThenSystemUpdateUserActivityCollectionWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<UserActivity>(multilineText);
            Func<List<UserActivity.LessonActivity>, bool> validateLessonActivityFunc = actualLessonActivities =>
            {
                var expectedLessonActivities = expected.LessonActivities.ToList();
                for (int index = 0; index < expectedLessonActivities.Count; index++)
                {
                    Assert.Equal(expectedLessonActivities[index].id, actualLessonActivities[index].id);
                    Assert.Equal(expectedLessonActivities[index].BeginDate, actualLessonActivities[index].BeginDate);
                    Assert.Equal(expectedLessonActivities[index].TotalContentsAmount, actualLessonActivities[index].TotalContentsAmount);
                    Assert.Equal(expectedLessonActivities[index].SawContentIds, actualLessonActivities[index].SawContentIds);
                    Assert.Equal(expectedLessonActivities[index].CreatedCommentAmount, actualLessonActivities[index].CreatedCommentAmount);
                    Assert.Equal(expectedLessonActivities[index].ParticipationAmount, actualLessonActivities[index].ParticipationAmount);
                    Assert.Equal(expectedLessonActivities[index].LessonId, actualLessonActivities[index].LessonId);
                }
                return true;
            };
            Func<UserActivity, bool> validateUpsertUserActivityFunc = actual =>
            {
                Assert.NotNull(actual.id);
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
                Assert.Equal(expected.CommentPercentage, actual.CommentPercentage);
                Assert.Equal(expected.OnlineExtrasPercentage, actual.OnlineExtrasPercentage);
                Assert.Equal(expected.SocialParticipationPercentage, actual.SocialParticipationPercentage);
                Assert.True(validateLessonActivityFunc(actual.LessonActivities.ToList()));
                return true;
            };
            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Verify(it => it.UpsertUserActivity(It.Is<UserActivity>(activity =>
                validateUpsertUserActivityFunc(activity)
            )));
        }
    }
}
