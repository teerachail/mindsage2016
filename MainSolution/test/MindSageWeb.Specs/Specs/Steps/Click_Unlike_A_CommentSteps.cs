using MindsageWeb.Controllers;
using MindsageWeb.Repositories;
using MindsageWeb.Repositories.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace MindSageWeb.Specs.Steps
{
    [Binding]
    public class Click_Unlike_A_CommentSteps
    {
        [When(@"UserProfileId '(.*)' click the unlike button for comment '(.*)' in the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileIdClickTheUnlikeButtonForCommentInTheLessonOfClassRoom(string userprofileId, string commentId, string lessonId, string classRoomId)
        {
            var mockLikeCommentRepo = ScenarioContext.Current.Get<Moq.Mock<ILikeCommentRepository>>();
            mockLikeCommentRepo.Setup(it => it.UpsertLikeComment(It.IsAny<LikeComment>()));

            var mockCommentRepo = ScenarioContext.Current.Get<Moq.Mock<ICommentRepository>>();
            mockCommentRepo.Setup(it => it.UpsertComment(It.IsAny<Comment>()));

            var commentCtrl = ScenarioContext.Current.Get<CommentController>();
            var body = new LikeCommentRequest
            {
                ClassRoomId = classRoomId,
                LessonId = lessonId,
                CommentId = commentId,
                UserProfileId = userprofileId
            };
            commentCtrl.Like(body);
        }
    }
}
