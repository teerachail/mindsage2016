module app.studentlists {
    'use strict';

    interface IFriendRequestResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SendFriendRequest(data: T): T;
    }

    interface ISearchFriendRequestResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SearchFriendRequestsvc(data: T): T;
    }

    export class StudentListService {

        private SendFriendRequestsvc: IFriendRequestResourceClass<any>;
        private SearchFriendRequestsvc: ISearchFriendRequestResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.SendFriendRequestsvc = <IFriendRequestResourceClass<any>>$resource(appConfig.SendFriendRequestUrl,
                { 'FromUserProfileId': '@FromUserProfileId', 'ToUserProfileId': '@ToUserProfileId', 'RequestId': '@RequestId', 'IsAccept': '@IsAccept' });
            this.SearchFriendRequestsvc = <ISearchFriendRequestResourceClass<any>>$resource(appConfig.SearchFriendUrl,
                { 'userId': '@userId', 'classRoomId': '@classRoomId', 'key': '@key'});
        }

        public SendFriendRequest(ToUserProfileId: string, RequestId: string, IsAccept: boolean): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.SendFriendRequestsvc.save(new SendFriendRequest(userId, ToUserProfileId, RequestId, IsAccept)).$promise;
        }

        public SearchFriendRequest(classRoomId: string, keyword: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.SearchFriendRequestsvc.query(new SearchFriendRequest(userId, classRoomId, keyword)).$promise;
        }

    }

    angular
        .module('app.studentlists')
        .service('app.studentlists.StudentListService', StudentListService);
}