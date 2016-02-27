module app.shared {
    'use strict';

    export class ClientUserProfileService {
        private clientUserProfile: ClientUserProfile;

        constructor() {
            // HACK: User profile information
            this.clientUserProfile = new ClientUserProfile();
            this.clientUserProfile.UserProfileId = 'sakul@mindsage.com';
            this.clientUserProfile.ImageUrl = 'http://placehold.it/100x100';
            this.clientUserProfile.FullName = 'Sakul Jaruthanaset';
            this.clientUserProfile.CurrentClassRoomId = "ClassRoom01";
            this.clientUserProfile.CurrentLessonId = "Lesson01";
        }

        public GetClientUserProfile(): ClientUserProfile {
            return this.clientUserProfile;
        }
    }

    interface ICommentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetComments(data: T): T;
    }
    export class CommentService {

        private svc: ICommentResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.svc = <ICommentResourceClass<any>>$resource(appConfig.LessonCommentUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
        }

        public GetComments(lessonId: string, classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.svc.get(new GetCommentsRequest(lessonId, classRoomId, userId)).$promise;
        }

    }

    interface IDiscussionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetDiscussions(data: T): T;
    }
    export class DiscussionService {

        private svc: IDiscussionResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.svc = <IDiscussionResourceClass<any>>$resource(appConfig.GetDiscussionUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'commentId': '@commentId', 'userId': '@userId' });
        }

        public GetDiscussions(lessonId: string, classRoomId: string, commentId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.svc.get(new GetDiscussionRequest(lessonId, classRoomId, commentId, userId)).$promise;
        }

    }

    angular
        .module('app.shared')
        .service('app.shared.ClientUserProfileService', ClientUserProfileService)
        .service('app.shared.CommentService', CommentService)
        .service('app.shared.DiscussionService', DiscussionService);
}