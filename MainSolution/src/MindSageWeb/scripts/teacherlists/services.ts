module app.teacherlists {
    'use strict';

    interface ITeacherListResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetStudentList(data: T): T;
    }

    export class TeacherListService {

        private svc: ITeacherListResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.svc = <ITeacherListResourceClass<any>>$resource(appConfig.TeacherListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId'});
        }

        public GetStudentList(classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.svc.query(new GetStudentListRequest(userId, classRoomId)).$promise;
        }

    }

    angular
        .module('app.teacherlists')
        .service('app.teacherlists.TeacherListService', TeacherListService);
}