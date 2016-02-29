module app.sidemenus {
    'use strict';

    class SideMenuController {
        
        private userProfile: any;
        public notification: number;

        static $inject = ['$scope', '$state', 'app.shared.ClientUserProfileService', 'app.sidemenus.SideMenuService', 'app.shared.GetProfileService'];
        constructor(private $scope, private $state, private userSvc: app.shared.ClientUserProfileService, private sideMenuSvc: app.sidemenus.SideMenuService, private notificationSvc: app.shared.GetProfileService) {
            this.userProfile = userSvc.GetClientUserProfile();
            this.notificationSvc.GetNotificationNumber()
                .then(it=> {
                    if (it == null) {
                        this.notification = 0;
                    }
                    else this.notification = it.notificationTotal;
                });
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