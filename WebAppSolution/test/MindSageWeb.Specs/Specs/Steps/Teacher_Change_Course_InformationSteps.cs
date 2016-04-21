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
    public class Teacher_Change_Course_InformationSteps
    {
        [When(@"UserProfile '(.*)' change course info from ClassRoom '(.*)' to ClassName '(.*)' and StudentCode '(.*)'")]
        public void WhenUserProfileChangeCourseInfoFromClassRoomToClassNameAndStudentCode(string userprofileId, string classRoomId, string newClassName, string newStudentCode)
        {
            var mockStudentKeyRepo = ScenarioContext.Current.Get<Mock<IStudentKeyRepository>>();
            mockStudentKeyRepo.Setup(it => it.UpsertStudentKey(It.IsAny<StudentKey>()));
            mockStudentKeyRepo.Setup(it => it.CreateNewStudentKey(It.IsAny<StudentKey>()))
                .Returns(() => System.Threading.Tasks.Task.Delay(100));

            var mockClassRoomRepo = ScenarioContext.Current.Get<Mock<IClassRoomRepository>>();
            mockClassRoomRepo.Setup(it => it.UpsertClassRoom(It.IsAny<ClassRoom>()));

            userprofileId = userprofileId.GetMockStrinValue();
            classRoomId = classRoomId.GetMockStrinValue();
            newClassName = newClassName.GetMockStrinValue();
            newStudentCode = newStudentCode.GetMockStrinValue();

            var myCourseCtrl = ScenarioContext.Current.Get<MyCourseController>();
            myCourseCtrl.Put(userprofileId, new UpdateCourseInfoRequest
            {
                ClassRoomId = classRoomId,
                ChangedStudentCode = newStudentCode,
                ClassName = newClassName,
            });
        }

        [Then(@"System don't upsert ClassRoom")]
        public void ThenSystemDonTUpsertClassRoom()
        {
            var mockClassRoomRepo = ScenarioContext.Current.Get<Mock<IClassRoomRepository>>();
            mockClassRoomRepo.Verify(it => it.UpsertClassRoom(It.IsAny<ClassRoom>()), Times.Never);
        }

        [Then(@"System don't upsert StudentKey")]
        public void ThenSystemDonTUpsertStudentKey()
        {
            var mockStudentKeyRepo = ScenarioContext.Current.Get<Mock<IStudentKeyRepository>>();
            mockStudentKeyRepo.Verify(it => it.UpsertStudentKey(It.IsAny<StudentKey>()), Times.Never);
        }

        [Then(@"System don't create new StudentKey")]
        public void ThenSystemDonTCreateNewStudentKey()
        {
            var mockStudentKeyRepo = ScenarioContext.Current.Get<Mock<IStudentKeyRepository>>();
            mockStudentKeyRepo.Verify(it => it.CreateNewStudentKey(It.IsAny<StudentKey>()), Times.Never);
        }
    }
}
