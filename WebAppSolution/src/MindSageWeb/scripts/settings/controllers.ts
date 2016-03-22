module app.settings {
    'use strict';
    
    class SettingController {

        static $inject = ['$scope', '$state', 'app.settings.ProfileService', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, private $state, private profileSvc: app.settings.ProfileService, private clientProfileSvc: app.shared.ClientUserProfileService) {
        }
        
        public UpdateProfile(name: string, schoolName: string, isPrivate: boolean, isReminderOnceTime: boolean) {
            if (name != null && name != "") this.profileSvc.UpdateProfile(name, schoolName, isPrivate, isReminderOnceTime);
        }

        public UpdateCoursee(ClassName: string, ChangedStudentCode: string, BeginDate: Date) {
            if (this.clientProfileSvc.ClientUserProfile.ClassName == ClassName) ClassName = null;
            if (this.clientProfileSvc.ClientUserProfile.CurrentStudentCode == ChangedStudentCode) ChangedStudentCode = null;
            if (ClassName != null || ChangedStudentCode != null) this.profileSvc.UpdateCourse(ClassName, ChangedStudentCode, BeginDate);
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