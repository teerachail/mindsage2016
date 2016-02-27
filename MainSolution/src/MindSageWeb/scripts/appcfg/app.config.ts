module app {
    'use strict';

    export interface IAppConfig {
        LessonUrl: string;
        LessonCommentUrl: string;
        CourseMapContentUrl: string;
        LessonDiscussionUrl: string;
        StudentListUrl: string;
        JournalCommentUrl: string;
        TeacherListUrl: string;
        GetDiscussionUrl: string;
    }

    export class AppConfig implements IAppConfig {

        public LessonUrl: string;
        public LessonCommentUrl: string;
        public CourseMapContentUrl: string;
        public LessonDiscussionUrl: string;
        public StudentListUrl: string;
        public JournalCommentUrl: string;
        public TeacherListUrl: string;
        public GetDiscussionUrl: string;

        static $inject = ['defaultUrl'];
        constructor(defaultUrl: string) {
            var apiUrl = defaultUrl + '/api';

            this.LessonUrl = apiUrl + '/lesson/:id/:classRoomId/:userId';
            this.LessonCommentUrl = apiUrl + '/lesson/:id/:classRoomId/comments/:userId';
            this.CourseMapContentUrl = apiUrl + '/mycourse/:id/:classRoomId/:action';
            this.LessonDiscussionUrl = apiUrl + '/lesson/:id/:classRoomId/discussions/:userId';
            this.StudentListUrl = apiUrl + '/friend/:userId/:classRoomId';
            this.JournalCommentUrl = apiUrl + '/journal/:targetUserId/:requestByUserId/:classRoomId';
            this.TeacherListUrl = apiUrl + '/mycourse/:userId/:classRoomId/students';
            this.GetDiscussionUrl = apiUrl + '/lesson/:id/:classRoomId/:commentId/discussions/:userId';
        }
    }
    
    // HACK: Change the host Url
    angular
        .module('app')
        .constant('defaultUrl', 'http://localhost:4147')
        .service('appConfig', AppConfig);
}