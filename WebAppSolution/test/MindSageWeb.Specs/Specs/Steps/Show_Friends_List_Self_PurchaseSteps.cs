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
    public class Show_Friends_List_Self_PurchaseSteps
    {
        [When(@"UserProfile '(.*)' request friend list from ClassRoom: '(.*)' by selfpurchase method")]
        public void WhenUserProfileRequestFriendListFromClassRoomBySelfpurchaseMethod(string userprofileId, string classRoomId)
        {
            userprofileId = userprofileId.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();
            var friendCtrl = ScenarioContext.Current.Get<FriendController>();
            var result = friendCtrl.GetFriendForSelfPurchase(userprofileId, classRoomId);
            ScenarioContext.Current.Set(result);
        }
    }
}
