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
    public class Show_Friends_List_StudentSteps
    {
        [When(@"UserProfile '(.*)' request friend list from ClassRoom: '(.*)'")]
        public void WhenUserProfileRequestFriendListFromClassRoom(string userprofile, string classRoomId)
        {
            userprofile = userprofile.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();

            var friendCtrl = ScenarioContext.Current.Get<FriendController>();
            var result = friendCtrl.Get(userprofile, classRoomId);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send friend list with JSON format are")]
        public void ThenSystemSendFriendListWithJSONFormatAre(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<IEnumerable<GetFriendListRespond>>(multilineText);
            var actual = ScenarioContext.Current.Get<IEnumerable<GetFriendListRespond>>();

            var expectedString = JsonConvert.SerializeObject(expected.OrderBy(it => it.Name));
            var actualString = JsonConvert.SerializeObject(actual.OrderBy(it => it.Name));
            Assert.Equal(expectedString, actualString);
        }
    }
}
