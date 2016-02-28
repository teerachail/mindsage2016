module app.settings {
    'use strict';
    
    class SettingController {

        static $inject = ['$scope', 'app.settings.ProfileService'];
        constructor(private $scope, private profileSvc: app.settings.ProfileService) {
        }

        public UpdateProfile(name: string, schoolName: string, isPrivate: boolean, isReminderOnceTime: boolean) {
            this.profileSvc.UpdateProfile(name, schoolName, isPrivate, isReminderOnceTime);
        }
    }

    angular
        .module('app.settings')
        .controller('app.settings.SettingController', SettingController);
}