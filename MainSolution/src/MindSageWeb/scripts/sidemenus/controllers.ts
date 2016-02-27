module app.sidemenus {
    'use strict';

    class SideMenuController {
        
        private userProfile: any;

        static $inject = ['$scope', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, private userSvc: app.shared.ClientUserProfileService) {
            this.userProfile = userSvc.GetClientUserProfile();
        }

        public GetUserProfileId(): string {
            return encodeURI(this.userProfile.UserProfileId);
        }

    }

    angular
        .module('app.sidemenus')
        .controller('app.sidemenus.SideMenuController', SideMenuController);
}