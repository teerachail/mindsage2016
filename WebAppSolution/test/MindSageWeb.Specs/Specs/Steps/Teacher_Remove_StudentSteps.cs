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
    public class Teacher_Remove_StudentSteps
    {
        [When(@"UserProfile '(.*)' remove StudentId '(.*)' from ClassRoom '(.*)'")]
        public void WhenUserProfileRemoveStudentIdFromClassRoom(string userprofileId, string removeStudentId, string classRoomId)
        {
            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

            var mockUserprofileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserprofileRepo.Setup(it => it.UpsertUserProfile(It.IsAny<UserProfile>()));

            userprofileId = userprofileId.GetMockStrinValue();
            removeStudentId = removeStudentId.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();

            var myCourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            myCourseCtrl.RemoveStudent(new RemoveStudentRequest
            {
                ClassRoomId = classRoomId,
                RemoveUserProfileId = removeStudentId,
                UserProfileId = userprofileId
            });
        }

        [Then(@"System upsert UserActivity collection with JSON format is")]
        public void ThenSystemUpsertUserActivityCollectionWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<UserActivity>(multilineText);
            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Verify(it => it.UpsertUserActivity(It.Is<UserActivity>(useractivity =>
                useractivity.id == expected.id
                && useractivity.UserProfileId == expected.UserProfileId
                && useractivity.ClassRoomId == expected.ClassRoomId
                && useractivity.DeletedDate == expected.DeletedDate
                && JsonConvert.SerializeObject(useractivity.LessonActivities) == JsonConvert.SerializeObject(expected.LessonActivities)
            )));
        }

        [Then(@"System doesn't upsert UserProfile")]
        public void ThenSystemDoesnTUpsertUserProfile()
        {
            var mockUserprofileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserprofileRepo.Verify(it => it.UpsertUserProfile(It.IsAny<UserProfile>()), Times.Never);
        }

        [Then(@"System doesn't upsert UserActivity")]
        public void ThenSystemDoesnTUpsertUserActivity()
        {
            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Verify(it => it.UpsertUserActivity(It.IsAny<UserActivity>()), Times.Never);
        }
    }
}
