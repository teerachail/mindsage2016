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
    public class Teacher_Apply_Schedule_To_All_CoursesSteps
    {
        [When(@"UserProfile '(.*)' click Apply to all courses button from the ClassRoomId: '(.*)'")]
        public void WhenUserProfileClickApplyToAllCoursesButtonFromTheClassRoomId(string userprofileId, string classRoomId)
        {
            var myCourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            myCourseCtrl.ApplyToAllCourse(new ApplyToAllCourseRequest
            {
                UserProfileId = userprofileId.GetMockStrinValue(),
                ClassRoomId = classRoomId.GetMockStrinValue()
            });
        }
    }
}
