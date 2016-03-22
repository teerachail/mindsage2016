module app.notification {
    'use strict';
    
    class NotificationController {

        private tag: any;
        private displayNpotifications;
        private notification: any[] = [];

        static $inject = ['$scope', '$state', 'app.shared.ClientUserProfileService', 'app.shared.GetProfileService'];
        constructor(private $scope, private $state, private userSvc: app.shared.ClientUserProfileService, private getProfile: app.shared.GetProfileService) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            this.userSvc.PrepareAllUserProfile().then(() => {
                this.prepareNotificationContents();
            });
        }

        private prepareNotificationContents(): void {
            this.getProfile.GetNotificationContent().then(respond => {
                if (respond != null) {
                    this.notification = respond;
                    this.displayNpotifications = this.notification.filter(it => it.FromUserProfiles != null);
                }
            }, error => {
                console.log('Load notification content failed');
            });
        }

        public OpenJournalPage(name: string, userId: string) {
            this.$state.go("app.course.journal", { 'classRoomId': this.userSvc.ClientUserProfile.CurrentClassRoomId, 'targetUserId': userId }, { inherit: false });
        }

        public GetFirstLiker(name: any) {
            return name[0];
        }
    }

    angular
        .module('app.notification')
        .controller('app.notification.NotificationController', NotificationController);
}