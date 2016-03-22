module app.settings {
    'use strict';
    
    class SettingController {

        static $inject = ['$scope', '$state', 'app.settings.ProfileService', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, private $state, private profileSvc: app.settings.ProfileService, private clientProfileSvc: app.shared.ClientUserProfileService) {
        }
        
        public UpdateProfile() {
            if (this.userInfo.FullName != null && this.userInfo.FullName != "")
                this.profileSvc.UpdateProfile(this.userInfo.FullName, this.userInfo.SchoolName, this.userInfo.IsPrivateAccout, this.userInfo.IsReminderOnceTime);
        }

        public UpdateCoursee() {
            //HACK: condition for send data
            this.profileSvc.UpdateCourse(this.userInfo.ClassName, this.userInfo.CurrentStudentCode, this.userInfo.StartDate);
        }

        public DeleteCourse() {
            this.profileSvc.DeleteCourse(this.clientProfileSvc.ClientUserProfile.CurrentClassRoomId);
        }

        public StudenMessageEdit(Message: string) {
            const NoneContentLength = 0;
            if (Message.length <= NoneContentLength) return Message;

            this.profileSvc.StudenMessageEdit(Message);
            return "";
        }
    }

    angular
        .module('app.settings')
        .controller('app.settings.SettingController', SettingController);
}