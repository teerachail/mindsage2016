module app.sidemenus {
    'use strict';

    class SideMenuController {
        
        private currentUserId: string;
        private userProfile: shared.ClientUserProfile;
        public notification: number;
        public AllAvailableCourses: shared.CourseCatalog[] = [];
        private isGetNotificationCompleted: boolean;
        private isWaittingForGetNotification: boolean;

        static $inject = ['$scope', '$state', 'waitRespondTime', 'app.shared.ClientUserProfileService', 'app.shared.GetProfileService'];
        constructor(private $scope, private $state, private waitRespondTime, private userSvc: app.shared.ClientUserProfileService, private notificationSvc: app.shared.GetProfileService) {
            this.userProfile = new shared.ClientUserProfile();
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            if (!this.userSvc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }

            this.userProfile = this.userSvc.GetClientUserProfile();
            this.AllAvailableCourses = this.userSvc.GetAllAvailableCourses();
            this.currentUserId = encodeURI(this.userProfile.UserProfileId);
            this.loadNotifications();
        }
        private loadNotifications(): void {
            var shouldRequestUserNotifications = !this.isGetNotificationCompleted && !this.isWaittingForGetNotification;
            if (shouldRequestUserNotifications) {
                this.isWaittingForGetNotification = true;
                this.notificationSvc.GetNotificationNumber()
                    .then(respond => {
                        this.isGetNotificationCompleted = true;
                        this.isWaittingForGetNotification = false;
                        if (respond == null) this.notification = 0;
                        else this.notification = respond.notificationTotal;
                    }, error=> {
                        this.isWaittingForGetNotification = false;
                        setTimeout(it => this.loadNotifications(), this.waitRespondTime);
                    });
            }
        }

        public ChangeClassRoom(classRoomId: string, lessonId: string, className: string) {
            var userProfile = this.userSvc.GetClientUserProfile();
            userProfile.CurrentClassRoomId = classRoomId;
            userProfile.CurrentLessonId = lessonId;
            userProfile.ClassName = className;
            // TODO: Update user profile
            //userProfile.CurrentStudentCode
            //userProfile.IsTeacher
            //userProfile.NumberOfStudents
            //userProfile.StartDate
            this.userSvc.UpdateUserProfile(userProfile);
            console.log('Change class room: ' + classRoomId + ', lessonId: ' + lessonId);
            this.$state.go("app.main.lesson", { 'classRoomId': classRoomId, 'lessonId': lessonId }, { inherit: false });
        }
    }

    angular
        .module('app.sidemenus')
        .controller('app.sidemenus.SideMenuController', SideMenuController);
}