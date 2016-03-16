module app.layouts {
    'use strict';

    class AppLayoutController {

        public targetUser: any;
        public classRoomId: string;

        static $inject = ['waitRespondTime', 'app.studentlists.StudentListService', 'app.shared.ClientUserProfileService'];
        constructor(private waitRespondTime, private listsSvc: app.studentlists.StudentListService, private userSvc: app.shared.ClientUserProfileService) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            if (!this.userSvc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }
            this.classRoomId = this.userSvc.GetClientUserProfile().CurrentClassRoomId;
        }
        
        public FriendsStatus(friendId: string) {
            if (this.userSvc.GetClientUserProfile().UserProfileId == friendId) return 2;
            return this.userSvc.GetFriendLists().filter(it=> it.UserProfileId == friendId)[0].Status; 
        }

        public targetData(friendId: string) {
            var targetObj = this.userSvc.GetFriendLists().filter(it=> it.UserProfileId == friendId)[0]; 
            if (targetObj == null)
                return;
            else
                this.targetUser = targetObj;
        }

        public SendFriendRequest(friendObj: any) {
            this.userSvc.GetFriendLists().filter(it=> it == friendObj)[0].Status = 0;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, null, false);
        }

        public ConfirmFriendRequest(friendObj: any) {
            this.userSvc.GetFriendLists().filter(it=> it == friendObj)[0].Status = 2;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, friendObj.RequestId, true);
        }

        public DeleteFriendRequest(friendObj: any) {
            this.userSvc.GetFriendLists().filter(it=> it == friendObj)[0].Status = 3;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, friendObj.RequestId, false);
        }
    }

    class LessonLayoutController {
        public RunningVideoUrl: string;

        static $inject = ['$sce'];
        constructor(private $sce) {
        }

        public ChangeVideo(url: string) {
            this.RunningVideoUrl = this.$sce.trustAsResourceUrl(url);
        }
    }

    class CourseLayoutController {

        private ads: any;

        static $inject = ['app.shared.ClientUserProfileService'];
        constructor(private clientUserProfileSvc: app.shared.ClientUserProfileService) {
            this.ads = this.clientUserProfileSvc.Advertisments;
        }
    }

    angular
        .module('app.layouts')
        .controller('app.layouts.AppLayoutController', AppLayoutController)
        .controller('app.layouts.LessonLayoutController', LessonLayoutController)
        .controller('app.layouts.CourseLayoutController', CourseLayoutController);
}