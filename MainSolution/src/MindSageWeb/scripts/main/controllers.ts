module app.main {
    'use strict';

    class MainController {

        public targetUser: any;

        static $inject = ['app.studentlists.StudentListService', 'app.shared.ClientUserProfileService'];
        constructor(private listsSvc: app.studentlists.StudentListService, private userSvc: app.shared.ClientUserProfileService) {
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

    angular
        .module('app.main')
        .controller('app.main.MainController', MainController);
}