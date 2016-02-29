module app.settings {
    'use strict';
    
    class SettingController {
        
        private userInfo;
        public ClassName: string;
        public CurrentStudentCode: string;

        static $inject = ['$scope', 'app.settings.ProfileService', 'courseInfo', 'app.shared.ClientUserProfileService', 'app.shared.GetProfileService'];
        constructor(private $scope, private profileSvc: app.settings.ProfileService, public courseInfo, private clientProfileSvc: app.shared.ClientUserProfileService, private getProfile: app.shared.GetProfileService) {
            this.userInfo = this.clientProfileSvc.GetClientUserProfile();
            this.ClassName = this.courseInfo.ClassName;
            this.CurrentStudentCode = this.courseInfo.CurrentStudentCode;
        }

        public UpdateProfile(name: string, schoolName: string, isPrivate: boolean, isReminderOnceTime: boolean) {
            if (name != null && name != "")
                this.profileSvc.UpdateProfile(name, schoolName, isPrivate, isReminderOnceTime);
        }

        public UpdateCoursee(ClassName: string, ChangedStudentCode: string, BeginDate: Date) {
            if (this.courseInfo.ClassName == ClassName) ClassName = null;
            if (this.courseInfo.CurrentStudentCode == ChangedStudentCode) ChangedStudentCode = null;
            if (ClassName != null || ChangedStudentCode != null)
                this.profileSvc.UpdateCourse(ClassName, ChangedStudentCode, BeginDate);
        }

        public DeleteCourse() {
            this.profileSvc.DeleteCourse(this.courseInfo.ClassRoomId);
        }
        public StudenMessageEdit(Message: string) {
            this.profileSvc.StudenMessageEdit(Message);
        }
        public GetAllCourse() {
            this.getProfile.GetAllCourse();
        }

    }

    angular
        .module('app.settings')
        .controller('app.settings.SettingController', SettingController);
}