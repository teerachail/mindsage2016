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
    public class Click_Unlike_A_DiscussionSteps
    {
        [When(@"UserProfileId '(.*)' click the unlike button discussion '(.*)' for comment '(.*)' in the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileIdClickTheUnlikeButtonDiscussionForCommentInTheLessonOfClassRoom(string userprofileId, string discussionId, string commentId, string lessonId, string classRoomId)
        {
            var mockLikeDiscussionRepo = ScenarioContext.Current.Get<Moq.Mock<ILikeDiscussionRepository>>();
            mockLikeDiscussionRepo.Setup(it => it.UpsertLikeDiscussion(It.IsAny<LikeDiscussion>()));

            var mockCommentRepo = ScenarioContext.Current.Get<Moq.Mock<ICommentRepository>>();
            mockCommentRepo.Setup(it => it.UpsertComment(It.IsAny<Comment>()));

            var discussionCtrl = ScenarioContext.Current.Get<DiscussionController>();
            var body = new LikeDiscussionRequest
            {
                ClassRoomId = classRoomId,
                LessonId = lessonId,
                CommentId = commentId,
                DiscussionId = discussionId,
                UserProfileId = userprofileId
            };
            discussionCtrl.Like(body);
        }
    }
}
