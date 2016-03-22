module app.sidemenus {
    'use strict';

    class SideMenuController {
        
        private currentUserId: string;
        public notification: number;
        private isGetNotificationCompleted: boolean;
        private isWaittingForGetNotification: boolean;

        static $inject = ['$scope', '$state', 'waitRespondTime', 'app.shared.ClientUserProfileService', 'app.shared.GetProfileService'];
        constructor(private $scope, private $state, private waitRespondTime, private userSvc: app.shared.ClientUserProfileService, private profileSvc: app.shared.GetProfileService) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            if (!this.userSvc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }

            this.currentUserId = encodeURI(this.userSvc.ClientUserProfile.UserProfileId);
            this.loadNotifications();
        }
        private loadNotifications(): void {
            var shouldRequestUserNotifications = !this.isGetNotificationCompleted && !this.isWaittingForGetNotification;
            if (shouldRequestUserNotifications) {
                this.isWaittingForGetNotification = true;
                this.profileSvc.GetNotificationNumber()
                    .then(respond => {
                        this.isGetNotificationCompleted = true;
                        this.isWaittingForGetNotification = false;
                        if (respond == null) this.notification = 0;
                        else this.notification = respond.notificationTotal;
                    }, error => {
                        this.isWaittingForGetNotification = false;
                        setTimeout(it => this.loadNotifications(), this.waitRespondTime);
                    });
            }
        }

        public ChangeCourse(classRoomId: string, lessonId: string) {
            this.userSvc.ChangeCourse(classRoomId).then(respond => {
                this.userSvc.UpdateCourseInformation(respond);
                var userProfile = this.userSvc.GetClientUserProfile();
                userProfile.CurrentLessonId = lessonId;
                this.userSvc.UpdateUserProfile(userProfile);
                this.$state.go("app.main.lesson", { 'classRoomId': classRoomId, 'lessonId': lessonId }, { inherit: false });
            }, error => {
                console.log('Change course failed, retrying ...');
                setTimeout(it => this.ChangeCourse(classRoomId, lessonId), this.waitRespondTime);
            });
        }
    }

    angular
        .module('app.sidemenus')
        .controller('app.sidemenus.SideMenuController', SideMenuController);
}