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
    public class Show_Friend_List
    {
        [When(@"UserProfile '(.*)' request friend list from ClassRoom: '(.*)'")]
        public void WhenUserProfileRequestFriendListFromClassRoom(string userprofile, string classRoomId)
        {
            var friendCtrl = ScenarioContext.Current.Get<FriendController>();
            var result = friendCtrl.Get(userprofile, classRoomId);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send friend list with JSON format are")]
        public void ThenSystemSendFriendListWithJSONFormatAre(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<IEnumerable<GetFriendListRespond>>(multilineText);
            var actual = ScenarioContext.Current.Get<IEnumerable<GetFriendListRespond>>();

            var expectedString = JsonConvert.SerializeObject(expected);
            var actualString = JsonConvert.SerializeObject(actual);
            Assert.Equal(expectedString, actualString);
        }
    }
}
