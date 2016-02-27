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
    public class Remove_A_DiscussionSteps
    {
        [When(@"UserProfileId '(.*)' remove the discussion '(.*)' from comment '(.*)' in the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileIdRemoveTheDiscussionFromCommentInTheLessonOfClassRoom(string userprofileId, string discussionId, string commentId, string lessonId, string classRoomId)
        {
            var mockCommentRepo = ScenarioContext.Current.Get<Mock<ICommentRepository>>();
            mockCommentRepo.Setup(it => it.UpsertComment(It.IsAny<Comment>()));

            var discussionCtrl = ScenarioContext.Current.Get<DiscussionController>();
            var body = new UpdateDiscussionRequest
            {
                ClassRoomId = classRoomId,
                LessonId = lessonId,
                CommentId = commentId,
                UserProfileId = userprofileId
            };
            discussionCtrl.Put(discussionId, body);
        }
    }
}
