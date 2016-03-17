module app.studentlists {
    'use strict';

    class studentlistsController {

        private classRoomId: string;
        private friendList: any[] = [];

        static $inject = ['$scope', '$stateParams', 'waitRespondTime', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, private $stateParams, private waitRespondTime, private userSvc: app.shared.ClientUserProfileService) {
            this.classRoomId = $stateParams.classRoomId;
            this.prepareUserprofile();
        }
        
        private prepareUserprofile(): void {
            if (!this.userSvc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }

            this.friendList = this.userSvc.GetFriendLists();
        }
    }

    angular
        .module('app.studentlists')
        .controller('app.studentlists.studentlistsController', studentlistsController);
}