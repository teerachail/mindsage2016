using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Linq;

namespace MindSageWeb.Controllers
{
    public static class APIUtil
    {
        #region Methods

        public static bool CheckAccessPermissionToSelectedClassRoom(this IUserProfileRepository userprofileRepo, string userprofileId, string classRoomId)
        {
            UserProfile userprofile;
            return CheckAccessPermissionToSelectedClassRoom(userprofileRepo, userprofileId, classRoomId, out userprofile);
        }
        public static bool CheckAccessPermissionToSelectedClassRoom(this IUserProfileRepository userprofileRepo, string userprofileId, string classRoomId, out UserProfile userprofile)
        {
            userprofile = null;
            var areArgumentsValid = !string.IsNullOrEmpty(userprofileId) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return false;

            var selectedUserProfile = userprofileRepo.GetUserProfileById(userprofileId);
            if (selectedUserProfile == null) return false;
            userprofile = selectedUserProfile;

            var canAccessToTheClass = selectedUserProfile
                .Subscriptions
                .Where(it => it.ClassRoomId.Equals(classRoomId, StringComparison.CurrentCultureIgnoreCase))
                .Any();

            return canAccessToTheClass;
        }
        public static bool CheckAccessPermissionToSelectedClassLesson(this IClassCalendarRepository classCalendarRepo, string classRoomId, string lessonId, DateTime currentTime)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(classRoomId) && !string.IsNullOrEmpty(lessonId);
            if (!areArgumentsValid) return false;

            var selectedClassCalendar = classCalendarRepo.GetClassCalendarByClassRoomId(classRoomId);
            if (selectedClassCalendar == null) return false;

            var canAccessToTheLesson = selectedClassCalendar.LessonCalendars
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.LessonId.Equals(lessonId, StringComparison.CurrentCultureIgnoreCase))
                .Where(it => it.BeginDate <= currentTime)
                .Any();
            return canAccessToTheLesson;
        }
        public static bool CheckAccessPermissionToUserProfile(this IUserProfileRepository userprofileRepo, string userprofileId)
        {
            var selectedCommentOwnerProfile = userprofileRepo.GetUserProfileById(userprofileId);
            var canPostNewDiscussion = selectedCommentOwnerProfile != null
                && !selectedCommentOwnerProfile.IsPrivateAccount;
            return canPostNewDiscussion;
        }

        #endregion Methods
    }
}
