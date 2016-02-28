﻿module app.settings {
    'use strict';
    
    class SettingController {

        static $inject = ['$scope', 'app.settings.ProfileService', 'app.shared.GetProfileService'];
        constructor(private $scope, private profileSvc: app.settings.ProfileService, private getprofileSvc: app.shared.GetProfileService) {
        }

        public UpdateProfile(name: string, schoolName: string, isPrivate: boolean, isReminderOnceTime: boolean) {
            this.profileSvc.UpdateProfile(name, schoolName, isPrivate, isReminderOnceTime);
        }
        public GetProfile() {
            this.getprofileSvc.GetProfile();
        }
        public GetCourse() {
            this.getprofileSvc.GetCourse();
        }
        public UpdateCoursee(ClassName: string, ChangedStudentCode :string) {
            this.profileSvc.UpdateCourse(ClassName, ChangedStudentCode);
        }
        public DeleteCourse(ClassRoomId: string) {
            this.profileSvc.DeleteCourse(ClassRoomId);
        }
    }

    angular
        .module('app.settings')
        .controller('app.settings.SettingController', SettingController);
}