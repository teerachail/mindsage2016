module app.sidemenus {
    'use strict';

    class SideMenuController {
        
        private userProfile: shared.ClientUserProfile;
        public notification: number;
        public AllAvailableCourses: shared.CourseCatalog[] = [];

        static $inject = ['$scope', '$state', 'waitRespondTime', 'app.shared.ClientUserProfileService', 'app.sidemenus.SideMenuService', 'app.shared.GetProfileService'];
        constructor(private $scope, private $state, private waitRespondTime, private userSvc: app.shared.ClientUserProfileService, private sideMenuSvc: app.sidemenus.SideMenuService, private notificationSvc: app.shared.GetProfileService) {
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
            this.loadNotifications();
        }

        private isGetNotificationCompleted: boolean;
        private isWaittingForGetNotification: boolean;
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
        

        public GetUserProfileId(): string {
            return encodeURI(this.userProfile.UserProfileId);
        }

        public ChangeTab(name: string) {
            this.sideMenuSvc.CurrentTabName = name;
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
            this.$state.go("app.main.lesson", { 'classRoomId': classRoomId, 'lessonId': lessonId }, { inherit: false });
        }
    }

    angular
        .module('app.sidemenus')
        .controller('app.sidemenus.SideMenuController', SideMenuController);
}