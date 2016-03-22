module app.settings {
    'use strict';
    
    class SettingController {

        static $inject = ['$scope', '$state', 'app.settings.ProfileService', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, private $state, private profileSvc: app.settings.ProfileService, private clientProfileSvc: app.shared.ClientUserProfileService) {
        }
        
        public UpdateProfile() {
            var FullName = this.clientProfileSvc.ClientUserProfile.FullName;
            var SchoolName = this.clientProfileSvc.ClientUserProfile.SchoolName;
            var IsPrivateAccout = this.clientProfileSvc.ClientUserProfile.IsPrivateAccout;
            var IsReminderOnceTime = this.clientProfileSvc.ClientUserProfile.IsReminderOnceTime;
            if (FullName != null && FullName != "")
                this.profileSvc.UpdateProfile(FullName, SchoolName, IsPrivateAccout, IsReminderOnceTime);
        }

        public UpdateCoursee() {
            //HACK: condition for send data
            var ClassName = this.clientProfileSvc.ClientUserProfile.ClassName;
            var CurrentStudentCode = this.clientProfileSvc.ClientUserProfile.CurrentStudentCode;
            var StartDate = this.clientProfileSvc.ClientUserProfile.StartDate;
            this.profileSvc.UpdateCourse(ClassName, CurrentStudentCode, StartDate);
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