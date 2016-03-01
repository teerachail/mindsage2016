module app.studentlists {
    'use strict';

    class studentlistsController {
        private nStatus: number;
        static $inject = ['$scope', 'list', 'classRoomId', 'app.studentlists.StudentListService'];
        constructor(private $scope, public list: any[], public classRoomId: string, private listsSvc: app.studentlists.StudentListService) {
        }

        public SendFriendRequest(friendObj: any) {
            var EditIndex = this.list.indexOf(friendObj);
            this.list[EditIndex].Status = 0;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, null, false);
        }

        public ConfirmFriendRequest(friendObj: any) {
            var EditIndex = this.list.indexOf(friendObj);
            this.list[EditIndex].Status = 2;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, friendObj.RequestId, true);
        }

        public DeleteFriendRequest(friendObj: any) {
            var EditIndex = this.list.indexOf(friendObj);
            this.list[EditIndex].Status = 3;
            this.listsSvc.SendFriendRequest(friendObj.UserProfileId, friendObj.RequestId, false);

        }
        //UserProfileId
        //RequestId
    }

    angular
        .module('app.studentlists')
        .controller('app.studentlists.studentlistsController', studentlistsController);
}