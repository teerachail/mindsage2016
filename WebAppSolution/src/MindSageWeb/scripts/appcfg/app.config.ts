module app {
    'use strict';

    export interface IAppConfig {
        LessonUrl: string;
        LessonCommentUrl: string;
        CourseMapContentUrl: string;
        LessonDiscussionUrl: string;
        StudentListUrl: string;
        SelfpurchaseListUrl: string;
        SearchFriendUrl: string;
        SendFriendRequestUrl: string;
        JournalCommentUrl: string;
        TeacherListUrl: string;
        TeacherRemoveStdUrl: string;
        GetDiscussionUrl: string;
        CreateCommentUrl: string;
        CreateDiscussionUrl: string;
        LikeCommentUrl: string;
        LikeDiscussionUrl: string;
        UpdateCommentUrl: string;
        UpdateDiscussionUrl: string;
        LikeLessonUrl: string;
        ReadNoteUrl: string;
        UpdateProfileUrl: string;
        ChangeCourseUrl: string;
        UpdateCourseUrl: string;
        DeleteCourseUrl: string;
        StudenMessageEditUrl: string;
        GetAllCourserofileUrl: string;
        GetNotificationNumberUrl: string;
        GetNotificationContentUrl: string;
        GetLiketUrl: string;
        GetAllLiketUrl: string;
        GetUserProfileUrl: string;
        CourseScheduleUrl: string;
        CourseStartDateUrl: string;
        CourseScheduleRangeUrl: string;
        CourseScheduleWeekUrl: string;
        ApplyToAllCourseUrl: string;
        LessonPreviewUrl: string;
        SendContactUrl: string;
    }

    export class AppConfig implements IAppConfig {

        public LessonUrl: string;
        public LessonCommentUrl: string;
        public CourseMapContentUrl: string;
        public LessonDiscussionUrl: string;
        public StudentListUrl: string;
        public SelfpurchaseListUrl: string;
        public SearchFriendUrl: string;
        public SendFriendRequestUrl: string;
        public JournalCommentUrl: string;
        public TeacherListUrl: string;
        public TeacherRemoveStdUrl: string;
        public GetDiscussionUrl: string;
        public CreateCommentUrl: string;
        public CreateDiscussionUrl: string;
        public LikeCommentUrl: string;
        public LikeDiscussionUrl: string;
        public UpdateCommentUrl: string;
        public UpdateDiscussionUrl: string;
        public LikeLessonUrl: string;
        public ReadNoteUrl: string;
        public UpdateProfileUrl: string;
        public ChangeCourseUrl: string;
        public UpdateCourseUrl: string;
        public DeleteCourseUrl: string;
        public StudenMessageEditUrl: string;
        public GetAllCourserofileUrl: string;
        public GetNotificationNumberUrl: string;
        public GetNotificationContentUrl: string;
        public GetLiketUrl: string;
        public GetAllLiketUrl: string;
        public GetUserProfileUrl: string;
        public CourseScheduleUrl: string;
        public CourseStartDateUrl: string;
        public CourseScheduleRangeUrl: string;
        public CourseScheduleWeekUrl: string;
        public ApplyToAllCourseUrl: string;
        public LessonPreviewUrl: string;
        public SendContactUrl: string;

        static $inject = ['defaultUrl'];
        constructor(defaultUrl: string) {
            var apiUrl = defaultUrl + '/api';

            this.LessonUrl = apiUrl + '/lesson/:id/:classRoomId/:userId';
            this.LessonCommentUrl = apiUrl + '/lesson/:id/:classRoomId/comments/:userId';
            this.CourseMapContentUrl = apiUrl + '/mycourse/:id/:classRoomId/:classCalendarId/:action';
            this.LessonDiscussionUrl = apiUrl + '/lesson/:id/:classRoomId/discussions/:userId';
            this.StudentListUrl = apiUrl + '/friend/:userId/:classRoomId';
            this.SelfpurchaseListUrl = this.StudentListUrl + '/selfpurchase';
            this.SearchFriendUrl = this.StudentListUrl + '/:key';
            this.SendFriendRequestUrl = apiUrl + '/friend';
            this.JournalCommentUrl = apiUrl + '/journal/:targetUserId/:requestByUserId/:classRoomId';
            this.TeacherListUrl = apiUrl + '/mycourse/:userId/:classRoomId/students';
            this.TeacherRemoveStdUrl = apiUrl + '/mycourse/removestud';
            this.GetDiscussionUrl = apiUrl + '/lesson/:id/:classRoomId/:commentId/discussions/:userId';
            this.CreateCommentUrl = apiUrl + '/comment';
            this.CreateDiscussionUrl = apiUrl + '/discussion';
            this.LikeCommentUrl = apiUrl + '/comment/like';
            this.LikeDiscussionUrl = apiUrl + '/discussion/like';
            this.UpdateCommentUrl = apiUrl + '/comment/:id';
            this.UpdateDiscussionUrl = apiUrl + '/discussion/:id';
            this.LikeLessonUrl = apiUrl + '/lesson/like';
            this.ReadNoteUrl = apiUrl + '/lesson/readnote';
            this.UpdateProfileUrl = apiUrl + '/profile/:id';
            this.ChangeCourseUrl = apiUrl + '/mycourse/changecourse';
            this.UpdateCourseUrl = apiUrl + '/mycourse/:id';
            this.DeleteCourseUrl = apiUrl + '/mycourse/leave';
            this.StudenMessageEditUrl = apiUrl + '/mycourse/message';
            this.GetAllCourserofileUrl = apiUrl + '/mycourse/:id/courses';
            this.GetNotificationNumberUrl = apiUrl + '/notification/:id/:classRoomId';
            this.GetNotificationContentUrl = apiUrl + '/notification/:id/:classRoomId/content';
            this.GetLiketUrl = apiUrl + '/mycourse/:id/:classRoomId/:lessonId';
            this.GetAllLiketUrl = apiUrl + '/mycourse/:id/:classRoomId/alllikes';
            this.GetUserProfileUrl = apiUrl + '/profile';
            this.CourseScheduleUrl = apiUrl + '/mycourse/:id/:classRoomId/schedule';
            this.CourseStartDateUrl = apiUrl + '/mycourse/startdate';
            this.CourseScheduleRangeUrl = apiUrl + '/mycourse/schedulerange';
            this.CourseScheduleWeekUrl = apiUrl + '/mycourse/scheduleweek';
            this.ApplyToAllCourseUrl = apiUrl + '/mycourse/applytoall';
            this.LessonPreviewUrl = apiUrl + '/lesson/:id/lessonpreview';
            this.SendContactUrl = '/Home/ContactUs';
        }
    }
    
    // HACK: Change the host Url
    angular
        .module('app')
        .constant('defaultUrl', 'http://localhost:2528')
        .service('appConfig', AppConfig);
}