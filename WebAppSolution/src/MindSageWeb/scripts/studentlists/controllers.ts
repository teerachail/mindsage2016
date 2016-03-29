module app.studentlists {
    'use strict';

    class studentlistsController {

        private classRoomId: string;
        public keyword: string;
        private friendList: any[] = [];
        private searchList: any[] = [];
        private searching: boolean;

        static $inject = ['$scope', '$stateParams', 'app.shared.ClientUserProfileService', 'app.studentlists.StudentListService'];
        constructor(private $scope, private $stateParams, private userSvc: app.shared.ClientUserProfileService, private searchSvc: app.studentlists.StudentListService) {
            this.classRoomId = $stateParams.classRoomId;
            this.searching = false;
        }

        public searchNewFriend() {
            if (this.keyword == null || this.keyword.length == 0) return;
            this.searching = true;
            this.searchSvc.SearchFriendRequest(this.classRoomId, this.keyword).then(respond => {
                this.searchList = respond;
            });
        }

        public cancelSearch() {
            this.keyword = "";
            this.searching = false;
        }
    }

    angular
        .module('app.studentlists')
        .controller('app.studentlists.studentlistsController', studentlistsController);
}