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
    public class Add_New_Course_By_Teacher_CodeSteps
    {
        [When(@"UserProfile '(.*)' Add new course by using teteacher code '(.*)' and grade '(.*)'")]
        public async Task WhenUserProfileAddNewCourseByUsingTeteacherCodeAndGrade(string userprofileId, string code, string grade)
        {
            var mockUserProfileRepo = ScenarioContext.Current.Get<Mock<IUserProfileRepository>>();
            mockUserProfileRepo.Setup(it => it.UpsertUserProfile(It.IsAny<UserProfile>()));

            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

            var noneTask = Task.Run(() => { });
            var mockClassRoomRepo = ScenarioContext.Current.Get<Mock<IClassRoomRepository>>();
            mockClassRoomRepo.Setup(it => it.CreateNewClassRoom(It.IsAny<ClassRoom>()))
                .Returns<ClassRoom>(it => noneTask);

            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Setup(it => it.CreateNewClassCalendar(It.IsAny<ClassCalendar>()))
                 .Returns<ClassCalendar>(it => noneTask);

            var mockStudentKeyRepo = ScenarioContext.Current.Get<Mock<IStudentKeyRepository>>();
            mockStudentKeyRepo.Setup(it => it.CreateNewStudentKey(It.IsAny<StudentKey>()))
                .Returns<StudentKey>(it => noneTask);

            var mycourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var body = new AddCourseRequest
            {
                UserProfileId = userprofileId,
                Code = code,
                Grade = grade
            };
            await mycourseCtrl.AddCourse(body);
        }

        [Then(@"System add new teacher subscription for user id '(.*)' collection with JSON format are")]
        public void ThenSystemAddNewTeacherSubscriptionForUserIdCollectionWithJSONFormatAre(string userprofileId, string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<IEnumerable<UserProfile.Subscription>>(multilineText).ToList();
            Func<List<UserProfile.Subscription>, bool> validateUpsertFunc = subscriptions =>
            {
                var collectionAreEqual = subscriptions.Count == expected.Count;
                if (!collectionAreEqual) return false;

                for (int elementIndex = 0; elementIndex < expected.Count; elementIndex++)
                {
                    var areAllEqual = !string.IsNullOrEmpty(subscriptions[elementIndex].id)
                        && expected[elementIndex].Role == UserProfile.AccountRole.Teacher
                        && expected[elementIndex].DeletedDate == subscriptions[elementIndex].DeletedDate
                        && expected[elementIndex].CreatedDate == subscriptions[elementIndex].CreatedDate
                        && expected[elementIndex].ClassRoomName == subscriptions[elementIndex].ClassRoomName
                        && expected[elementIndex].CourseCatalogId == subscriptions[elementIndex].CourseCatalogId
                        && expected[elementIndex].LastActiveDate == subscriptions[elementIndex].LastActiveDate
                        && expected[elementIndex].LicenseId == subscriptions[elementIndex].LicenseId;
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

        [Then(@"System create new ClassCalendar with JSON format is")]
        public void ThenSystemCreateNewClassCalendarWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<ClassCalendar>(multilineText);

            Func<List<ClassCalendar.LessonCalendar>, bool> validateLessonCalendarFunc = lessonCalendars =>
            {
                var expectedLessonCalendars = expected.LessonCalendars.ToList();
                for (int index = 0; index < expectedLessonCalendars.Count; index++)
                {
                    Assert.NotNull(lessonCalendars[index].id);
                    Assert.Equal(expectedLessonCalendars[index].Order, lessonCalendars[index].Order);
                    Assert.Equal(expectedLessonCalendars[index].SemesterGroupName, lessonCalendars[index].SemesterGroupName);

                    Assert.Equal(expectedLessonCalendars[index].BeginDate, lessonCalendars[index].BeginDate);
                    Assert.Equal(expectedLessonCalendars[index].CreatedDate, lessonCalendars[index].CreatedDate);
                    Assert.Equal(expectedLessonCalendars[index].LessonId, lessonCalendars[index].LessonId);
                    Assert.Equal(expectedLessonCalendars[index].TopicOfTheDays.Count(), lessonCalendars[index].TopicOfTheDays.Count());
                }
                return true;
            };

            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Verify(it => it.CreateNewClassCalendar(It.Is<ClassCalendar>(actual =>
                !string.IsNullOrEmpty(actual.id)
                && !string.IsNullOrEmpty( actual.ClassRoomId)
                && actual.CreatedDate == expected.CreatedDate
                && !actual.DeletedDate.HasValue
                && !actual.Holidays.Any()
                && !actual.ShiftDays.Any()
                && actual.LessonCalendars.Count() == expected.LessonCalendars.Count()
                && validateLessonCalendarFunc(actual.LessonCalendars.ToList())
            )));
        }

        [Then(@"System create new StudentKey with JSON format is")]
        public void ThenSystemCreateNewStudentKeyWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<StudentKey>(multilineText);

            var mockStudentKeyRepo = ScenarioContext.Current.Get<Mock<IStudentKeyRepository>>();
            mockStudentKeyRepo.Verify(it => it.CreateNewStudentKey(It.Is<StudentKey>(actual =>
                !string.IsNullOrEmpty(actual.id)
                && actual.Grade == expected.Grade
                && !string.IsNullOrEmpty(actual.Code)
                && actual.CourseCatalogId == expected.CourseCatalogId
                && !string.IsNullOrEmpty(actual.ClassRoomId)
                && actual.CreatedDate == expected.CreatedDate
                && !actual.DeletedDate.HasValue
            )));
        }
    }
}
