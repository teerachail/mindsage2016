module app.notification {
    'use strict';
    
    class NotificationController {

        private userInfo: any;
        private tag: any;
        private displayNpotifications;

        static $inject = ['$scope', '$state', 'app.shared.ClientUserProfileService', 'notification', 'app.shared.GetProfileService'];
        constructor(private $scope, private $state, private userSvc: app.shared.ClientUserProfileService, public notification:any[], private getProfile: app.shared.GetProfileService) {
            this.userInfo = userSvc.GetClientUserProfile();
            this.displayNpotifications = this.notification.filter(it=> it.FromUserProfiles != null);
        }

        public OpenJournalPage(name: string, userId: string) {
            this.$state.go("app.course.teacherlist", { 'classRoomId': this.userInfo.CurrentClassRoomId, 'targetUserId': userId }, { inherit: false });
        }

        public GetFirstLiker(name: any) {
            return name[0];
        }

        
    }


    angular
        .module('app.notification')
        .controller('app.notification.NotificationController', NotificationController);
}