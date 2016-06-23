module app.shared {
    'use strict';

    interface IGetCourseInfoResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetCourse(data: T): T;
    }
    interface IStudentListResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetStudentList(data: T): T;
    }
    interface ISelfpurchaseListResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetSelfpurchaseList(data: T): T;
    }
    export class ClientUserProfileService {

        public UserInCourseList: any[];
        public PrimaryVideoUrl: string;
        public Advertisments: Advertisment[];
        public AllAvailableCourses: CourseCatalog[];
        public ClientUserProfile: ClientUserProfile;

        private isWaittingForAllCourses: boolean;
        private isWaittingForFriendList: boolean;
        private isPrepareUserProfileComplete: boolean;
        private isWaittingForPrepareUserProfile: boolean;
        private isWaittingForUserProfileRespond: boolean;
        private getCourseSvc: IGetCourseInfoResourceClass<any>;
        private getAllCourseSvc: IGetAllCourseResourceClass<any>;
        private getStudentListsvc: IStudentListResourceClass<any>;
        private getSelfpurchaseListsvc: ISelfpurchaseListResourceClass<any>;
        private getUserProfileSvc: IGetUserProfileResourceClass<any>;

        static $inject = ['appConfig', '$resource', '$q'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private $q: ng.IQService) {
            this.getUserProfileSvc = <IGetUserProfileResourceClass<any>>$resource(appConfig.GetUserProfileUrl, {});
            this.getAllCourseSvc = <IGetAllCourseResourceClass<any>>$resource(appConfig.GetAllCourserofileUrl, { 'id': '@id' });
            this.getStudentListsvc = <IStudentListResourceClass<any>>$resource(appConfig.StudentListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId' });
            this.getSelfpurchaseListsvc = <ISelfpurchaseListResourceClass<any>>$resource(appConfig.SelfpurchaseListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId' });
            this.getCourseSvc = <IGetCourseInfoResourceClass<any>>$resource(appConfig.ChangeCourseUrl, { 'UserProfileId': '@UserProfileId', 'ClassRoomId': '@ClassRoomId' });
        }

        private getCourseInformation(): ng.IPromise<any> {
            var prom = this.$q.defer();
            this.getCourseInfo(this.ClientUserProfile.CurrentClassRoomId).then(courseInfoRespond => {
                this.UpdateCourseInformation(courseInfoRespond);
                console.log('2.Get course information, done');
                this.$q.all([
                    this.getAllCourses(),
                    this.getUsersLists(),
                ]).then(data => {
                    console.log('3.Get user information and friend list, done');
                    this.AllAvailableCourses = <CourseCatalog[]>data[0];
                    this.UserInCourseList = <any[]>data[1];
                    this.isWaittingForPrepareUserProfile = false;
                    this.isPrepareUserProfileComplete = true;
                    prom.resolve(null);
                }, error => {
                    console.log('3.Error');
                    this.isWaittingForPrepareUserProfile = false;
                    prom.reject(null);
                });
            }, error => {
                console.log('2.Error');
                prom.reject(null);
            });
            return prom.promise;
        }
        public PrepareAllUserProfile(): ng.IPromise<any> {
            var prom = this.$q.defer();
            var shouldPrepareUserProfile = !this.isPrepareUserProfileComplete && !this.isWaittingForPrepareUserProfile;
            if (shouldPrepareUserProfile) {
                this.isWaittingForPrepareUserProfile = true;
                this.getUserProfileSvc.get().$promise.then(it => {
                    console.log('1.Get user profile, done');
                    this.ClientUserProfile = it;
                    this.getCourseInformation().then(
                        respond => { prom.resolve(null) },
                        error => { prom.resolve(null) });
                    //this.$q.all([
                    //    this.getAllCourses(),
                    //    this.getFriendLists(),
                    //    this.getCourseInfo(this.ClientUserProfile.CurrentClassRoomId)
                    //]).then(data => {
                    //    console.log('2.Get all user contents, done');
                    //    this.AllAvailableCourses = <CourseCatalog[]>data[0];
                    //    this.UserInCourseList = <any[]>data[1];
                    //    this.UpdateCourseInformation(data[2]);
                    //    this.isWaittingForPrepareUserProfile = false;
                    //    this.isPrepareUserProfileComplete = true;
                    //    prom.resolve(null);
                    //}, error => {
                    //    console.log('2.Error');
                    //    this.isWaittingForPrepareUserProfile = false;
                    //    prom.reject(null);
                    //});
                }, error => {
                    console.log('1.Error');
                    this.isWaittingForPrepareUserProfile = false;
                    prom.reject(null);
                });
            }
            else if (this.isPrepareUserProfileComplete) {
                prom.resolve(null);
            }

            return prom.promise;
        }
        private getAllCourses(): ng.IPromise<any> {
            var userId = this.ClientUserProfile.UserProfileId;
            return this.getAllCourseSvc.query(new GetAllCourseRequest(userId)).$promise;
        }

        private getUsersLists(): ng.IPromise<any> {
            if (this.ClientUserProfile.IsSelfPurchase)
                return this.getSelfpurchaseLists();
            else
                return this.getFriendLists();
        }

        private getFriendLists(): ng.IPromise<any> {
            var userId = this.ClientUserProfile.UserProfileId;
            var classRoomId = this.ClientUserProfile.CurrentClassRoomId;
            return this.getStudentListsvc.query(new GetFriendListRequest(userId, classRoomId)).$promise;
        }
        private getSelfpurchaseLists(): ng.IPromise<any> {
            var userId = this.ClientUserProfile.UserProfileId;
            var classRoomId = this.ClientUserProfile.CurrentClassRoomId;
            return this.getSelfpurchaseListsvc.query(new GetFriendListRequest(userId, classRoomId)).$promise;
        }

        private getCourseInfo(classRoomId: string): ng.IPromise<any> {
            var userId = this.ClientUserProfile.UserProfileId;
            return this.getCourseSvc.save(new ChangeCourseRequest(userId, classRoomId)).$promise;
        }
        public UpdateCourseInformation(courseInfo: any): void {
            if (courseInfo == null) return;
            this.ClientUserProfile.CurrentClassRoomId = courseInfo.ClassRoomId;
            this.ClientUserProfile.IsTeacher = courseInfo.IsTeacher;
            this.ClientUserProfile.ClassName = courseInfo.ClassName;
            this.ClientUserProfile.CurrentStudentCode = courseInfo.CurrentStudentCode;
            this.ClientUserProfile.NumberOfStudents = courseInfo.NumberOfStudents;
            this.ClientUserProfile.StartDate = courseInfo.StartDate;
            this.ClientUserProfile.IsSelfPurchase = courseInfo.IsSelfPurchase;
        }

        public ChangeCourse(classRoomId: string): ng.IPromise<any> {
            return this.getCourseInfo(classRoomId);
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
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.getCommentSvc.get(new GetCommentsRequest(lessonId, classRoomId, userId)).$promise;
        }

        public CreateNewComment(classRoomId: string, lessonId: string, description: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.createCommentSvc.save(new CreateCommentRequest(classRoomId, lessonId, userId, description)).$promise;
        }

        public LikeComment(classRoomId: string, lessonId: string, commentId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.likeCommentSvc.save(new LikeCommentRequest(classRoomId, lessonId, commentId, userId)).$promise;
        }

        public UpdateComment(classRoomId: string, lessonId: string, commentId: string, isDelete: boolean, message: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
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
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.getDiscussionSvc.query(new GetDiscussionRequest(lessonId, classRoomId, commentId, userId)).$promise;
        }

        public CreateDiscussion(classRoomId: string, lessonId: string, commentId: string, message: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.createDiscussionSvc.save(new CreateDiscussionRequest(classRoomId, lessonId, commentId, userId, message)).$promise;
        }

        public LikeDiscussion(classRoomId: string, lessonId: string, commentId: string, discussionId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.likeDiscussionSvc.save(new LikeDiscussionRequest(classRoomId, lessonId, commentId, discussionId, userId)).$promise;
        }

        public UpdateDiscussion(classRoomId: string, lessonId: string, commentId: string, discussionId: string, isDelete: boolean, message: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.updateDiscussionSvc.UpdateDiscussion(new UpdateDiscussionRequest(discussionId, classRoomId, lessonId, commentId, userId, isDelete, message)).$promise;
        }
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
    interface IGetUserProfileResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetUserProfile(data: T): T;
    }
    export class GetProfileService {

        private getAllCourseSvc: IGetAllCourseResourceClass<any>;
        private getNotificationNumberSvc: IGetNotificationNumberClass<any>;
        private getNotificationContentSvc: IGetNotificationContentClass<any>;
        private getLikeSvc: IGetLikeClass<any>;
        private getAllLikeSvc: IGetAllLikeClass<any>;
        private getUserProfileSvc: IGetUserProfileResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.getAllCourseSvc = <IGetAllCourseResourceClass<any>>$resource(appConfig.GetAllCourserofileUrl, { 'id': '@id' });
            this.getNotificationNumberSvc = <IGetNotificationNumberClass<any>>$resource(appConfig.GetNotificationNumberUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
            this.getNotificationContentSvc = <IGetNotificationContentClass<any>>$resource(appConfig.GetNotificationContentUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
            this.getLikeSvc = <IGetLikeClass<any>>$resource(appConfig.GetLiketUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'lessonId': '@lessonId' });
            this.getAllLikeSvc = <IGetAllLikeClass<any>>$resource(appConfig.GetAllLiketUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
            this.getUserProfileSvc = <IGetUserProfileResourceClass<any>>$resource(appConfig.GetUserProfileUrl, {});
        }

        public GetUserProfile(): ng.IPromise<any> {
            return this.getUserProfileSvc.get().$promise;
        }

        public GetAllCourse(): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.getAllCourseSvc.query(new GetAllCourseRequest(userId)).$promise;
        }
        public GetNotificationNumber(): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            var classroomId = this.userprofileSvc.ClientUserProfile.CurrentClassRoomId;
            return this.getNotificationNumberSvc.get(new GetNotificationNumberRequest(userId, classroomId)).$promise;
        }
        public GetNotificationContent(): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            var classroomId = this.userprofileSvc.ClientUserProfile.CurrentClassRoomId;
            return this.getNotificationContentSvc.query(new GetNotificationContentRequest(userId, classroomId)).$promise;
        }
        public GetLike(): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            var classroomId = this.userprofileSvc.ClientUserProfile.CurrentClassRoomId;
            var lessonId = this.userprofileSvc.ClientUserProfile.CurrentDisplayLessonId;
            return this.getLikeSvc.get(new GetLikeRequest(userId, classroomId, lessonId)).$promise;
        }
        public GetAllLike(): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            var classroomId = this.userprofileSvc.ClientUserProfile.CurrentClassRoomId;
            var lessonId = this.userprofileSvc.ClientUserProfile.CurrentLessonId;
            return this.getAllLikeSvc.get(new GetAllLikeRequest(userId, classroomId)).$promise;
        }
    }

    interface ISendContactResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SendContact(data: T): T;
    }

    export class ContactUsService {

        private sendContactSvc: ISendContactResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.sendContactSvc = <ISendContactResourceClass<any>>$resource(appConfig.SendContactUrl, { 'Name': '@Name', 'Email': '@Email', 'Message': '@Message' }, { SendContact: { method: 'POST' } });
        }
        
        public SendContact(name: string, email: string, message: string): ng.IPromise<any> {
            return this.sendContactSvc.SendContact(new SendContactRequest(name, email, message)).$promise;
        }
    }
    angular
        .module('app.shared')
        .service('app.shared.ClientUserProfileService', ClientUserProfileService)
        .service('app.shared.CommentService', CommentService)
        .service('app.shared.DiscussionService', DiscussionService)
        .service('app.shared.GetProfileService', GetProfileService)
        .service('app.shared.ContactUsService', ContactUsService);
}