module app.courses {
    'use strict';

    interface ICourseMapContentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
    }

    export class CourseMapService {

        private svc: ICourseMapContentResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.svc = <ICourseMapContentResourceClass<any>>$resource(appConfig.CourseMapContentUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
        }

        public GetContent(classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.svc.get(new GetCourseMapContentRequest(userId, classRoomId)).$promise;
        }

    }

    angular
        .module('app.courses')
        .service('app.courses.CourseMapService', CourseMapService);
}