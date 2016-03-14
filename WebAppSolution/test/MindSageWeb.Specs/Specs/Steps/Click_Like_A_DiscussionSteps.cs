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
    public class Click_Like_A_DiscussionSteps
    {
        [When(@"UserProfileId '(.*)' click the like button discussion '(.*)' for comment '(.*)' in the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileIdClickTheLikeButtonDiscussionForCommentInTheLessonOfClassRoom(string userprofileId, string discussionId, string commentId, string lessonId, string classRoomId)
        {
            var mockUserActivityRepo = ScenarioContext.Current.Get<Moq.Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

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

        [Then(@"System update total likes discussion '(.*)' for comment '(.*)' in the lesson '(.*)' of ClassRoom '(.*)' to '(.*)' likes")]
        public void ThenSystemUpdateTotalLikesDiscussionForCommentInTheLessonOfClassRoomToLikes(string discussionId, string commentId, string lessonId, string classRoomId, int totalLikes)
        {
            Func<IEnumerable<Comment.Discussion>, bool> validateDiscussionFunc = discussions =>
            {
                var selectedDiscussion = discussions.FirstOrDefault(it => it.id == discussionId);
                return selectedDiscussion != null && selectedDiscussion.TotalLikes == totalLikes;
            };
            var mockCommentRepo = ScenarioContext.Current.Get<Moq.Mock<ICommentRepository>>();
            mockCommentRepo.Verify(it => it.UpsertComment(It.Is<Comment>(comment =>
                comment.id == commentId
                && comment.LessonId == lessonId
                && comment.ClassRoomId == classRoomId
                && validateDiscussionFunc(comment.Discussions)
            )));
        }

        [Then(@"System upsert LikeDiscussion by JSON format is")]
        public void ThenSystemUpsertLikeDiscussionByJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<LikeDiscussion>(multilineText);
            var mockLikeDiscussionRepo = ScenarioContext.Current.Get<Moq.Mock<ILikeDiscussionRepository>>();
            mockLikeDiscussionRepo.Verify(it => it.UpsertLikeDiscussion(It.Is<LikeDiscussion>(likeDiscussion =>
                !string.IsNullOrEmpty(likeDiscussion.id)
                && likeDiscussion.CommentId == expected.CommentId
                && likeDiscussion.CreatedDate == expected.CreatedDate
                && likeDiscussion.DeletedDate == expected.DeletedDate
                && likeDiscussion.DiscussionId == expected.DiscussionId
                && likeDiscussion.LessonId == expected.LessonId
                && likeDiscussion.LikedByUserProfileId == expected.LikedByUserProfileId
            )));
        }
    }
}
