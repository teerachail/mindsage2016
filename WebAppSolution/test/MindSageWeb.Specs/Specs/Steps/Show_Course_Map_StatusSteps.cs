using MindSageWeb.Controllers;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using MindSageWeb.Repositories.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace MindSageWeb.Specs.Steps
{
    [Binding]
    public class Show_Course_Map_StatusSteps
    {
        [When(@"UserProfileId '(.*)' reuqest course map status of ClassRoom: '(.*)'")]
        public void WhenUserProfileIdReuqestCourseMapStatusOfClassRoom(string userprofileId, string classRoomId)
        {
            var mycourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var result = mycourseCtrl.GetStatus(userprofileId, classRoomId);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send course map status back are")]
        public void ThenSystemSendCourseMapStatusBackAre(Table table)
        {
            var expected = table.CreateSet<CourseMapStatusRespond>();
            var actual = ScenarioContext.Current.Get<IEnumerable<CourseMapStatusRespond>>();

            Assert.Equal(expected.Count(), actual.Count());
            var expectedString = JsonConvert.SerializeObject(expected);
            var actualString = JsonConvert.SerializeObject(actual);

            Assert.Equal(expectedString, actualString);
        }
    }
}
