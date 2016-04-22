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
    public class Show_Lesson_CommentsSteps
    {
        [When(@"UserProfile '(.*)' request comment from the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileRequestCommentFromTheLessonOfClassRoom(string userprofile, string lessonId, string classRoomId)
        {
            userprofile = userprofile.GetMockStrinValue();
            lessonId = lessonId.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();

            var lessonCtrl = ScenarioContext.Current.Get<LessonController>();
            var result = lessonCtrl.Comments(lessonId, classRoomId, userprofile);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send lesson's comment with JSON format is")]
        public void ThenSystemSendLessonSCommentWithJSONFormatIs(string multilineText)
        {
            var expectedObj = JsonConvert.DeserializeObject<GetCommentRespond>(multilineText);
            var actualObj = ScenarioContext.Current.Get<GetCommentRespond>();

            var expectedString = JsonConvert.SerializeObject(expectedObj);
            var actualString = JsonConvert.SerializeObject(actualObj);

            Assert.Equal(expectedString, actualString);
        }
    }
}
