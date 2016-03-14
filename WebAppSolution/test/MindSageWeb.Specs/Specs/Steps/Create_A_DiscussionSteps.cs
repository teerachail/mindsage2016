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
    public class Create_A_DiscussionSteps
    {
        [When(@"UserProfileId '(.*)' create a new discussion with a message is '(.*)' for comment '(.*)' in the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileIdCreateANewDiscussionWithAMessageIsForCommentInTheLessonOfClassRoom(string userprofileId, string message, string commentId, string lessonId, string classRoomId)
        {
            var mockCommentRepo = ScenarioContext.Current.Get<Mock<ICommentRepository>>();
            mockCommentRepo.Setup(it => it.UpsertComment(It.IsAny<Comment>()));

            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

            var discussionCtrl = ScenarioContext.Current.Get<DiscussionController>();
            var body = new PostNewDiscussionRequest
            {
                ClassRoomId = classRoomId,
                CommentId = commentId,
                Description = message,
                LessonId = lessonId,
                UserProfileId = userprofileId
            };
            discussionCtrl.Post(body);
        }

        [Then(@"System update Discussion collection with JSON format in the Comment '(.*)' are")]
        public void ThenSystemUpdateDiscussionCollectionWithJSONFormatInTheCommentAre(string commentId, string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<IEnumerable<Comment.Discussion>>(multilineText).ToList();
            Func<Comment, bool> validateCommentFunc = (comment) =>
            {
                var isCommentVerify = comment != null
                    && comment.id == comment.id
                    && comment.Discussions.Count() == expected.Count();
                if (!isCommentVerify) return false;

                var actualDiscussion = comment.Discussions.ToList();
                for (int elementIndex = 0; elementIndex < expected.Count; elementIndex++)
                {
                    var areArgumentsVerify = !string.IsNullOrEmpty(actualDiscussion[elementIndex].id)
                        && expected[elementIndex].CreatedByUserProfileId == actualDiscussion[elementIndex].CreatedByUserProfileId
                        && expected[elementIndex].CreatorDisplayName == actualDiscussion[elementIndex].CreatorDisplayName
                        && expected[elementIndex].CreatorImageUrl == actualDiscussion[elementIndex].CreatorImageUrl
                        && expected[elementIndex].Description == actualDiscussion[elementIndex].Description
                        && expected[elementIndex].TotalLikes == actualDiscussion[elementIndex].TotalLikes
                        && expected[elementIndex].CreatedDate == actualDiscussion[elementIndex].CreatedDate
                        && expected[elementIndex].DeletedDate == actualDiscussion[elementIndex].DeletedDate;
                    if (!areArgumentsVerify) return false;
                }
                return true;
            };

            var mockCommentRepo = ScenarioContext.Current.Get<Mock<ICommentRepository>>();
            mockCommentRepo.Verify(it => it.UpsertComment(It.Is<Comment>(comment => validateCommentFunc(comment))));
        }
    }
}
