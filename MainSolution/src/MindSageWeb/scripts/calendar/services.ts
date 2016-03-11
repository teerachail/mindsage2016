module app.calendar {
    'use strict';
    interface IGetCourseScheduleResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetCourseSchedule(data: T): T;
    }
    export class CourseScheduleService {

        private getCourseScheduleSvc: IGetCourseScheduleResourceClass<any>;


        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.getCourseScheduleSvc = <IGetCourseScheduleResourceClass<any>>$resource(appConfig.CourseScheduleUrl, { 'id': '@id', 'classRoomId': '@classRoomId'});
        }

        public GetCourseSchedule(): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classRoomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            return this.getCourseScheduleSvc.get(new CourseScheduleRequest(userId, classRoomId)).$promise;
        }

    }
    angular
        .module('app.calendar')
        .service('app.calendar.CourseScheduleService', CourseScheduleService);
}