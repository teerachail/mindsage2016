module app.calendar {
    'use strict';
    interface IGetCourseScheduleResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetCourseSchedule(data: T): T;
    }
    interface ISetCourseStartDateResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SetCourseStartDate(data: T): T;
    }
    interface ISetCourseScheduleRangeResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SetCourseScheduleRange(data: T): T;
    }
    interface ISetCourseScheduleWeekResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SetCourseScheduleWeek(data: T): T;
    }
    interface IApplyToAllCourseResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        ApplyToAllCourse(data: T): T;
    }
    export class CourseScheduleService {

        private getCourseScheduleSvc: IGetCourseScheduleResourceClass<any>;
        private setCourseStartDateSvc: ISetCourseStartDateResourceClass<any>;
        private setCourseScheduleRangeSvc: ISetCourseScheduleRangeResourceClass<any>;
        private setCourseScheduleWeekSvc: ISetCourseScheduleWeekResourceClass<any>;
        private applyToAllCourseSvc: IApplyToAllCourseResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.getCourseScheduleSvc = <IGetCourseScheduleResourceClass<any>>$resource(appConfig.CourseScheduleUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
            this.setCourseStartDateSvc = <ISetCourseStartDateResourceClass<any>>$resource(appConfig.CourseStartDateUrl, { 'userProfileId': '@userProfileId', 'classRoomId': '@classRoomId', 'beginDate': '@beginDate' });
            this.setCourseScheduleRangeSvc = <ISetCourseScheduleRangeResourceClass<any>>$resource(appConfig.CourseScheduleRangeUrl, {
                'userProfileId': '@userProfileId', 'classRoomId': '@classRoomId', 'isHoliday': '@isHoliday', 'isShift': '@isShift',
                'fromDate': '@fromDate', 'toDate': '@toDate'
            });
            this.setCourseScheduleWeekSvc = <ISetCourseScheduleWeekResourceClass<any>>$resource(appConfig.CourseScheduleWeekUrl, {
                'userProfileId': '@userProfileId', 'classRoomId': '@classRoomId', 'isHoliday': '@isHoliday', 'isShift': '@isShift',
                'isSunday': '@isSunday', 'isMonday': '@isMonday', 'isTuesday': '@isTuesday', 'isWednesday': '@isWednesday', 'isTursday': '@isTursday', 'isFriday': '@isFriday', 'isSaturday': 'isSaturday'
            });
            this.applyToAllCourseSvc = <IApplyToAllCourseResourceClass<any>>$resource(appConfig.ApplyToAllCourseUrl, { 'userProfileId': '@userProfileId', 'classRoomId': '@classRoomId' });
        }

        public GetCourseSchedule(): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classRoomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            return this.getCourseScheduleSvc.get(new CourseScheduleRequest(userId, classRoomId)).$promise;
        }

        public SetCourseStartDate(beginDate: Date): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classRoomId  = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            return this.setCourseStartDateSvc.save(new SetCourseStartDateRequest(userId, classRoomId, beginDate)).$promise;
        }

        public SetCourseScheduleRange(isHoliday: boolean, isShift: boolean, fromDate: Date, toDate: Date): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classRoomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            return this.setCourseScheduleRangeSvc.save(new SetCourseScheduleRangeRequest(userId, classRoomId, isHoliday, isShift, fromDate, toDate)).$promise;
        }
        public SetCourseScheduleWeek(isHoliday: boolean, isShift: boolean, isSunday: boolean, isMonday: boolean, isTuesday: boolean, isWednesday: boolean, isTursday: boolean, isFriday: boolean, isSaturday: boolean): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classRoomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            return this.setCourseScheduleWeekSvc.save(new SetCourseScheduleWeekRequest(userId, classRoomId, isHoliday, isShift, isSunday, isMonday, isTuesday, isWednesday, isTursday, isFriday, isSaturday)).$promise;
        }
        public ApplyToAllCourse(): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classRoomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            return this.applyToAllCourseSvc.save(new ApplyToAllCourseRequest(userId, classRoomId)).$promise;
        }

    }
    angular
        .module('app.calendar')
        .service('app.calendar.CourseScheduleService', CourseScheduleService);
}