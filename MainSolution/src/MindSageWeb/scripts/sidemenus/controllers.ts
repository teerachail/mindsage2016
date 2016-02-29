module app.sidemenus {
    'use strict';

    class SideMenuController {
        
        private userProfile: any;

        static $inject = ['$scope', 'app.shared.ClientUserProfileService', 'app.sidemenus.SideMenuService'];
        constructor(private $scope, private userSvc: app.shared.ClientUserProfileService, private sideMenuSvc: app.sidemenus.SideMenuService) {
            this.userProfile = userSvc.GetClientUserProfile();
        }

        public GetUserProfileId(): string {
            return encodeURI(this.userProfile.UserProfileId);
        }

        public ChangeTab(name: string) {
            this.sideMenuSvc.CurrentTabName = name;
        }
    }

    angular
        .module('app.sidemenus')
        .controller('app.sidemenus.SideMenuController', SideMenuController);
}