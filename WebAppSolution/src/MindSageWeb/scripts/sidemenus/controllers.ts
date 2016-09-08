module app.sidemenus {
    'use strict';

    class SideMenuController {
        
        private currentUserId: string;
        public notification: number;

        static $inject = ['$scope', '$state', 'app.shared.ClientUserProfileService', 'app.shared.GetProfileService', '$location', '$anchorScroll'];
        constructor(private $scope, private $state, private userSvc: app.shared.ClientUserProfileService, private profileSvc: app.shared.GetProfileService, private $location, private $anchorScroll) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            this.userSvc.PrepareAllUserProfile().then(() => {
                this.currentUserId = encodeURI(this.userSvc.ClientUserProfile.UserProfileId);
                this.loadNotifications();
            });
        }
        private loadNotifications(): void {
            this.profileSvc.GetNotificationNumber()
                .then(respond => {
                    if (respond == null) this.notification = 0;
                    else this.notification = respond.notificationTotal;
                }, error => {
                    console.log('Load notifications content failed');
                });
        }

        public ChangeCourse(classRoomId: string, lessonId: string, classCalendarId: string) {
            this.userSvc.ChangeCourse(classRoomId).then(respond => {
                this.userSvc.UpdateCourseInformation(respond);
                this.userSvc.ClientUserProfile.CurrentLessonId = lessonId;
                this.userSvc.ClientUserProfile.CurrentClassCalendarId = classCalendarId;
                this.$state.go("app.main.lesson", { 'classRoomId': classRoomId, 'lessonId': lessonId }, { inherit: false });
            }, error => {
                console.log('Change course failed');
            });
        }

        public ToTop() {
            this.$location.hash('main-bg');
            this.$anchorScroll();
        }
    }

    angular
        .module('app.sidemenus')
        .controller('app.sidemenus.SideMenuController', SideMenuController);
}