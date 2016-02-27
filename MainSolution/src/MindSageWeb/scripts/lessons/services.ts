module app.lessons {
    'use strict';
    
    interface IGetLessonContentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }
    interface ILikeLessonContentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        LikeLesson(data: T): T;
    }

    export class LessonService {
                
        private getLessonSvc: IGetLessonContentResourceClass<any>;
        private likeLessonSvc: ILikeLessonContentResourceClass<any>;
        
        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.getLessonSvc = <IGetLessonContentResourceClass<any>>$resource(appConfig.LessonUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
            this.likeLessonSvc = <ILikeLessonContentResourceClass<any>>$resource(appConfig.LikeLessonUrl, {
                'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'UserProfileId': '@UserProfileId' });
        }

        public GetContent(id: string, classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.getLessonSvc.get(new LessonContentRequest(id, classRoomId, userId)).$promise;
        }
        
        public LikeLesson(classRoomId: string, lessonId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.likeLessonSvc.save(new LikeLessonRequest(classRoomId, lessonId, userId)).$promise;
        }
    }
    
    angular
        .module('app.lessons')
        .service('app.lessons.LessonService', LessonService);
}