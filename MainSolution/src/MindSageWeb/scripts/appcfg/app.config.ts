module app {
    'use strict';

    export interface IAppConfig {
        LessonUrl: string;
        LessonCommentUrl: string;
        CourseMapContentUrl: string;
        LessonDiscussionUrl: string;
        StudentListUrl: string;
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
        GetUserProfileUrl: string;
        GetCourserofileUrl: string;
        UpdateCourseUrl: string;
        DeleteCourseUrl: string;
        StudenMessageEditUrl: string;
        GetAllCourserofileUrl: string;
        GetNotificationNumberUrl: string;
        GetNotificationContentUrl: string;
        GetLiketUrl: string;
        GetAllLiketUrl: string;
        GetUserIdUrl: string;
    }

    export class AppConfig implements IAppConfig {

        public LessonUrl: string;
        public LessonCommentUrl: string;
        public CourseMapContentUrl: string;
        public LessonDiscussionUrl: string;
        public StudentListUrl: string;
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
        public GetUserProfileUrl: string;
        public GetCourserofileUrl: string;
        public UpdateCourseUrl: string;
        public DeleteCourseUrl: string;
        public StudenMessageEditUrl: string;
        public GetAllCourserofileUrl: string;
        public GetNotificationNumberUrl: string;
        public GetNotificationContentUrl: string;
        public GetLiketUrl: string;
        public GetAllLiketUrl: string;
        public GetUserIdUrl: string;

        static $inject = ['defaultUrl'];
        constructor(defaultUrl: string) {
            var apiUrl = defaultUrl + '/api';

            this.LessonUrl = apiUrl + '/lesson/:id/:classRoomId/:userId';
            this.LessonCommentUrl = apiUrl + '/lesson/:id/:classRoomId/comments/:userId';
            this.CourseMapContentUrl = apiUrl + '/mycourse/:id/:classRoomId/:action';
            this.LessonDiscussionUrl = apiUrl + '/lesson/:id/:classRoomId/discussions/:userId';
            this.StudentListUrl = apiUrl + '/friend/:userId/:classRoomId';
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
            this.GetUserProfileUrl = apiUrl + '/profile/:id';
            this.GetCourserofileUrl = apiUrl + '/mycourse/:id/:classRoomId/info';
            this.UpdateCourseUrl = apiUrl + '/mycourse/:id';
            this.DeleteCourseUrl = apiUrl + '/mycourse/leave';
            this.StudenMessageEditUrl = apiUrl + '/mycourse/message';
            this.GetAllCourserofileUrl = apiUrl + '/mycourse/:id/courses';
            this.GetNotificationNumberUrl = apiUrl + '/notification/:id/:classRoomId';
            this.GetNotificationContentUrl = apiUrl + '/notification/:id/:classRoomId/content';
            this.GetLiketUrl = apiUrl + '/mycourse/:id/:classRoomId/:lessonId';
            this.GetAllLiketUrl = apiUrl + '/mycourse/:id/:classRoomId/alllikes';
            this.GetUserIdUrl = apiUrl + '/profile';
        }
    }
    
    // HACK: Change the host Url
    angular
        .module('app')
        .constant('defaultUrl', 'http://localhost:4147')
        .service('appConfig', AppConfig);
}