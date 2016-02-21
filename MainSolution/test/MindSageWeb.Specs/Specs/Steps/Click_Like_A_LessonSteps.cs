using MindSageWeb.Controllers;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

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
            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Verify(it => it.UpsertUserActivity(It.Is<UserActivity>(activity =>
                JsonConvert.SerializeObject(activity) == JsonConvert.SerializeObject(expected)
            )));
        }
    }
}
