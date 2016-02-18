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
            Assert.Equal(expected.FullTeacherLessonPlan, actual.FullTeacherLessonPlan);
            Assert.Equal(expected.CourseMessage, actual.CourseMessage);
            Assert.Equal(expected.TotalLikes, actual.TotalLikes);
            Assert.Equal(expected.IsTeacher, actual.IsTeacher);
        }
    }
}
