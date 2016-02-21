using MindSageWeb.Controllers;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace MindSageWeb.Specs.Steps
{
    [Binding]
    public class Send_Friend_RequestSteps
    {
        [When(@"UserProfile '(.*)' send friend request to UserProfile '(.*)'")]
        public void WhenUserProfileSendFriendRequestToUserProfile(string fromUserprofileId, string toUserProfileId)
        {
            var mockFriendRequestRepo = ScenarioContext.Current.Get<Mock<IFriendRequestRepository>>();
            mockFriendRequestRepo.Setup(it => it.UpsertFriendRequest(It.IsAny<FriendRequest>()));

            var friendCtrl = ScenarioContext.Current.Get<FriendController>();
            var body = new SendFriendRequest
            {
                FromUserProfileId = fromUserprofileId,
                ToUserProfileId = toUserProfileId,
            };
            friendCtrl.Post(body);
        }

        [When(@"UserProfile '(.*)' respond friend request to UserProfile '(.*)' by RequestId '(.*)' IsAccept '(.*)'")]
        public void WhenUserProfileRespondFriendRequestToUserProfileByRequestIdIsAccept(string fromUserprofileId, string toUserProfileId, string friendReqId, bool isAccept)
        {
            var mockFriendRequestRepo = ScenarioContext.Current.Get<Mock<IFriendRequestRepository>>();
            mockFriendRequestRepo.Setup(it => it.UpsertFriendRequest(It.IsAny<FriendRequest>()));

            var friendCtrl = ScenarioContext.Current.Get<FriendController>();
            var body = new SendFriendRequest
            {
                FromUserProfileId = fromUserprofileId,
                ToUserProfileId = toUserProfileId,
                RequestId = friendReqId,
                IsAccept = isAccept
            };
            friendCtrl.Post(body);
        }

        [Then(@"System upsert FriendRequest with JSON format is")]
        public void ThenSystemUpsertFriendRequestWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<FriendRequest>(multilineText);
            var mockFriendRequestRepo = ScenarioContext.Current.Get<Mock<IFriendRequestRepository>>();
            mockFriendRequestRepo.Verify(it => it.UpsertFriendRequest(It.Is<FriendRequest>(req=>
                !string.IsNullOrEmpty(req.id)
                && req.FromUserProfileId == expected.FromUserProfileId
                && req.ToUserProfileId == expected.ToUserProfileId
                && req.Status == expected.Status
                && req.CreatedDate == expected.CreatedDate
                && req.AcceptedDate == expected.AcceptedDate
                && req.DeletedDate == expected.DeletedDate
            )));
        }
    }
}
