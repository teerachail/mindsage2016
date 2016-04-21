using Microsoft.Extensions.Logging;
using MindSageWeb.Controllers;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;


namespace MindSageWeb.Specs.Steps
{
    [Binding]
    public class Search_Friends_Self_PurchaseSteps
    {
        [When(@"UserProfile '(.*)' search friends by keyword '(.*)' from ClassRoom: '(.*)'")]
        public void WhenUserProfileSearchFriendsByKeywordFromClassRoom(string userprofileId, string searchKeyWord, string classRoomId)
        {
            userprofileId = userprofileId.GetMockStrinValue();
            searchKeyWord = searchKeyWord.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();

            var friendCtrl = ScenarioContext.Current.Get<FriendController>();
            var result = friendCtrl.GetSearchResult(userprofileId, classRoomId, searchKeyWord);
            ScenarioContext.Current.Set(result);
        }
    }
}
