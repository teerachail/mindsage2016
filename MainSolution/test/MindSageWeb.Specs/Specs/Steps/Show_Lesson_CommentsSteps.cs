using MindsageWeb.Controllers;
using MindsageWeb.Repositories;
using MindsageWeb.Repositories.Models;
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
            var lessonCtrl = ScenarioContext.Current.Get<LessonController>();
            var result = lessonCtrl.Comments(lessonId, classRoomId, userprofile);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send lesson's comment with JSON format are")]
        public void ThenSystemSendLessonSCommentWithJSONFormatAre(string multilineText)
        {
            var expectedObj = JsonConvert.DeserializeObject<IEnumerable<GetCommentRespond>>(multilineText);
            var actualObj = ScenarioContext.Current.Get<IEnumerable<GetCommentRespond>>();

            var expectedString = JsonConvert.SerializeObject(expectedObj);
            var actualString = JsonConvert.SerializeObject(actualObj);

            Assert.Equal(expectedString, actualString);
        }
    }
}
