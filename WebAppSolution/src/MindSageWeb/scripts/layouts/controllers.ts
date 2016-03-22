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

        public SendFriendRequest(friendObj: any) {
            this.userSvc.UserInCourseList.filter(it=> it == friendObj)[0].Status = 0;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, null, false);
        }

        public ConfirmFriendRequest(friendObj: any) {
            this.userSvc.UserInCourseList.filter(it=> it == friendObj)[0].Status = 2;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, friendObj.RequestId, true);
        }

        public DeleteFriendRequest(friendObj: any) {
            this.userSvc.UserInCourseList.filter(it=> it == friendObj)[0].Status = 3;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, friendObj.RequestId, false);
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
                var lessonId = userprofile.CurrentLessonId;
                var classRoomId = userprofile.CurrentClassRoomId;
                (<any>angular.element(".owl-carousel")).owlCarousel({
                    autoPlay: true,
                    slideSpeed: 300,
                    jsonPath: this.defaultUrl + '/api/lesson/' + lessonId + '/' + classRoomId + '/ads',
                    singleItem: true
                });
            }, error => {
                this.prepareUserprofile();
            });
        }
    }

    angular
        .module('app.layouts')
        .controller('app.layouts.AppLayoutController', AppLayoutController)
        .controller('app.layouts.LessonLayoutController', LessonLayoutController)
        .controller('app.layouts.CourseLayoutController', CourseLayoutController);
}