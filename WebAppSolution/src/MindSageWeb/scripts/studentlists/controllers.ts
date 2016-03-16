module app.studentlists {
    'use strict';

    class studentlistsController {

        static $inject = ['$scope', 'classRoomId', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, public classRoomId: string, private userSvc: app.shared.ClientUserProfileService) {
            this.userSvc.ReloadFriendLists();
        }
        
    }

    angular
        .module('app.studentlists')
        .controller('app.studentlists.studentlistsController', studentlistsController);
}