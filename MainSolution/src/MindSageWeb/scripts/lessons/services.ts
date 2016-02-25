module app.lessons {
    'use strict';
    
    interface ILessonContentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
        //GetComments(data: T): T;
        //GetDiscussion(data: T): T;
    }

    interface ILessonCommentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetComments(data: T): T;
    }

    export class LessonService {
                
        private svc: ILessonContentResourceClass<any>;
        
        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.svc = <ILessonContentResourceClass<any>>$resource(appConfig.LessonUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
        }

        public GetContent(id: string, classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.svc.get(new LessonContentRequest(id, classRoomId, userId)).$promise;
        }
        
    }

    export class LessonCommentService {

        private svc: ILessonCommentResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.svc = <ILessonCommentResourceClass<any>>$resource(appConfig.LessonCommentUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
        }

        public GetComments(id: string, classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.svc.get(new LessonCommentRequest(id, classRoomId, userId)).$promise;
        }

    }
    
    angular
        .module('app.lessons')
        .service('app.lessons.LessonService', LessonService)
        .service('app.lessons.LessonCommentService', LessonCommentService);
}