﻿using MindSageWeb.Controllers;
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
    public class Add_New_CourseSteps
    {
        [When(@"UserProfile '(.*)' Add new course by using code '(.*)' and grade '(.*)'")]
        public void WhenUserProfileAddNewCourseByUsingCodeAndGrade(string userprofileId, string code, string grade)
        {
            var mockUserProfileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserProfileRepo.Setup(it => it.UpsertUserProfile(It.IsAny<UserProfile>()));

            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

            var mycourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var body = new AddCourseRequest
            {
                UserProfileId = userprofileId,
                Code = code,
                Grade = grade
            };
            mycourseCtrl.AddCourse(body);
        }

        [Then(@"System upsert user id '(.*)' UserProfile's subscriptions collection with JSON format are")]
        public void ThenSystemUpsertUserIdUserProfileSSubscriptionsCollectionWithJSONFormatAre(string userprofileId, string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<IEnumerable<UserProfile.Subscription>>(multilineText).ToList();
            Func<List<UserProfile.Subscription> ,bool> validateUpsertFunc = subscriptions =>
            {
                var collectionAreEqual = subscriptions.Count == expected.Count;
                if (!collectionAreEqual) return false;

                for (int elementIndex = 0; elementIndex < expected.Count; elementIndex++)
                {
                    var areAllEqual = !string.IsNullOrEmpty(subscriptions[elementIndex].id)
                        && expected[elementIndex].Role == subscriptions[elementIndex].Role
                        && expected[elementIndex].DeletedDate == subscriptions[elementIndex].DeletedDate
                        && expected[elementIndex].CreatedDate == subscriptions[elementIndex].CreatedDate
                        && expected[elementIndex].ClassRoomName == subscriptions[elementIndex].ClassRoomName
                        && expected[elementIndex].ClassRoomId == subscriptions[elementIndex].ClassRoomId
                        && expected[elementIndex].ClassCalendarId == subscriptions[elementIndex].ClassCalendarId;
                    if (!areAllEqual) return false;
                }

                return true;
            };

            var mockUserProfileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserProfileRepo.Verify(it => it.UpsertUserProfile(It.Is<UserProfile>(userprofile =>
                userprofile.id == userprofileId
                && validateUpsertFunc(userprofile.Subscriptions.ToList())
            )));
        }

        [Then(@"System upsert user id '(.*)' UserActivity's LessonActivities collection with JSON format are")]
        public void ThenSystemUpsertUserIdUserActivitySLessonActivitiesCollectionWithJSONFormatAre(string userprofileId, string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<IEnumerable<UserActivity.LessonActivity>>(multilineText).ToList();
            Func<List<UserActivity.LessonActivity>, bool> validateUpsertFunc = subscriptions =>
            {
                var collectionAreEqual = subscriptions.Count == expected.Count;
                if (!collectionAreEqual) return false;

                for (int elementIndex = 0; elementIndex < expected.Count; elementIndex++)
                {
                    var areAllEqual = !string.IsNullOrEmpty(subscriptions[elementIndex].id)
                        && expected[elementIndex].LessonId == subscriptions[elementIndex].LessonId
                        && expected[elementIndex].SawContentIds.Count() == subscriptions[elementIndex].SawContentIds.Count()
                        && expected[elementIndex].SawContentIds.All(it => subscriptions[elementIndex].SawContentIds.Contains(it))
                        && expected[elementIndex].ParticipationAmount == subscriptions[elementIndex].ParticipationAmount
                        && expected[elementIndex].TotalContentsAmount == subscriptions[elementIndex].TotalContentsAmount
                        && expected[elementIndex].CreatedCommentAmount == subscriptions[elementIndex].CreatedCommentAmount
                        && expected[elementIndex].BeginDate == subscriptions[elementIndex].BeginDate;
                    if (!areAllEqual) return false;
                }

                return true;
            };

            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Verify(it => it.UpsertUserActivity(It.Is<UserActivity>(useractivity =>
                !string.IsNullOrEmpty(useractivity.id)
                && useractivity.UserProfileId == userprofileId
                && validateUpsertFunc(useractivity.LessonActivities.ToList())
            )));
        }
    }
}
