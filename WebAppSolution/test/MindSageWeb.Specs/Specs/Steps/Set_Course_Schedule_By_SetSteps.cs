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
    public class Set_Course_Schedule_By_SetSteps
    {
        [When(@"User set course schedule by set by JSON format is")]
        public void WhenUserSetCourseScheduleBySetByJSONFormatIs(string multilineText)
        {
            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Setup(it => it.UpsertClassCalendar(It.IsAny<ClassCalendar>()));

            var body = JsonConvert.DeserializeObject<SetScheduleWithWeekRequest>(multilineText);
            body.UserProfileId = body.UserProfileId.GetMockStrinValue();
            body.ClassRoomId = body.ClassRoomId.GetMockStrinValue();
            var myCourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            var result = myCourseCtrl.SetScheduleWithWeek(body);
            ScenarioContext.Current.Set(result);
        }
    }
}
