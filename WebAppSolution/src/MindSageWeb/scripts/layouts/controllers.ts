module app.layouts {
    'use strict';

    class AppLayoutController {

        public targetUser: any;

        static $inject = ['app.studentlists.StudentListService', 'app.shared.ClientUserProfileService'];
        constructor(private listsSvc: app.studentlists.StudentListService, private userSvc: app.shared.ClientUserProfileService) {
        }

        public CanAccessToUserJournal(targetUserProfileId): boolean {
            var isHimselfOrTeacher = this.userSvc.ClientUserProfile.UserProfileId == targetUserProfileId
                || this.userSvc.ClientUserProfile.IsTeacher;
            if (isHimselfOrTeacher) return true;

            var selectedUser = this.userSvc.UserInCourseList.filter(it => it.UserProfileId == targetUserProfileId);
            if (selectedUser[0].IsTeacher) return true;

            const UserNotFound = 0;
            const FriendStatus = 2;
            var canAccess = selectedUser != null
                && selectedUser.length > UserNotFound
                && selectedUser[0].Status == FriendStatus;
            return canAccess;
        }

        public targetData(friendId: string) {
            var targetObj = this.userSvc.UserInCourseList.filter(it=> it.UserProfileId == friendId)[0]; 
            if (targetObj == null)
                return;
            else
                this.targetUser = targetObj;
        }

        public targetNewData(friendObj: any) {
                this.targetUser = friendObj;
        }

        public SendFriendRequest(friendObj: any) {
            var Index = this.userSvc.UserInCourseList.indexOf(friendObj);
            if (this.userSvc.ClientUserProfile.IsSelfPurchase && Index == -1) this.userSvc.UserInCourseList.push(friendObj);
            this.userSvc.UserInCourseList.filter(it=> it == friendObj)[0].Status = 0;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, null, false);
            if (this.userSvc.ClientUserProfile.IsSelfPurchase) this.userSvc.PrepareAllUserProfile();
        }

        public ConfirmFriendRequest(friendObj: any) {
            this.userSvc.UserInCourseList.filter(it=> it == friendObj)[0].Status = 2;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, friendObj.RequestId, true);
        }

        public DeleteFriendRequest(friendObj: any) {
            this.userSvc.UserInCourseList.filter(it=> it == friendObj)[0].Status = 3;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, friendObj.RequestId, false);
            if (this.userSvc.ClientUserProfile.IsSelfPurchase) {
                var removeIndex = this.userSvc.UserInCourseList.indexOf(friendObj);
                if (removeIndex > -1) this.userSvc.UserInCourseList.splice(removeIndex, 1);
                this.userSvc.PrepareAllUserProfile();
            }
        }
    }

    class LessonLayoutController {

        static $inject = ['app.shared.ClientUserProfileService'];
        constructor(private clientSvc: app.shared.ClientUserProfileService) {
        }

    }

    class CourseLayoutController {

        static $inject = ['defaultUrl', 'app.shared.ClientUserProfileService'];
        constructor(private defaultUrl, private clientUserProfileSvc: app.shared.ClientUserProfileService) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            this.clientUserProfileSvc.PrepareAllUserProfile().then(() => {
                var userprofile = this.clientUserProfileSvc.ClientUserProfile;
                var lessonId = userprofile.CurrentDisplayLessonId;
                if (lessonId == null)
                    lessonId = this.clientUserProfileSvc.ClientUserProfile.CurrentLessonId;
                var classRoomId = userprofile.CurrentClassRoomId;
                (<any>angular.element(".owl-carousel")).owlCarousel({
                    autoPlay: true,
                    slideSpeed: 300,
                    jsonPath: this.defaultUrl + '/api/lesson/' + lessonId + '/' + classRoomId + '/ads',
                    singleItem: true
                });
            });
        }
    }

    class ContactUsController {

        public Name: string;
        public Email: string;
        public Message: string;
        public send: boolean;

        static $inject = ['app.shared.ContactUsService'];
        constructor(private ContactSvc: app.shared.ContactUsService) {
            this.send = false;
        }

        private SendContact(): void {
            if (this.Name == null || this.Name == "") return;
            if (this.Email == null || this.Email == "") return;
            if (this.Email.indexOf("@") == -1 || this.Email.indexOf(".") == -1 || this.Email.lastIndexOf("@") > this.Email.lastIndexOf(".")) return;
            if (this.Message == null || this.Message == "") return;
            this.ContactSvc.SendContact(this.Name, this.Email, this.Message).then(() => {
                this.send = true;
            });
        }


    }

    angular
        .module('app.layouts')
        .controller('app.layouts.AppLayoutController', AppLayoutController)
        .controller('app.layouts.LessonLayoutController', LessonLayoutController)
        .controller('app.layouts.CourseLayoutController', CourseLayoutController)
        .controller('app.layouts.ContactUsController', ContactUsController);
}