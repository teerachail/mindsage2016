module app.notification {
    'use strict';
    
    class NotificationController {

        private userInfo: any;

        static $inject = ['$scope', '$state', 'app.shared.ClientUserProfileService', 'notification', 'app.shared.GetProfileService', 'app.sidemenus.SideMenuService'];
        constructor(private $scope, private $state, private userSvc: app.shared.ClientUserProfileService, public notification, private getProfile: app.shared.GetProfileService, private sideMenuSvc: app.sidemenus.SideMenuService) {
            this.userInfo = userSvc.GetClientUserProfile();
        }

        public OpenJournalPage(name: string, userId: string) {
            this.sideMenuSvc.CurrentTabName = name;
            this.$state.go("app.course.teacherlist", { 'classRoomId': this.userInfo.CurrentClassRoomId, 'targetUserId': userId }, { inherit: false });
        }

    }


    angular
        .module('app.notification')
        .controller('app.notification.NotificationController', NotificationController);
}