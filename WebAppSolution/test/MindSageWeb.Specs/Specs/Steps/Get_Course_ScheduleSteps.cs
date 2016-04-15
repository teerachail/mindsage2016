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
    public class Get_Course_ScheduleSteps
    {
        [When(@"User '(.*)' request course schedule from ClassRoomId '(.*)'")]
        public void WhenUserRequestCourseScheduleFromClassRoomId(string userprofileId, string classRoomId)
        {
            userprofileId = userprofileId.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();

            var myCourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var result = myCourseCtrl.GetCourseSchedule(userprofileId, classRoomId);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send course schedule with JSON format is")]
        public void ThenSystemSendCourseScheduleWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<GetCourseScheduleRespond>(multilineText);
            var actual = ScenarioContext.Current.Get<GetCourseScheduleRespond>();

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Then(@"System send null back")]
        public void ThenSystemSendNullBack()
        {
            var actual = ScenarioContext.Current.Get<GetCourseScheduleRespond>();
            Assert.Null(actual);
        }
    }
}
