module app.notification {
    'use strict';
    
    class NotificationController {

        private tag: any;
        private displayNpotifications;
        private notification: any[] = [];
        private isWaittingForGetNotificationContent: boolean;
        private isPrepareNotificationContentComplete: boolean;

        static $inject = ['$scope', '$state', 'waitRespondTime', 'app.shared.ClientUserProfileService', 'app.shared.GetProfileService'];
        constructor(private $scope, private $state, private waitRespondTime, private userSvc: app.shared.ClientUserProfileService, private getProfile: app.shared.GetProfileService) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            if (!this.userSvc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }

            this.prepareNotificationContents();
        }

        private prepareNotificationContents(): void {
            var shouldRequestNotificationContent = !this.isPrepareNotificationContentComplete && !this.isWaittingForGetNotificationContent;
            if (shouldRequestNotificationContent) {
                this.isWaittingForGetNotificationContent = true;
                this.getProfile.GetNotificationContent().then(respond => {
                    if (respond != null) {
                        this.notification = respond;
                        this.displayNpotifications = this.notification.filter(it => it.FromUserProfiles != null);
                    }
                    this.isWaittingForGetNotificationContent = false;
                    this.isPrepareNotificationContentComplete = true;
                }, error => {
                    console.log('Load notification content failed, retrying ...');
                    this.isWaittingForGetNotificationContent = false;
                    setTimeout(it => this.prepareNotificationContents(), this.waitRespondTime);
                });
            }
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