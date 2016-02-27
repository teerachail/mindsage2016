module app.studentlists {
    'use strict';

    class studentlistsController {
        static $inject = ['$scope', 'list', 'classRoomId', 'app.studentlists.StudentListService'];
        constructor(private $scope, public list, public classRoomId: string, private listsSvc: app.studentlists.StudentListService) {
        }

        public SendFriendRequest(SendtoId: string) {
            this.listsSvc.SendFriendRequest(SendtoId, null, false);
        }

        public ConfirmFriendRequest(SendtoId: string, requestId: string) {
            this.listsSvc.SendFriendRequest(SendtoId, requestId, true);
        }

        public DeleteFriendRequest(SendtoId: string, requestId: string) {
            this.listsSvc.SendFriendRequest(SendtoId, requestId, false);
        }

    }

    angular
        .module('app.studentlists')
        .controller('app.studentlists.studentlistsController', studentlistsController);
}