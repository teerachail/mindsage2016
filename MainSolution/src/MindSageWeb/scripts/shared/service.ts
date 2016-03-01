module app.shared {
    'use strict';

    export class ClientUserProfileService {

        public Advertisments: Advertisment[];
        public AllCourses: CourseCatalog[];
        private clientUserProfile: ClientUserProfile;

        constructor() {
            // HACK: User profile information
            this.clientUserProfile = new ClientUserProfile();
            this.clientUserProfile.UserProfileId = 'sakul@mindsage.com';
            //this.clientUserProfile.ImageUrl = 'http://placehold.it/100x100';
            //this.clientUserProfile.FullName = 'Sakul Jaruthanaset';
            //this.clientUserProfile.CurrentClassRoomId = "ClassRoom01";
            //this.clientUserProfile.CurrentLessonId = "Lesson01";
            //this.clientUserProfile.IsTeacher = true;
        }

        public UpdateUserProfile(userInfo: ClientUserProfile): void {
            this.clientUserProfile = userInfo;
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
    interface IUpdateCommentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        UpdateComment(data: T): T;
    }
    export class CommentService {

        private getCommentSvc: IGetCommentResourceClass<any>;
        private createCommentSvc: ICreateCommentResourceClass<any>;
        private likeCommentSvc: ILikeCommentResourceClass<any>;
        private updateCommentSvc: IUpdateCommentResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.getCommentSvc = <IGetCommentResourceClass<any>>$resource(appConfig.LessonCommentUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
            this.createCommentSvc = <ICreateCommentResourceClass<any>>$resource(appConfig.CreateCommentUrl, {
                'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'UserProfileId': '@UserProfileId', 'Description': '@Description'
            });
            this.likeCommentSvc = <ILikeCommentResourceClass<any>>$resource(appConfig.LikeCommentUrl, {
                'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'UserProfileId': '@UserProfileId'
            });
            this.updateCommentSvc = <IUpdateCommentResourceClass<any>>$resource(appConfig.UpdateCommentUrl, {
                'id': '@id', 'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'UserProfileId': '@UserProfileId', 'IsDelete': '@IsDelete', 'Description': '@Description'
            }, { UpdateComment: { method: 'PUT' } });
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

        public UpdateComment(classRoomId: string, lessonId: string, commentId: string, isDelete: boolean, message: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.updateCommentSvc.UpdateComment(new UpdateCommentRequest(commentId, classRoomId, lessonId, userId, isDelete, message)).$promise;
        }
    }

    interface IGetDiscussionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetDiscussions(data: T): T;
    }
    interface ICreateDiscussionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        CreateNewDiscussion(data: T): T;
    }
    interface ILikeDiscussionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        LikeDiscussion(data: T): T;
    }
    interface IUpdateDiscussionResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        UpdateDiscussion(data: T): T;
    }
    export class DiscussionService {

        private getDiscussionSvc: IGetDiscussionResourceClass<any>;
        private createDiscussionSvc: IGetDiscussionResourceClass<any>;
        private likeDiscussionSvc: ILikeDiscussionResourceClass<any>;
        private updateDiscussionSvc: IUpdateDiscussionResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.getDiscussionSvc = <IGetDiscussionResourceClass<any>>$resource(appConfig.GetDiscussionUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'commentId': '@commentId', 'userId': '@userId' });
            this.createDiscussionSvc = <IGetDiscussionResourceClass<any>>$resource(appConfig.CreateDiscussionUrl, {
                'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'UserProfileId': '@UserProfileId', 'Description': '@Description'
            });
            this.likeDiscussionSvc = <ILikeDiscussionResourceClass<any>>$resource(appConfig.LikeDiscussionUrl, {
                'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'DiscussionId': '@DiscussionId', 'UserProfileId': '@UserProfileId', 'Description': '@Description'
            });
            this.updateDiscussionSvc = <IUpdateDiscussionResourceClass<any>>$resource(appConfig.UpdateDiscussionUrl, {
                'id': '@id', 'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'UserProfileId': '@UserProfileId', 'IsDelete': '@IsDelete', 'Description': '@Description'
            }, { UpdateDiscussion: { method: 'PUT' } });
        }

        public GetDiscussions(lessonId: string, classRoomId: string, commentId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.getDiscussionSvc.query(new GetDiscussionRequest(lessonId, classRoomId, commentId, userId)).$promise;
        }

        public CreateDiscussion(classRoomId: string, lessonId: string, commentId: string, message: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.createDiscussionSvc.save(new CreateDiscussionRequest(classRoomId, lessonId, commentId, userId, message)).$promise;
        }

        public LikeDiscussion(classRoomId: string, lessonId: string, commentId: string, discussionId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.likeDiscussionSvc.save(new LikeDiscussionRequest(classRoomId, lessonId, commentId, discussionId, userId)).$promise;
        }

        public UpdateDiscussion(classRoomId: string, lessonId: string, commentId: string, discussionId: string, isDelete: boolean, message: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.updateDiscussionSvc.UpdateDiscussion(new UpdateDiscussionRequest(discussionId, classRoomId, lessonId, commentId, userId, isDelete, message)).$promise;
        }
    }

    interface IGetProfileResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
       GetProfile(data: T): T;
    }
    interface IGetCourseResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetCourse(data: T): T;
    }
    interface IGetAllCourseResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetAllCourse(data: T): T;
    }
    interface IGetNotificationNumberClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetAllCourse(data: T): T;
    }
    interface IGetNotificationContentClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetCourseContent(data: T): T;
    }
    interface IGetLikeClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetLike(data: T): T;
    }
    interface IGetAllLikeClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetAllLike(data: T): T;
    }
    interface IGetUserIdResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetUserId(data: T): T;
    }

    export class GetProfileService {

        private getProfileSvc: IGetProfileResourceClass<any>;
        private getCourseSvc: IGetCourseResourceClass<any>;
        private getAllCourseSvc: IGetAllCourseResourceClass<any>;
        private getNotificationNumberSvc: IGetNotificationNumberClass<any>;
        private getNotificationContentSvc: IGetNotificationContentClass<any>;
        private getLikeSvc: IGetLikeClass<any>;
        private getAllLikeSvc: IGetAllLikeClass<any>;
        private getUserIdSvc: IGetUserIdResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.getProfileSvc = <IGetProfileResourceClass<any>>$resource(appConfig.GetUserProfileUrl, { 'id': '@id' });
            this.getCourseSvc = <IGetCourseResourceClass<any>>$resource(appConfig.GetCourserofileUrl, { 'id': '@id', 'classRoomId': '@classRoomId'});
            this.getAllCourseSvc = <IGetAllCourseResourceClass<any>>$resource(appConfig.GetAllCourserofileUrl, { 'id': '@id' });
            this.getNotificationNumberSvc = <IGetNotificationNumberClass<any>>$resource(appConfig.GetNotificationNumberUrl, { 'id': '@id', 'classRoomId': '@classRoomId'});
            this.getNotificationContentSvc = <IGetNotificationContentClass<any>>$resource(appConfig.GetNotificationContentUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
            this.getLikeSvc = <IGetLikeClass<any>>$resource(appConfig.GetLiketUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'lessonId': '@lessonId' });
            this.getAllLikeSvc = <IGetAllLikeClass<any>>$resource(appConfig.GetAllLiketUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
            this.getUserIdSvc = <IGetUserIdResourceClass<any>>$resource(appConfig.GetUserIdUrl, { });
        }

        public GetProfile(): ng.IPromise<any> {
            return this.GetUserId().then(it=> {
                var userProfile = this.userprofileSvc.GetClientUserProfile();
                userProfile.UserProfileId = it.UserProfileId;
                this.userprofileSvc.UpdateUserProfile(userProfile);
                return this.getProfileSvc.get(new GetUserProfileRequest(it.UserProfileId)).$promise;
            });
        }

        public GetCourse(): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            return this.getCourseSvc.get(new GetCourseRequest(userId,classroomId)).$promise;
        }
        public GetAllCourse(): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.getAllCourseSvc.query(new GetAllCourseRequest(userId)).$promise;
        }
        public GetNotificationNumber(): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            return this.getNotificationNumberSvc.get(new GetNotificationNumberRequest(userId, classroomId)).$promise;
        }
        public GetNotificationContent(): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            return this.getNotificationContentSvc.query(new GetNotificationContentRequest(userId, classroomId)).$promise;
        }
        public GetLike(): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            var lessonId = this.userprofileSvc.GetClientUserProfile().CurrentLessonId;
            return this.getLikeSvc.get(new GetLikeRequest(userId, classroomId, lessonId)).$promise;
        }
        public GetAllLike(): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            var lessonId = this.userprofileSvc.GetClientUserProfile().CurrentLessonId;
            return this.getAllLikeSvc.get(new GetAllLikeRequest(userId, classroomId)).$promise;
        }
        public GetUserId(): ng.IPromise<any> {
            return this.getUserIdSvc.get().$promise;
        }

    }
    angular
        .module('app.shared')
        .service('app.shared.ClientUserProfileService', ClientUserProfileService)
        .service('app.shared.CommentService', CommentService)
        .service('app.shared.DiscussionService', DiscussionService)
        .service('app.shared.GetProfileService', GetProfileService);
}