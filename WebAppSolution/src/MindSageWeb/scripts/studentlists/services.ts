module app.studentlists {
    'use strict';

    //interface IStudentListResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
    //    GetStudentList(data: T): T;
    //}

    interface IFriendRequestResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SendFriendRequest(data: T): T;
    }

    export class StudentListService {

        //private GetStudentListsvc: IStudentListResourceClass<any>;
        private SendFriendRequestsvc: IFriendRequestResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            //this.GetStudentListsvc = <IStudentListResourceClass<any>>$resource(appConfig.StudentListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId' });
            this.SendFriendRequestsvc = <IFriendRequestResourceClass<any>>$resource(appConfig.SendFriendRequestUrl,
                { 'FromUserProfileId': '@FromUserProfileId', 'ToUserProfileId': '@ToUserProfileId', 'RequestId': '@RequestId', 'IsAccept': '@IsAccept' });
        }

        //public GetStudentList(classRoomId: string): ng.IPromise<any> {
        //    var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
        //    return this.GetStudentListsvc.query(new GetFriendListRequest(userId, classRoomId)).$promise;
        //}

        public SendFriendRequest(ToUserProfileId: string, RequestId: string, IsAccept: boolean): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.SendFriendRequestsvc.save(new SendFriendRequest(userId, ToUserProfileId, RequestId, IsAccept)).$promise;
        }

    }

    angular
        .module('app.studentlists')
        .service('app.studentlists.StudentListService', StudentListService);
}