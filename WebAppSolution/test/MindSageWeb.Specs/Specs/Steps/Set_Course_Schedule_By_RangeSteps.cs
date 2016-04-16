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
    public class Set_Course_Schedule_By_RangeSteps
    {
        [When(@"User '(.*)' set schedule of ClassRoomId '(.*)' FromDate '(.*)' ToDate '(.*)' IsHoliday '(.*)' IsShift '(.*)'")]
        public void WhenUserSetScheduleOfClassRoomIdFromDateToDateTrueFalse(string userprofileId, string classRoomId, DateTime fromDate, string toDateString, bool isHoliday, bool isShift)
        {
            userprofileId = userprofileId.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();

            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Setup(it => it.UpsertClassCalendar(It.IsAny<ClassCalendar>()));

            DateTime toDateValue;
            DateTime? toDate = DateTime.TryParse(toDateString, out toDateValue) ? new Nullable<DateTime>(toDateValue) : null;

            var body = new SetScheduleWithRangeRequest
            {
                ClassRoomId = classRoomId,
                FromDate = fromDate,
                ToDate = toDate,
                IsHoliday = isHoliday,
                IsShift = isShift,
                UserProfileId = userprofileId
            };
            var myCourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var result = myCourseCtrl.SetScheduleWithRange(body);
            ScenarioContext.Current.Set(result);
        }
    }
}
