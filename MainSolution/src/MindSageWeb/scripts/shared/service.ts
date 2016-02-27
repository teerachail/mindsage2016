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

    interface IGetCommentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetComments(data: T): T;
    }
    interface ICreateCommentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        CreateNewComment(data: T): T;
    }
    interface ILikeCommentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        LikeComment(data: T): T;
    }
    export class CommentService {

        private getCommentSvc: IGetCommentResourceClass<any>;
        private createCommentSvc: ICreateCommentResourceClass<any>;
        private likeCommentSvc: ILikeCommentResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.getCommentSvc = <IGetCommentResourceClass<any>>$resource(appConfig.LessonCommentUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
            this.createCommentSvc = <ICreateCommentResourceClass<any>>$resource(appConfig.CreateCommentUrl, {
                'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'UserProfileId': '@UserProfileId', 'Description': '@Description'
            });
            this.likeCommentSvc = <ILikeCommentResourceClass<any>>$resource(appConfig.LikeCommentUrl, {
                'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'UserProfileId': '@UserProfileId' });
        }

        public GetComments(lessonId: string, classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.getCommentSvc.get(new GetCommentsRequest(lessonId, classRoomId, userId)).$promise;
        }

        public CreateNewComment(classRoomId: string, lessonId: string, description: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.createCommentSvc.save(new CreateCommentRequest(classRoomId, lessonId, userId, description)).$promise;
        }

        public LikeComment(classRoomId: string, lessonId: string, commentId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.likeCommentSvc.save(new LikeCommentRequest(classRoomId, lessonId, commentId, userId)).$promise;
        }
    }

    interface IGetDiscussionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetDiscussions(data: T): T;
    }
    interface ICreateDiscussionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        CreateNewDiscussion(data: T): T;
    }
    export class DiscussionService {

        private getDiscussionSvc: IGetDiscussionResourceClass<any>;
        private createDiscussionSvc: IGetDiscussionResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.getDiscussionSvc = <IGetDiscussionResourceClass<any>>$resource(appConfig.GetDiscussionUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'commentId': '@commentId', 'userId': '@userId' });
            this.createDiscussionSvc = <IGetDiscussionResourceClass<any>>$resource(appConfig.CreateDiscussionUrl, {
                'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'UserProfileId': '@UserProfileId', 'Description': '@Description' });
        }

        public GetDiscussions(lessonId: string, classRoomId: string, commentId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.getDiscussionSvc.query(new GetDiscussionRequest(lessonId, classRoomId, commentId, userId)).$promise;
        }

        public CreateDiscussion(classRoomId: string, lessonId: string, commentId: string, message: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.createDiscussionSvc.save(new CreateDiscussionRequest(classRoomId, lessonId, commentId, userId, message)).$promise;
        }

    }

    angular
        .module('app.shared')
        .service('app.shared.ClientUserProfileService', ClientUserProfileService)
        .service('app.shared.CommentService', CommentService)
        .service('app.shared.DiscussionService', DiscussionService);
}