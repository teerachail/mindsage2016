using MindSageWeb.Controllers;
using MindSageWeb.Repositories;
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
    public class Show_Course_Map_ContentSteps
    {
        [When(@"UserProfileId '(.*)' reuqest course map content of ClassRoom: '(.*)' and ClassCalendarId: '(.*)'")]
        public void WhenUserProfileIdReuqestCourseMapContentOfClassRoomAndClassCalendarId(string userprofileId, string classRoomId, string classCalendarId)
        {
            var mycourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var result = mycourseCtrl.Get(userprofileId.GetMockStrinValue(), classRoomId.GetMockStrinValue(), classCalendarId.GetMockStrinValue()).Result;
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send course map content back are")]
        public void ThenSystemSendCourseMapContentBackAre(Table table)
        {
            var expected = table.CreateSet<CourseMapContentRespond>();
            var actual = ScenarioContext.Current.Get<IEnumerable<CourseMapContentRespond>>();

            Assert.Equal(expected.Count(), actual.Count());
            var expectedString = JsonConvert.SerializeObject(expected);
            var actualString = JsonConvert.SerializeObject(actual);

            Assert.Equal(expectedString, actualString);
        }

        [Then(@"System send course map content collection with JSON format are")]
        public void ThenSystemSendCourseMapContentCollectionWithJSONFormatAre(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<IEnumerable<CourseMapContentRespond>>(multilineText);
            var actual = ScenarioContext.Current.Get<IEnumerable<CourseMapContentRespond>>();

            var expectedString = JsonConvert.SerializeObject(expected);
            var actualString = JsonConvert.SerializeObject(actual);

            Assert.Equal(expectedString, actualString);
        }
    }
}
