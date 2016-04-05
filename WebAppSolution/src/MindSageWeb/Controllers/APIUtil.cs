using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
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


        public static IEnumerable<string> GetAllUserCoursCatalogIds(this MyCourseController ctrl, string userprofileId)
        {
            var qry = ctrl.GetAllCourses(userprofileId).Select(it => it.CourseCatalogId);
            return qry;
        }
        public static bool CanAddNewCourseCatalog(this MyCourseController ctrl, string userprofileId, string courseCatalogId, out IEnumerable<string> allUserCourseCatalogIds)
        {
            allUserCourseCatalogIds = ctrl.GetAllUserCoursCatalogIds(userprofileId);
            var result = !allUserCourseCatalogIds.Contains(courseCatalogId);
            return result;
        }
        public static bool CanAddNewCourseCatalog(this MyCourseController ctrl, string userprofileId, string courseCatalogId)
        {
            var allUserCourseCatalogIds = Enumerable.Empty<string>();
            return ctrl.CanAddNewCourseCatalog(userprofileId, courseCatalogId, out allUserCourseCatalogIds);
        }

        public static void CalculateCourseSchedule(this ClassCalendar classCalendar)
        {
            var areArgumentsValid = classCalendar != null
                && classCalendar.BeginDate.HasValue
                && classCalendar.LessonCalendars != null
                && classCalendar.LessonCalendars.Any();
            if (!areArgumentsValid) return;

            const int ShiftOneDay = 1;
            const int LessonDuration = 5;
            var currentBeginDate = classCalendar.BeginDate.Value;

            // HACK: ต้องยกเลิกการคลี่ lessonRange มาใช้ช่วงเวลาแทน
            var shiftDays = (classCalendar.ShiftDays ?? Enumerable.Empty<DateTime>())
                .Select(it => new DateTime(it.Year, it.Month, it.Day).ToUniversalTime());
            
            var lessonQry = classCalendar.LessonCalendars.Where(it => !it.DeletedDate.HasValue).OrderBy(it => it.Order);
            foreach (var lesson in lessonQry)
            {
                // Set lesson's begin date
                while (true)
                {
                    var beginDate = currentBeginDate;
                    var endDate = currentBeginDate.AddDays(LessonDuration);
                    var lessonRange = Enumerable.Range(0, LessonDuration).Select(it => beginDate.AddDays(it).ToUniversalTime()); // HACK: ต้องยกเลิกการคลี่ lessonRange มาใช้ช่วงเวลาแทน
                    var availableRange = lessonRange.Except(shiftDays);
                    if (availableRange.Any())
                    {
                        lesson.BeginDate = availableRange.First();
                        var totalShiftDayInLessonRange = lessonRange.Intersect(shiftDays).Count();
                        var nextBeginDate = endDate.AddDays(totalShiftDayInLessonRange);
                        currentBeginDate = nextBeginDate;
                        break;
                    }
                    else
                    {
                        const int ShiftOneDayForNextLesson = 1;
                        var nextBeginDate = endDate.AddDays(ShiftOneDayForNextLesson);
                        currentBeginDate = nextBeginDate;
                    }
                }

                // Set topic of the day date
                foreach (var totd in lesson.TopicOfTheDays)
                {
                    var sendDay = totd.SendOnDay - ShiftOneDay;
                    totd.RequiredSendTopicOfTheDayDate = lesson.BeginDate.AddDays(sendDay);
                }
            }
        }

        public static UserActivity CreateNewUserActivity(this UserProfile selectedUserProfile, ClassRoom selectedClassRoom, ClassCalendar selectedClassCalendar, List<LessonCatalog> lessonCatalogs, DateTime now)
        {
            const int PrimaryContent = 1;
            var lessonActivities = selectedClassRoom.Lessons.Select(lesson =>
            {
                var selectedLessonCalendar = selectedClassCalendar.LessonCalendars
                    .Where(it => !it.DeletedDate.HasValue)
                    .FirstOrDefault(lc => lc.LessonId == lesson.id);

                var selectedLessonCatalog = lessonCatalogs
                    .FirstOrDefault(it => it.id == lesson.LessonCatalogId);

                return new UserActivity.LessonActivity
                {
                    id = Guid.NewGuid().ToString(),
                    BeginDate = selectedLessonCalendar.BeginDate,
                    TotalContentsAmount = selectedLessonCatalog.ExtraContents.Count() + PrimaryContent,
                    LessonId = lesson.id,
                    SawContentIds = Enumerable.Empty<string>()
                };
            }).ToList();

            var userActivity = new UserActivity
            {
                id = Guid.NewGuid().ToString(),
                UserProfileName = selectedUserProfile.Name,
                UserProfileImageUrl = selectedUserProfile.ImageProfileUrl,
                UserProfileId = selectedUserProfile.id,
                ClassRoomId = selectedClassRoom.id,
                CreatedDate = now,
                LessonActivities = lessonActivities
            };

            return userActivity;
        }

        #endregion Methods
    }
}
