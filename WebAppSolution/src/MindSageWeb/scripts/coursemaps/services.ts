module app.coursemaps {
    'use strict';

    interface ICourseMapContentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetContent(data: T): T;
        GetLessonStatus(data: T): T;
    }

    export class CourseMapService {

        private svc: ICourseMapContentResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.svc = <ICourseMapContentResourceClass<any>>$resource(appConfig.CourseMapContentUrl, { 'id': '@id', 'classRoomId': '@classRoomId' }, {
                GetContent: { method: 'GET', isArray: true, },
                GetLessonStatus: { method: 'GET', isArray: true, params: { 'action': 'status' } },
            });
        }

        public GetContent(classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.svc.GetContent(new GetCourseMapContentRequest(userId, classRoomId)).$promise;
        }

        public GetLessonStatus(classRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.ClientUserProfile.UserProfileId;
            return this.svc.GetLessonStatus(new GetCourseMapContentRequest(userId, classRoomId)).$promise;
        }

    }

    angular
        .module('app.coursemaps')
        .service('app.coursemaps.CourseMapService', CourseMapService);
}