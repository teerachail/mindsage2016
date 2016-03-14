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
    public class Remove_A_StudentSteps
    {
        [When(@"UserProfile '(.*)' remove student '(.*)' from class room '(.*)'")]
        public void WhenUserProfileRemoveStudentFromClassRoom(string userprofileId, string removeUserProfileId, string classRoomId)
        {
            var mockUserProfileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserProfileRepo.Setup(it => it.UpsertUserProfile(It.IsAny<UserProfile>()));

            var mycourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var body = new RemoveStudentRequest
            {
                ClassRoomId = classRoomId,
                RemoveUserProfileId = removeUserProfileId,
                UserProfileId = userprofileId
            };
            mycourseCtrl.RemoveStudent(body);
        }

        [Then(@"System upsert UserProfile with JSON format is")]
        public void ThenSystemUpsertUserProfileWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<UserProfile>(multilineText);

            var mockUserProfileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserProfileRepo.Verify(it => it.UpsertUserProfile(It.Is<UserProfile>(up =>
                  JsonConvert.SerializeObject(up) == JsonConvert.SerializeObject(expected)
            )));
        }
    }
}
