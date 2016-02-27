module app.teacherlists {
    'use strict';

    interface ITeacherListResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetStudentList(data: T): T;
    }

    interface ITeacherRemoveStdResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        RemoveStudent(data: T): T;
    }

    export class TeacherListService {

        private GetStudentListsvc: ITeacherListResourceClass<any>;
        private RemoveStudentsvc: ITeacherRemoveStdResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.GetStudentListsvc = <ITeacherListResourceClass<any>>$resource(appConfig.TeacherListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId' });
            this.RemoveStudentsvc = <ITeacherRemoveStdResourceClass<any>>$resource(appConfig.TeacherRemoveStdUrl,
                { 'classRoomId': '@classRoomId', 'UserProfileId': '@UserProfileId', 'RemoveUserProfileId': '@RemoveUserProfileId' });
        }

        public GetStudentList(classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.GetStudentListsvc.query(new GetStudentListRequest(userId, classRoomId)).$promise;
        }

        public RemoveStudent(classRoomId: string, removeId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.RemoveStudentsvc.save(new RemoveStudentRequest(classRoomId, userId, removeId)).$promise;
        }

    }

    angular
        .module('app.teacherlists')
        .service('app.teacherlists.TeacherListService', TeacherListService);
}