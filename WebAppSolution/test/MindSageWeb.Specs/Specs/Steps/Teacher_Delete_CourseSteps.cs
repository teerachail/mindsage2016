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
    public class Teacher_Delete_CourseSteps
    {
        [When(@"UserProfile '(.*)' delete ClassRoom '(.*)'")]
        public void WhenUserProfileDeleteClassRoom(string userprofileId, string classRoomId)
        {
            var mockClassRoomRepo = ScenarioContext.Current.Get<Mock<IClassRoomRepository>>();
            mockClassRoomRepo.Setup(it => it.UpsertClassRoom(It.IsAny<ClassRoom>()));

            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Setup(it => it.UpsertClassCalendar(It.IsAny<ClassCalendar>()));

            var mockUserprofileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserprofileRepo.Setup(it => it.UpsertUserProfile(It.IsAny<UserProfile>()));

            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

            var mockStudentKeyRepo = ScenarioContext.Current.Get<Mock<IStudentKeyRepository>>();
            mockStudentKeyRepo.Setup(it => it.UpsertStudentKey(It.IsAny<StudentKey>()));

            userprofileId = userprofileId.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();

            var myCourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            myCourseCtrl.Leave(new LeaveCourseRequest
            {
                UserProfileId = userprofileId,
                ClassRoomId = classRoomId
            });
        }
    }
}
