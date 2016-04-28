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
    public class Enter_LessonSteps
    {
        [When(@"UserProfile '(.*)' enter LessonId '(.*)' from ClassRoom '(.*)'")]
        public void WhenUserProfileEnterLessonIdFromClassRoom(string userprofileId, string lessonId, string classRoomId)
        {
            var lessonCtrl = ScenarioContext.Current.Get<LessonController>();
            var result = lessonCtrl.Get(lessonId.GetMockStrinValue(), classRoomId.GetMockStrinValue(), userprofileId.GetMockStrinValue());
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send lesson information back with JSON format is")]
        public void ThenSystemSendLessonInformationBackWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<LessonContentRespond>(multilineText);
            var actual = ScenarioContext.Current.Get<LessonContentRespond>();

            Assert.Equal(expected.id, actual.id);
            Assert.Equal(expected.Order, actual.Order);
            Assert.Equal(expected.SemesterName, actual.SemesterName);
            Assert.Equal(expected.UnitNo, actual.UnitNo);
            Assert.Equal(expected.CourseCatalogId, actual.CourseCatalogId);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.ShortDescription, actual.ShortDescription);
            Assert.Equal(expected.MoreDescription, actual.MoreDescription);
            Assert.Equal(expected.ShortTeacherLessonPlan, actual.ShortTeacherLessonPlan);
            Assert.Equal(expected.MoreTeacherLessonPlan, actual.MoreTeacherLessonPlan);
            Assert.Equal(expected.PrimaryContentURL, actual.PrimaryContentURL);
            Assert.Equal(expected.PrimaryContentDescription, actual.PrimaryContentDescription);
            Assert.Equal(expected.CreatedDate, actual.CreatedDate);
            Assert.Equal(expected.DeletedDate, actual.DeletedDate);
            Assert.Equal(expected.CourseMessage, actual.CourseMessage);
            Assert.Equal(expected.IsTeacher, actual.IsTeacher);
            Assert.Equal(expected.TotalLikes, actual.TotalLikes);

            Assert.Equal(expected.Advertisments.Count(), actual.Advertisments.Count());
            var expectedAds = expected.Advertisments.ToList();
            var actualAds = actual.Advertisments.ToList();
            for (int index = 0; index < expectedAds.Count; index++)
            {
                Assert.Equal(expectedAds[index].id, actualAds[index].id);
                Assert.Equal(expectedAds[index].ImageUrl, actualAds[index].ImageUrl);
                Assert.Equal(expectedAds[index].LinkUrl, actualAds[index].LinkUrl);
                Assert.Equal(expectedAds[index].CreatedDate, actualAds[index].CreatedDate);
                Assert.Equal(expectedAds[index].DeletedDate, actualAds[index].DeletedDate);
            }

            Assert.Equal(expected.ExtraContents.Count(), actual.ExtraContents.Count());
            var expectedExtraContents = expected.ExtraContents.ToList();
            var actualExtraContents = actual.ExtraContents.ToList();
            for (int index = 0; index < expectedExtraContents.Count; index++)
            {
                Assert.Equal(expectedExtraContents[index].id, actualExtraContents[index].id);
                Assert.Equal(expectedExtraContents[index].ContentURL, actualExtraContents[index].ContentURL);
                Assert.Equal(expectedExtraContents[index].Description, actualExtraContents[index].Description);
                Assert.Equal(expectedExtraContents[index].IconURL, actualExtraContents[index].IconURL);
            }
        }

        [Then(@"System doesn't send lesson information back")]
        public void ThenSystemDoesnTSendLessonInformationBack()
        {
            var actual = ScenarioContext.Current.Get<LessonContentRespond>();
            Assert.Null(actual);
        }
    }
}
