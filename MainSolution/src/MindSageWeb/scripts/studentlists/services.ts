module app.studentlists {
    'use strict';

    interface IStudentListResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetStudentList(data: T): T;
    }

    export class StudentListService {

        private svc: IStudentListResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.svc = <IStudentListResourceClass<any>>$resource(appConfig.StudentListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId'});
        }

        public GetStudentList(classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.svc.query(new GetFriendListRequest(userId, classRoomId)).$promise;
        }

    }

    angular
        .module('app.studentlists')
        .service('app.studentlists.StudentListService', StudentListService);
}