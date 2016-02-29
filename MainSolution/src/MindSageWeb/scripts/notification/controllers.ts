module app.notification {
    'use strict';
    
    class NotificationController {
        private Test;
        public MessageNoti: string;
        static $inject = ['$scope', 'notification','app.shared.GetProfileService'];
        constructor(private $scope, public notification,private getProfile: app.shared.GetProfileService) {
            this.Test = "456"
            this.MessageNoti = notification.id;
        }

    }


    angular
        .module('app.notification')
        .controller('app.notification.NotificationController', NotificationController);
}