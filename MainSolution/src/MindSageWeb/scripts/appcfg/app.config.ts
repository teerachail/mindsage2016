module app {
    'use strict';
    
    export interface IAppConfig {
        LessonUrl: string;
        LessonCommentUrl: string;
        LessonDiscussionUrl: string;
        StudentListUrl: string;
    }
    
    export class AppConfig implements IAppConfig {
        
        public LessonUrl: string;
        public LessonCommentUrl: string;
        public LessonDiscussionUrl: string;
        public StudentListUrl: string;
        
        static $inject = ['defaultUrl'];
        constructor(defaultUrl: string) {
            var apiUrl = defaultUrl + '/api';
            
            this.LessonUrl = apiUrl + '/lesson/:id/:classRoomId/:userId';
            this.LessonCommentUrl = apiUrl + '/lesson/:id/:classRoomId/comments/:userId';
            this.LessonDiscussionUrl = apiUrl + '/lesson/:id/:classRoomId/discussions/:userId';
            this.StudentListUrl = apiUrl + '/friend/:userId/:classRoomId';
        }
    }
    
    // HACK: Change the host Url
    angular
        .module('app')
        .constant('defaultUrl', 'http://localhost:4147') 
        .service('appConfig', AppConfig);
}