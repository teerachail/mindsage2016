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
    public class Show_Student_ListSteps
    {
        [When(@"UserProfile '(.*)' request student list from ClassRoom: '(.*)'")]
        public void WhenUserProfileRequestStudentListFromClassRoom(string userprofileId, string classRoomId)
        {
            userprofileId = userprofileId.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();

            var myCourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var result = myCourseCtrl.Students(userprofileId, classRoomId);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send student list are")]
        public void ThenSystemSendStudentListAre(Table table)
        {
            var expected = table.CreateSet<GetStudentListRespond>();
            var actual = ScenarioContext.Current.Get<IEnumerable<GetStudentListRespond>>();

            var expectedString = JsonConvert.SerializeObject(expected);
            var actualString = JsonConvert.SerializeObject(actual);

            Assert.Equal(expectedString, actualString);
        }
    }
}
