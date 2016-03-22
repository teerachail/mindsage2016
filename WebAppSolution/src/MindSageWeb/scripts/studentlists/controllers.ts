module app.studentlists {
    'use strict';

    class studentlistsController {

        private classRoomId: string;
        private friendList: any[] = [];

        static $inject = ['$scope', '$stateParams', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, private $stateParams, private userSvc: app.shared.ClientUserProfileService) {
            this.classRoomId = $stateParams.classRoomId;
        }
    }

    angular
        .module('app.studentlists')
        .controller('app.studentlists.studentlistsController', studentlistsController);
}