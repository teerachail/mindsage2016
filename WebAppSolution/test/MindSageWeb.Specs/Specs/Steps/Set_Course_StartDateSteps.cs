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
    public class Set_Course_StartDateSteps
    {
        [When(@"User '(.*)' set course start date '(.*)' from ClassRoomId '(.*)'")]
        public void WhenUserSetCourseStartDateFromClassRoomId(string userprofileId, DateTime beginDate, string classRoomId)
        {
            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Setup(it => it.UpsertClassCalendar(It.IsAny<ClassCalendar>()));

            userprofileId = userprofileId.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();

            var myCourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var result = myCourseCtrl.SetStartDate(new SetStartDateRequest
            {
                BeginDate = beginDate,
                ClassRoomId = classRoomId,
                UserProfileId = userprofileId
            });
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System set course ClassCalendar collection with JSON format is")]
        public void ThenSystemSetCourseClassCalendarCollectionWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<ClassCalendar>(multilineText);

            Func<ClassCalendar, bool> validateClassCalendarFunc = actual =>
            {
                Assert.Equal(expected.BeginDate?.ToUniversalTime(), actual.BeginDate?.ToUniversalTime());
                Assert.Equal(expected.ClassRoomId, actual.ClassRoomId);
                Assert.Equal(expected.CloseDate?.ToUniversalTime(), actual.CloseDate?.ToUniversalTime());
                Assert.Equal(expected.CreatedDate, actual.CreatedDate);
                Assert.Equal(expected.DeletedDate, actual.DeletedDate);
                Assert.Equal(expected.ExpiredDate?.ToUniversalTime(), actual.ExpiredDate?.ToUniversalTime());

                var expectedHoliday = JsonConvert.SerializeObject(expected.Holidays.Select(it => it.ToUniversalTime()));
                var actualHoliday = JsonConvert.SerializeObject(actual.Holidays.Select(it => it.ToUniversalTime()));
                Assert.Equal(expectedHoliday, actualHoliday);

                var expectedShiftDays = JsonConvert.SerializeObject(expected.ShiftDays.Select(it => it.ToUniversalTime()));
                var actualShiftDays = JsonConvert.SerializeObject(actual.ShiftDays.Select(it => it.ToUniversalTime()));
                Assert.Equal(expectedShiftDays, actualShiftDays);

                Assert.Equal(expected.id, actual.id);
                return true;
            };

            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Verify(it => it.UpsertClassCalendar(It.Is<ClassCalendar>(actual =>
                validateClassCalendarFunc(actual)
            )));
        }

        [Then(@"System doesn't set course ClassCalendar")]
        public void ThenSystemDoesnTSetCourseClassCalendar()
        {
            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Verify(it => it.UpsertClassCalendar(It.IsAny<ClassCalendar>()), Times.Never());
        }
    }
}
