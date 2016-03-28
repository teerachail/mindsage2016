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
    public class Show_Lesson_ContentSteps
    {
        [When(@"UserProfile '(.*)' open the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileOpenTheLessonOfClassRoom(string userprofileId, string lessonId, string classRoomId)
        {
            var mockUserProfileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserProfileRepo.Setup(it => it.UpsertUserProfile(It.IsAny<UserProfile>()));
            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

            var lessonCtrl = ScenarioContext.Current.Get<LessonController>();
            var result = lessonCtrl.Get(lessonId, classRoomId, userprofileId);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send lesson's content with JSON format is")]
        public void ThenSystemSendLessonSContentWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<LessonContentRespond>(multilineText);
            var actual = ScenarioContext.Current.Get<LessonContentRespond>();

            Assert.Equal(expected.id, actual.id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.ShortTeacherLessonPlan, actual.ShortTeacherLessonPlan);
            Assert.Equal(expected.MoreTeacherLessonPlan, actual.MoreTeacherLessonPlan);
            Assert.Equal(expected.CourseMessage, actual.CourseMessage);
            Assert.Equal(expected.TotalLikes, actual.TotalLikes);
            Assert.Equal(expected.IsTeacher, actual.IsTeacher);
        }

        [Then(@"System update last active class room is '(.*)' for user '(.*)'")]
        public void ThenSystemUpdateLastActiveClassRoomIsForUser(string classRoomId, string userprofileId)
        {
            var mockUserProfileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserProfileRepo.Verify(it => it.UpsertUserProfile(It.Is<UserProfile>(actual =>
                actual.id == userprofileId
                && actual.Subscriptions.OrderBy(s => s.LastActiveDate).First().ClassRoomId == classRoomId
            )));
        }
    }
}
