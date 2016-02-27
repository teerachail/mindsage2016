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
    public class Remove_A_CommentSteps
    {
        [When(@"UserProfileId '(.*)' remove the comment '(.*)' in the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileIdRemoveTheCommentInTheLessonOfClassRoom(string userprofileName, string commentId, string lessonId, string classRoomId)
        {
            var mockCommentRepo = ScenarioContext.Current.Get<Mock<ICommentRepository>>();
            mockCommentRepo.Setup(it => it.UpsertComment(It.IsAny<Comment>()));

            var commentCtrl = ScenarioContext.Current.Get<CommentController>();
            var body = new UpdateCommentRequest
            {
                ClassRoomId = classRoomId,
                LessonId = lessonId,
                UserProfileId = userprofileName
            };
            commentCtrl.Put(commentId, body);
        }

        [Then(@"System Update Comment by JSON format is")]
        public void ThenSystemUpdateCommentByJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<Comment>(multilineText);
            var mockCommentRepo = ScenarioContext.Current.Get<Mock<ICommentRepository>>();
            mockCommentRepo.Verify(it => it.UpsertComment(It.Is<Comment>(comment =>
                comment.id == expected.id
                && comment.ClassRoomId == expected.ClassRoomId
                && comment.CreatedByUserProfileId == expected.CreatedByUserProfileId
                && comment.Description == expected.Description
                && comment.TotalLikes == expected.TotalLikes
                && comment.LessonId == expected.LessonId
                && comment.DeletedDate == expected.DeletedDate
                && comment.CreatorDisplayName == expected.CreatorDisplayName 
                && comment.CreatorImageUrl == expected.CreatorImageUrl
            )));
        }

        [Then(@"System doesn't update UserActivity collection with JSON format is")]
        public void ThenSystemDoesnTUpdateUserActivityCollectionWithJSONFormatIs()
        {
            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Verify(it => it.UpsertUserActivity(It.IsAny<UserActivity>()), Times.Never);
        }
    }
}
