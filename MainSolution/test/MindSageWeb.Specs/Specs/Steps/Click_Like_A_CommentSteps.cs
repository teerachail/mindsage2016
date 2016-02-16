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
    public class Click_Like_A_CommentSteps
    {
        [When(@"UserProfileId '(.*)' click the like button for comment '(.*)' in the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileIdClickTheLikeButtonForCommentInTheLessonOfClassRoom(string userprofileId, string commentId, string lessonId, string classRoomId)
        {
            var mockUserActivityRepo = ScenarioContext.Current.Get<Moq.Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

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

        [Then(@"System update total likes for comment '(.*)' in the lesson '(.*)' of ClassRoom '(.*)' to '(.*)' likes")]
        public void ThenSystemUpdateTotalLikesForCommentInTheLessonOfClassRoomToLikes(string commentId, string lessonId, string classRoomId, int totalLikes)
        {
            var mockCommentRepo = ScenarioContext.Current.Get<Moq.Mock<ICommentRepository>>();
            mockCommentRepo.Verify(it => it.UpsertComment(It.Is<Comment>(comment =>
                comment.id == commentId
                && comment.LessonId == lessonId
                && comment.ClassRoomId == classRoomId
                && comment.TotalLikes == totalLikes
            )));
        }

        [Then(@"System add new LikeComment by JSON format is")]
        public void ThenSystemAddNewLikeCommentByJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<LikeComment>(multilineText);
            var mockLikeCommentRepo = ScenarioContext.Current.Get<Moq.Mock<ILikeCommentRepository>>();
            mockLikeCommentRepo.Verify(it => it.UpsertLikeComment(It.Is<LikeComment>(likeComment =>
                !string.IsNullOrEmpty(likeComment.id)
                && likeComment.LessonId == expected.LessonId
                && likeComment.CommentId == expected.CommentId
                && likeComment.LikedByUserProfileId == expected.LikedByUserProfileId
                && likeComment.CreatedDate == expected.CreatedDate
                && likeComment.DeletedDate == expected.DeletedDate
            )));
        }
    }
}
