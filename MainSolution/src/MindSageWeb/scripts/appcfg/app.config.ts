module app {
    'use strict';
    
    export interface IAppConfig {
        LessonUrl: string;    
    }
    
    export class AppConfig implements IAppConfig {
        
        public LessonUrl: string;
        
        static $inject = ['defaultUrl'];
        constructor(defaultUrl: string) {
            var apiUrl = defaultUrl + '/api';
            
            this.LessonUrl = apiUrl + '/lesson/:id/:classRoomId/:userId';
        }
    }
    
    // HACK: Change the host Url
    angular
        .module('app')
        .constant('defaultUrl', 'http://localhost:4147') 
        .service('appConfig', AppConfig);
}