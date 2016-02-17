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
    public class Show_DiscussionSteps
    {
        [When(@"UserProfile '(.*)' request discussion from comment '(.*)' in the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileRequestDiscussionFromCommentInTheLessonOfClassRoom(string userprofileId, string commentId, string lessonId, string classRoomId)
        {
            var lessonCtrl = ScenarioContext.Current.Get<LessonController>();
            var result = lessonCtrl.Discussions(lessonId, classRoomId, commentId, userprofileId);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send comment's discussion with JSON format are")]
        public void ThenSystemSendCommentSDiscussionWithJSONFormatAre(string multilineText)
        {
            var expectedObj = JsonConvert.DeserializeObject<IEnumerable<GetDiscussionRespond>>(multilineText);
            var actualObj = ScenarioContext.Current.Get<IEnumerable<GetDiscussionRespond>>();

            var expectedString = JsonConvert.SerializeObject(expectedObj);
            var actualString = JsonConvert.SerializeObject(actualObj);

            Assert.Equal(expectedString, actualString);
        }
    }
}
