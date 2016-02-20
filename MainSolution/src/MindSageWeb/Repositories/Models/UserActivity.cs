using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class UserActivity
    {
        #region Properties

        public string id { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsPrivateAccount { get; set; }
        public string UserProfileName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public DateTime? HideClassRoomMessageDate { get; set; }
        public string UserProfileId { get; set; }
        public string ClassRoomId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<LessonActivity> LessonActivities { get; set; }

        [BsonIgnore]
        public int CommentPercentage
        {
            get
            {

                const int NoneScore = 0;
                const int MaximumCommentRequired = 6;
                var totalCommentScore = (double)LessonActivities.Select(it =>
                {
                    if (it.CreatedCommentAmount > NoneScore)
                    {
                        if (it.CreatedCommentAmount >= MaximumCommentRequired) return MaximumCommentRequired;
                        else return it.CreatedCommentAmount;
                    }
                    else return NoneScore;
                }).Sum();

                const int ConvertToPercent = 100;
                return (int)((totalCommentScore / LessonActivities.Count()) * ConvertToPercent);
            }
        }
        [BsonIgnore]
        public int OnlineExtrasPercentage
        {
            get
            {
                var totalContents = LessonActivities.Sum(it => it.TotalContentsAmount);
                var sawContents = (double)LessonActivities.Sum(it => it.SawContentIds.Count());
                const int ConvertToPercent = 100;
                return (int)((sawContents / totalContents) * ConvertToPercent);
            }
        }
        [BsonIgnore]
        public int SocialParticipationPercentage
        {
            get
            {
                const int MinimumParticipationRequired = 1;
                var participationLessons = (double) LessonActivities.Where(it => it.ParticipationAmount >= MinimumParticipationRequired).Count();
                const int ConvertToPercent = 100;
                return (int)((participationLessons / LessonActivities.Count()) * ConvertToPercent);
            }
        }

        #endregion Properties

        public class LessonActivity
        {
            #region Properties

            public string id { get; set; }
            public DateTime BeginDate { get; set; }
            public int TotalContentsAmount { get; set; }
            public IEnumerable<string> SawContentIds { get; set; }
            public int CreatedCommentAmount { get; set; }
            public int ParticipationAmount { get; set; }
            public string LessonId { get; set; }

            #endregion Properties
        }
    }
}
