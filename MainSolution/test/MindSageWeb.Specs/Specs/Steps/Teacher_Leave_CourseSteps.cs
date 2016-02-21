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
    public class Teacher_Leave_CourseSteps
    {
        [When(@"UserProfile '(.*)' leave class room '(.*)'")]
        public void WhenUserProfileLeaveClassRoom(string userprofileId, string classRoomId)
        {
            var mockClassRoomRepo = ScenarioContext.Current.Get<Mock<IClassRoomRepository>>();
            mockClassRoomRepo.Setup(it => it.UpsertClassRoom(It.IsAny<ClassRoom>()));

            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Setup(it => it.UpsertClassCalendar(It.IsAny<ClassCalendar>()));

            var mockUserProfileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserProfileRepo.Setup(it => it.UpsertUserProfile(It.IsAny<UserProfile>()));

            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

            var mycourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var body = new LeaveCourseRequest
            {
                ClassRoomId = classRoomId,
                UserProfileId = userprofileId
            };
            mycourseCtrl.Leave(body);
        }

        [Then(@"System upsert ClassRoom with JSON format is")]
        public void ThenSystemUpsertClassRoomWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<ClassRoom>(multilineText);

            var mockClassRoomRepo = ScenarioContext.Current.Get<Mock<IClassRoomRepository>>();
            mockClassRoomRepo.Verify(it => it.UpsertClassRoom(It.Is<ClassRoom>(actual =>
                JsonConvert.SerializeObject(actual) == JsonConvert.SerializeObject(expected)
            )));
        }

        [Then(@"System upsert ClassCalendar with JSON format is")]
        public void ThenSystemUpsertClassCalendarWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<ClassCalendar>(multilineText);

            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Verify(it => it.UpsertClassCalendar(It.Is<ClassCalendar>(actual =>
                JsonConvert.SerializeObject(actual) == JsonConvert.SerializeObject(expected)
            )));
        }

        [Then(@"System upsert UserActivity with JSON format is")]
        public void ThenSystemUpsertUserActivityWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<UserActivity>(multilineText);

            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Verify(it => it.UpsertUserActivity(It.Is<UserActivity>(actual=>
                JsonConvert.SerializeObject(actual) == JsonConvert.SerializeObject(expected)
            )));
        }
    }
}
