module app.layouts {
    'use strict';

    class AppLayoutController {

        public targetUser: any;

        static $inject = ['app.studentlists.StudentListService', 'app.shared.ClientUserProfileService'];
        constructor(private listsSvc: app.studentlists.StudentListService, private userSvc: app.shared.ClientUserProfileService) {
        }
        
        public FriendsStatus(friendId: string) {
            if (this.userSvc.ClientUserProfile.UserProfileId == friendId) return 2;
            return this.userSvc.UserInCourseList.filter(it=> it.UserProfileId == friendId)[0].Status; 
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
            });
        }
    }

    angular
        .module('app.layouts')
        .controller('app.layouts.AppLayoutController', AppLayoutController)
        .controller('app.layouts.LessonLayoutController', LessonLayoutController)
        .controller('app.layouts.CourseLayoutController', CourseLayoutController);
}