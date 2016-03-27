module app.lessonpreviews {
    'use strict';
    
    interface IGetLessonContentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }

    export class LessonService {
                
        private getLessonSvc: IGetLessonContentResourceClass<any>;
        
        static $inject = ['appConfig', '$resource'];
        constructor(private appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.getLessonSvc = <IGetLessonContentResourceClass<any>>$resource(appConfig.LessonPreviewUrl, { 'id': '@id' });
        }

        public GetContent(lessonId: string): ng.IPromise<any> {
            return this.getLessonSvc.get(new LessonContentRequest(lessonId)).$promise;
        }
    }
    
    angular
        .module('app.lessonpreviews')
        .service('app.lessonpreviews.LessonService', LessonService);
}