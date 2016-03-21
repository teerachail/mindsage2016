module app.settings {
    'use strict';
    
    class SettingController {

        private userInfo: shared.ClientUserProfile;

        static $inject = ['$scope', '$state', 'waitRespondTime', 'app.settings.ProfileService', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, private $state, private waitRespondTime, private profileSvc: app.settings.ProfileService, private clientProfileSvc: app.shared.ClientUserProfileService) {
            this.userInfo = new shared.ClientUserProfile();
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            if (!this.clientProfileSvc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }

            this.userInfo = this.clientProfileSvc.GetClientUserProfile();
        }
        
        public UpdateProfile(name: string, schoolName: string, isPrivate: boolean, isReminderOnceTime: boolean) {
            if (name != null && name != "") this.profileSvc.UpdateProfile(name, schoolName, isPrivate, isReminderOnceTime);
        }

        public UpdateCoursee(ClassName: string, ChangedStudentCode: string, BeginDate: Date) {
            if (this.userInfo.ClassName == ClassName) ClassName = null;
            if (this.userInfo.CurrentStudentCode == ChangedStudentCode) ChangedStudentCode = null;
            if (ClassName != null || ChangedStudentCode != null) this.profileSvc.UpdateCourse(ClassName, ChangedStudentCode, BeginDate);
        }

        public DeleteCourse() {
            this.profileSvc.DeleteCourse(this.userInfo.CurrentClassRoomId);
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