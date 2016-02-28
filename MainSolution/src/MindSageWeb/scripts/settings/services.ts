module app.settings {
    'use strict';
    
    interface IUpdateProfileResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        UpdateProfile(data: T): T;
    }
    interface IUpdatePCoursetResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        UpdateCourse(data: T): T;
    }
    interface IDeletePCoursetResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        DeleteCourse(data: T): T;
    }
    export class ProfileService {

        private svc: IUpdateProfileResourceClass<any>;
        private updateCoursesvc: IUpdatePCoursetResourceClass<any>;
        private deleteCoursesvc: IDeletePCoursetResourceClass<any>;
        

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.svc = <IUpdateProfileResourceClass<any>>$resource(appConfig.UpdateProfileUrl, {
                'id': '@id', 'Name': '@Name', 'SchoolName': '@SchoolName', 'IsPrivate': '@IsPrivate', 'IsReminderOnceTime': '@IsReminderOnceTime'
            }, { UpdateProfile: { method: 'PUT' } });
            this.updateCoursesvc = <IUpdatePCoursetResourceClass<any>>$resource(appConfig.UpdateCourseUrl, {
                'id': '@id', 'classRoomId': '@classRoomId', 'ClassName': '@ClassName', 'ChangedStudentCode': '@ChangedStudentCode'
            }, { UpdateCourse: { method: 'PUT' } });
            this.deleteCoursesvc = <IDeletePCoursetResourceClass<any>>$resource(appConfig.DeleteCourseUrl, {
                'ClassRoomId': '@ClassRoomId', 'UserProfileId': '@UserProfileId'
            });
        }

        public UpdateProfile(name: string, schoolName: string, isPrivate: boolean, isReminderOnceTime: boolean): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.svc.UpdateProfile(new UpdateProfileRequest(userId, name, schoolName, isPrivate, isReminderOnceTime)).$promise;
        }

        public UpdateCourse(ClassName: string, ChangedStudentCode: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
            return this.updateCoursesvc.UpdateCourse(new UpdateCourseRequest(userId, classroomId, ClassName, ChangedStudentCode)).$promise;
        }
        public DeleteCourse(ClassRoomId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.deleteCoursesvc.save(new DeleteCourseRequest(ClassRoomId, userId)).$promise;
        }
    }

    angular
        .module('app.settings')
        .service('app.settings.ProfileService', ProfileService);
}