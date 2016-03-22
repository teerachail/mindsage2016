module app.studentlists {
    'use strict';

    interface IFriendRequestResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SendFriendRequest(data: T): T;
    }

    export class StudentListService {

        private SendFriendRequestsvc: IFriendRequestResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.SendFriendRequestsvc = <IFriendRequestResourceClass<any>>$resource(appConfig.SendFriendRequestUrl,
                { 'FromUserProfileId': '@FromUserProfileId', 'ToUserProfileId': '@ToUserProfileId', 'RequestId': '@RequestId', 'IsAccept': '@IsAccept' });
        }

        public SendFriendRequest(ToUserProfileId: string, RequestId: string, IsAccept: boolean): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.SendFriendRequestsvc.save(new SendFriendRequest(userId, ToUserProfileId, RequestId, IsAccept)).$promise;
        }

    }

    angular
        .module('app.studentlists')
        .service('app.studentlists.StudentListService', StudentListService);
}