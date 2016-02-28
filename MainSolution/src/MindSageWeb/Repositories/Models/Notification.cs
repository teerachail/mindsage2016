using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class Notification
    {
        #region Properties

        public string id { get; set; }
        public DateTime? LastReadedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string ToUserProfileId { get; set; }
        public string ClassRoomId { get; set; }
        public string LessonId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? HideDate { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> ByUserProfileId { get; set; }
        public int TotalLikes { get; set; }
        public NotificationTag Tag { get; set; }

        #endregion Properties

        public enum NotificationTag
        {
            Reminder,
            TopicOfTheDay,
            FriendCreateNewComment,
            SomeOneLikesYourComment,
            SomeOneLikesYourDiscussion,
            SomeOneCreateDiscussionInYourComment,
        }
    }
}
